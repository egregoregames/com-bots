using R3;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Logic that controls the "Planner" app in the <see cref="PauseMenu"/>
/// </summary>
public partial class PlannerPanel : MonoBehaviourR3
{
    public static PlannerPanel Instance { get; private set; }

    [field: SerializeField]
    private GameObject QuestItemTemplate { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI QuestDescription { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextQuestTitle { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextQuestRewardCredits { get; set; }

    [field: SerializeField]
    private GameObject ControlHintSetActiveQuest { get; set; }

    [field: SerializeField]
    private GameObject ImageSelectedRequirements { get; set; }

    [field: SerializeField]
    private GameObject ImageSelectedElectives { get; set; } 

    [field: SerializeField]
    private AudioClip AudioClipNavigation { get; set; }

    [field: SerializeField]
    private AudioClip AudioClipSetQuestActive { get; set; }

    [field: SerializeField]
    private AudioClip AudioClipLeaveMenu { get; set; }

    [field: SerializeField]
    private PauseMenuAppScrollList ScrollList { get; set; }

    [field: SerializeField, ReadOnly]
    private List<PlannerQuestItem> InstantiatedQuestItems { get; set; }

    [field: SerializeField, ReadOnly]
    private int SelectedQuestIdElective { get; set; } = -1;

    [field: SerializeField, ReadOnly]
    private int SelectedQuestIdRequirement { get; set; } = -1;

    [field: SerializeField, ReadOnly]
    private QuestType QuestType { get; set; }

    [field: SerializeField, ReadOnly]
    private bool RefreshInProgress { get; set; }

    #region Monobehaviour
    private new void Awake()
    {
        base.Awake();
        ClearInstantiatedQuestItems();
        SelectedQuestIdElective = -1; 
        SelectedQuestIdRequirement = -1;
        QuestType = QuestType.Requirement;
    }

    protected override void Initialize()
    {
        base.Initialize();
        RefreshInProgress = false;
        Instance = this;

        AddEvents(
            ComBotsSaveSystem.OnLoadSuccess(RefreshQuestItems),
            PlannerQuestItem.OnSelected(UpdateSelected),
            Inputs.UI_Right(_ => SetQuestType(QuestType.Elective)),
            Inputs.UI_Left(_ => SetQuestType(QuestType.Requirement)),
            Inputs.UI_Down(_ => SetSelectedQuest(1)),
            Inputs.UI_Up(_ => SetSelectedQuest(-1)),
            Inputs.UI_Submit(_ => SetSelectedQuestActive()),
            Inputs.UI_Cancel(_ => Close()),
            Inputs.UI_OpenMenu(_ => Close()),

            // Bad for performance but we can worry about that after the MVP
            PersistentGameData.GameEvents.OnQuestUpdated(_ => RefreshQuestItems()));

        gameObject.SetActive(false);
    }

    private new void OnEnable()
    {
        base.OnEnable();
        RefreshQuestItems();
        UpdateSelectedQuestTypeUI();
    }
    #endregion

    private void Close()
    {
        if (!gameObject.activeInHierarchy) return;
        AudioManager.PlaySoundEffect(AudioClipLeaveMenu);
        gameObject.SetActive(false);
    }

    private async void SetSelectedQuestActive()
    {
        if (InstantiatedQuestItems.Count < 1) return;

        var selected = InstantiatedQuestItems
            .First(x => x.IsSelected);

        var questData = await selected.GetQuestTrackingDatumAsync();

        if (questData.IsCompleted || questData.IsActive)
            return;

        AudioManager.PlaySoundEffect(AudioClipSetQuestActive);

        await PersistentGameData.GetInstanceAsync();

        foreach (var item in PersistentGameData.Quests.GetAll())
        {
            item.IsActive = false;
        }

        foreach (var item in InstantiatedQuestItems)
        {
            item.MakeQuestInactive();
        }

        questData.IsActive = true;
        selected.MakeQuestActive();
        ControlHintSetActiveQuest.SetActive(false);
    }

    private void PlaySoundNavigation()
    {
        AudioManager.PlaySoundEffect(AudioClipNavigation);
    }

    private void SetQuestType(QuestType type)
    {
        QuestType = type;
        UpdateSelectedQuestTypeUI();
        RefreshQuestItems();
        PlaySoundNavigation();
    }

    private void UpdateSelectedQuestTypeUI()
    {
        ImageSelectedElectives.SetActive(QuestType == QuestType.Elective);
        ImageSelectedRequirements.SetActive(QuestType == QuestType.Requirement);
    }

    private void SetSelectedQuest(int increment)
    {
        Log($"Incrementing selected quest index by {increment}", LogLevel.Verbose);

        if (increment != 1 && increment != -1)
        {
            throw new Exception(
                "Improper usage. Argument must be 1 (next quest) or -1 (prev quest)");
        }

        if (InstantiatedQuestItems.Count <= 1) return;

        var selectedQuest = InstantiatedQuestItems.First(x => x.IsSelected);
        int selectedQuestIndex = InstantiatedQuestItems.IndexOf(selectedQuest);

        var newIndex = selectedQuestIndex + increment;
        if (newIndex < 0)
        {
            // Wrap back to bottom of quest list
            newIndex = InstantiatedQuestItems.Count - 1;
        }
        else if (newIndex > InstantiatedQuestItems.Count - 1)
        {
            // Wrap to top of quest list
            newIndex = 0;
        }

        selectedQuest.Deselect();
        InstantiatedQuestItems[newIndex].Select();
        PlaySoundNavigation();
    }

    private void UpdateQuestDetails(QuestTrackingDatum quest, StaticQuestDatum data)
    {
        int step = quest.CurrentStep;
        if (quest.IsCompleted)
        {
            QuestDescription.text = data.Steps.Last();
        }
        else if (data.Steps.Length > step)
        {
            QuestDescription.text = data.Steps[step];
        }
        else
        {
            QuestDescription.text = "ERROR: STEP OUT OF RANGE OR MISSING";
        }

        TextQuestRewardCredits.text = data.RewardCredits.ToString("0.0");
        TextQuestTitle.text = data.QuestName;
    }

    private async void UpdateSelected(QuestTrackingDatum quest)
    {
        Log($"Updating selected quest details (ID:{quest.QuestId})", LogLevel.Verbose);
        var data = await quest.GetQuestDataAsync();
        var type = data.QuestType;

        if (type == QuestType.Elective)
        {
            SelectedQuestIdElective = quest.QuestId;
        }
        else
        {
            SelectedQuestIdRequirement = quest.QuestId;
        }

        UpdateQuestDetails(quest, data);

        PlannerQuestItem selectedQuestItem = InstantiatedQuestItems
            .First(x => x.IsSelected);

        ScrollList.UpdateItemList(selectedQuestItem, InstantiatedQuestItems);
        ControlHintSetActiveQuest.SetActive(!quest.IsCompleted && !quest.IsActive);
    }

    private async Task<IEnumerable<QuestTrackingDatum>> GetFilteredQuests()
    {
        // Await these to avoid null reference exceptions
        var gameData = await PersistentGameData.GetInstanceAsync();
        var staticGameDataInstance = await StaticGameData.GetInstanceAsync();

        var questsFilteredByType = PersistentGameData.Quests.GetAll()
            .Where(x => x.GetQuestData().QuestType == QuestType);

        var completed = questsFilteredByType
            .Where(x => x.IsCompleted)
            .OrderBy(x => x.QuestId);

        var notCompleted = questsFilteredByType
            .Where(x => !x.IsCompleted)
            .OrderBy(x => x.QuestId);

        return notCompleted.Concat(completed);
    }

    private async Task InstantiateQuestItems(IEnumerable<QuestTrackingDatum> all)
    {
        QuestItemTemplate.SetActive(true);

        foreach (var item in all)
        {
            var newObj = Instantiate(
                QuestItemTemplate, QuestItemTemplate.transform.parent);

            var comp = newObj.GetComponent<PlannerQuestItem>();
            await comp.SetQuest(item);
            comp.Deselect();
            InstantiatedQuestItems.Add(comp);
        }

        QuestItemTemplate.SetActive(false);
    }

    private void RestoreSelectedQuest()
    {
        int selected = QuestType == QuestType.Elective ?
        SelectedQuestIdElective : SelectedQuestIdRequirement;

        if (InstantiatedQuestItems.Count < 1)
        {
            ScrollList.UpdateItemList(null, InstantiatedQuestItems);
            return;
        }

        if (selected == -1)
        {
            InstantiatedQuestItems.First().Select();
            Log($"Selected first quest in list", LogLevel.Verbose);
        }
        else
        {
            InstantiatedQuestItems.First(x => x.Quest.QuestId == selected).Select();
            Log($"Selected last selected quest", LogLevel.Verbose);
        }
    }

    private void ClearInstantiatedQuestItems()
    {
        InstantiatedQuestItems
            .Where(x => x != null)
            .ToList()
            .ForEach(x => Destroy(x.gameObject));

        InstantiatedQuestItems.Clear();
    }

    private async Task WaitForQuestRefreshToComplete()
    {
        while (RefreshInProgress)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }
    }

    // I'm sure we can make this more performance friendly later
    private async void RefreshQuestItems()
    {
        if (!gameObject.activeSelf) return;
        await WaitForQuestRefreshToComplete();
        RefreshInProgress = true;

        try
        {
            Log($"Refreshing quest items: {QuestType}", LogLevel.Verbose);
            ClearInstantiatedQuestItems();
            var all = await GetFilteredQuests();
            await InstantiateQuestItems(all);
            RestoreSelectedQuest();
        }
        catch (Exception e)
        {
            Log(e);
        }
        finally
        {
            RefreshInProgress = false;
        }
    }
}