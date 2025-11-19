using R3;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Logic that controls the "Planner" app in the <see cref="PauseMenu"/>
/// </summary>
public partial class AppPlanner : PauseMenuAppSingleton<AppPlanner>
{
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

    [field: SerializeField, ReadOnly]
    private int SelectedQuestIdElective { get; set; } = -1;

    [field: SerializeField, ReadOnly]
    private int SelectedQuestIdRequirement { get; set; } = -1;

    [field: SerializeField]
    private PauseMenuAppScrollList<QuestTrackingDatum> ScrollList { get; set; }

    [field: SerializeField, ReadOnly]
    private QuestType QuestType { get; set; }

    private List<PauseMenuAppSelectableListItem<QuestTrackingDatum>> Items 
        => ScrollList.InstantiatedItems;

    #region Monobehaviour
    protected override void Awake()
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

        AddEvents(
            ComBotsSaveSystem.OnLoadSuccess(RefreshQuestItems),
            PlannerQuestItem.OnSelected(UpdateSelected),
            Inputs.UI_Right(_ => SetQuestType(QuestType.Elective)),
            Inputs.UI_Left(_ => SetQuestType(QuestType.Requirement)),
            Inputs.UI_Down(_ => SetSelected(1)),
            Inputs.UI_Up(_ => SetSelected(-1)),
            Inputs.UI_Submit(_ => SetSelectedQuestActive()),

            // Bad for performance but we can worry about that after the MVP
            PersistentGameData.GameEvents.OnQuestUpdated(_ => RefreshQuestItems()));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshQuestItems();
        UpdateSelectedQuestTypeUI();
    }
    #endregion

    private void SetSelected(int increment)
    {
        ScrollList.SetSelected(increment);
        PlaySoundNavigation();
    }

    private async void SetSelectedQuestActive()
    {
        if (Items.Count < 1) return;

        var selected = Items
            .First(x => x.IsSelected);

        var questData = await selected.GetDatumAsync();

        if (questData.IsCompleted || questData.IsActive)
            return;

        PlaySoundSubmit();

        await PersistentGameData.GetInstanceAsync();

        foreach (var item in PersistentGameData.Quests.GetAll())
        {
            item.IsActive = false;
        }

        foreach (var item in Items)
        {
            ((PlannerQuestItem)item).MakeQuestInactive();
        }

        questData.IsActive = true;
        ((PlannerQuestItem)selected).MakeQuestActive();
        ControlHintSetActiveQuest.SetActive(false);
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

        PlannerQuestItem selectedQuestItem = (PlannerQuestItem)Items
            .First(x => x.IsSelected);

        ScrollList.UpdateItemList(selectedQuestItem, Items);
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

    private void RestoreSelectedQuest()
    {
        int selected = QuestType == QuestType.Elective ?
        SelectedQuestIdElective : SelectedQuestIdRequirement;

        if (Items.Count < 1)
        {
            ScrollList.UpdateItemList(null, Items);
            return;
        }

        if (selected == -1)
        {
            Items.First().Select();
            Log($"Selected first quest in list", LogLevel.Verbose);
        }
        else
        {
            Items.First(x => x.Datum.QuestId == selected).Select();
            Log($"Selected last selected quest", LogLevel.Verbose);
        }
    }

    private void ClearInstantiatedQuestItems()
    {
        Items
            .Where(x => x != null)
            .ToList()
            .ForEach(x => Destroy(x.gameObject));

        Items.Clear();
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
            await ScrollList.InstantiateItems(all);
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