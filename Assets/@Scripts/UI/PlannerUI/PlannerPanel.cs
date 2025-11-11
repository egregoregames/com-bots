using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Threading.Tasks;
using System;
using TMPro;
using R3.Triggers;
using Sirenix.OdinInspector;
using UnityEngine.UI;

/// <summary>
/// Logic that controls the "Planner" app in the <see cref="PauseMenu"/>
/// </summary>
public class PlannerPanel : MonoBehaviourR3
{
    public static PlannerPanel Instance { get; private set; }

    [field: SerializeField, ReadOnly]
    private List<PlannerQuestItem> InstantiatedQuestItems { get; set; }

    [field: SerializeField]
    private GameObject QuestItemTemplate { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI QuestDescription { get; set; }

    [field: SerializeField]
    private ScrollRect ScrollRectQuestItems { get; set; }

    [field: SerializeField, ReadOnly]
    private int SelectedQuestElective { get; set; } = -1;

    [field: SerializeField, ReadOnly]
    private int SelectedQuestRequirement { get; set; } = -1;

    [field: SerializeField, ReadOnly]
    private QuestType QuestType { get; set; }

    [field: SerializeField, ReadOnly]
    private bool RefreshInProgress { get; set; }

    private new void Awake()
    {
        base.Awake();
        InstantiatedQuestItems.Clear();
    }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = this;

        AddEvents(
            ComBotsSaveSystem.OnLoadSuccess(RefreshQuestItems),
            PlannerQuestItem.OnSelected(UpdateSelected),
            Inputs.UI_Right(_ => SetQuestType(QuestType.Elective)),
            Inputs.UI_Left(_ => SetQuestType(QuestType.Requirement)),
            Inputs.UI_Down(_ => SetSelectedQuest(1)),
            Inputs.UI_Up(_ => SetSelectedQuest(-1)),

            // Bad for performance but we can worry about that after the MVP
            PersistentGameData.GameEvents.OnQuestUpdated(_ => RefreshQuestItems()));
    }

    private void Start()
    {
        RefreshQuestItems();
        gameObject.SetActive(false);
    }

    private void SetQuestType(QuestType type)
    {
        QuestType = type;
        RefreshQuestItems();
    }

    private void SetSelectedQuest(int increment)
    {
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
    }

    private async void UpdateSelected(QuestTrackingDatum quest)
    {
        var data = await quest.GetQuestDataAsync();
        var type = data.QuestType;

        if (type == QuestType.Elective)
        {
            SelectedQuestElective = quest.QuestId;
        }
        else
        {
            SelectedQuestRequirement = quest.QuestId;
        }

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

        // Todo: Set scrollview
        // ScrollViewQuestItems ....
    }

    private async Task<IEnumerable<QuestTrackingDatum>> GetFilteredQuests()
    {
        // Await these to avoid null reference exceptions
        var gameData = await PersistentGameData.GetInstanceAsync();
        var staticGameDataInstance = await StaticGameData.GetInstanceAsync();

        var questsFilteredByType = gameData.PlayerQuestTrackingData
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
            InstantiatedQuestItems.Add(comp);
        }

        QuestItemTemplate.SetActive(false);
    }

    private void RestoreSelectedQuest()
    {
        int selected = QuestType == QuestType.Elective ?
        SelectedQuestElective : SelectedQuestRequirement;

        if (InstantiatedQuestItems.Count < 1) return;

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
        InstantiatedQuestItems.ForEach(x => Destroy(x.gameObject));
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
        await WaitForQuestRefreshToComplete();
        RefreshInProgress = true;

        try
        {
            Log($"Refreshing quest items: {QuestType}");
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