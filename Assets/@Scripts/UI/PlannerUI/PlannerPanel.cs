using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using System;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

/// <summary>
/// Logic that controls the "Planner" app in the <see cref="PauseMenu"/>
/// </summary>
public class PlannerPanel : MonoBehaviourR3
{
    public static PlannerPanel Instance { get; private set; }

    [field: SerializeField]
    private int MaxQuestItemsOnScreen { get; set; } = 3;

    [field: SerializeField]
    private GameObject QuestItemTemplate { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI QuestDescription { get; set; }

    [field: SerializeField]
    private GameObject UpArrow { get; set; }

    [field: SerializeField]
    private GameObject DownArrow { get; set; }

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

    private new void Awake()
    {
        base.Awake();
        ClearInstantiatedQuestItems();
        SelectedQuestIdElective = -1; 
        SelectedQuestIdRequirement = -1;
        QuestType = QuestType.Requirement;
        RefreshInProgress = false;
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

        gameObject.SetActive(false);
    }

    private void SetQuestType(QuestType type)
    {
        QuestType = type;
        RefreshQuestItems();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        RefreshQuestItems();
    }

    private void SetSelectedQuest(int increment)
    {
        Log($"Incrementing selected quest index by {increment}");

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

    private void UpdateQuestDescription(QuestTrackingDatum quest, StaticQuestData data)
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
    }

    private void UpdateQuestList()
    {
        // Get total number of instantiated quest items
        int total = InstantiatedQuestItems.Count();

        // Get index of selected
        var selectedQuestItem = InstantiatedQuestItems.First(x => x.IsSelected);
        int selQuestInd = InstantiatedQuestItems.IndexOf(selectedQuestItem);

        int half = (int)Math.Floor(MaxQuestItemsOnScreen / 2d);
        int max = MaxQuestItemsOnScreen;

        bool totalGreaterThanMax = total > max;
        UpArrow.SetActive(selQuestInd > half && totalGreaterThanMax);
        DownArrow.SetActive((total - 1) - selQuestInd >= half && totalGreaterThanMax);

        foreach (var questItem in InstantiatedQuestItems)
        {
            UpdateQuestItemVisibility(
                total, selectedQuestItem, selQuestInd, half, max, questItem);
        }
    }

    private void UpdateQuestItemVisibility(
        int totalQuestItems, PlannerQuestItem selectedQuestItem, 
        int selectedQuestItemIndex, int halfOfMaxItemsOnScreen, 
        int MaxItemsOnScreen, PlannerQuestItem questItem)
    {
        if (questItem == selectedQuestItem)
        {
            questItem.gameObject.SetActive(true);
            return;
        }

        var localIndex = InstantiatedQuestItems.IndexOf(questItem);
        bool active;

        bool isNearTop = selectedQuestItemIndex <= halfOfMaxItemsOnScreen;
        bool isNearBottom = (totalQuestItems - 1) - selectedQuestItemIndex <= 
            halfOfMaxItemsOnScreen;

        if (isNearTop) 
        {
            active = localIndex < MaxItemsOnScreen;
        }
        else if (isNearBottom)
        {
            bool isActive = (totalQuestItems - 1) - localIndex <= MaxItemsOnScreen - 1;
            active = isActive;
        }
        else
        {
            active = Math.Abs(localIndex - selectedQuestItemIndex) <= 
                halfOfMaxItemsOnScreen;
        }

        questItem.gameObject.SetActive(active);
    }

    private async void UpdateSelected(QuestTrackingDatum quest)
    {
        Log($"Updating selected quest details (ID:{quest.QuestId})");
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

        UpdateQuestDescription(quest, data);
        UpdateQuestList();
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
            comp.Deselect();
            InstantiatedQuestItems.Add(comp);
        }

        QuestItemTemplate.SetActive(false);
    }

    private void RestoreSelectedQuest()
    {
        int selected = QuestType == QuestType.Elective ?
        SelectedQuestIdElective : SelectedQuestIdRequirement;

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
            Log($"Refreshing quest items: {QuestType}");
            ClearInstantiatedQuestItems();
            var all = await GetFilteredQuests();
            await InstantiateQuestItems(all);
            DownArrow.transform.SetAsLastSibling();
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