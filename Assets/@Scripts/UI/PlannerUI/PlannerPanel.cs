using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ComBots.Sandbox.Global.UI.Menu;
using System.Linq;
using System.Threading.Tasks;
using System;
using TMPro;
using R3.Triggers;
using UnityEngine.UIElements;

/// <summary>
/// Logic that controls the "Planner" app in the <see cref="PauseMenu"/>
/// </summary>
public class PlannerPanel : MonoBehaviourR3
{
    public static PlannerPanel Instance { get; private set; }

    [field: SerializeField]
    private List<PlannerQuestItem> QuestItems { get; set; }

    [field: SerializeField]
    private GameObject QuestItemTemplate { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI QuestDescription { get; set; }

    [field: SerializeField]
    private ScrollView ScrollViewQuestItems { get; set; }

    private InputSystem_Actions Inputs { get; set; }

    private int SelectedQuestElective { get; set; } = -1;
    private int SelectedQuestRequirement { get; set; } = -1;

    private QuestType QuestType { get; set; }

    private bool RefreshInProgress { get; set; }

    private new void Awake()
    {
        base.Awake();
        QuestItems.Clear();
    }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = this;
        Inputs = new();

        var onInputRight = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Right.performed += h,
            h => Inputs.UI.Right.performed -= h);

        var onInputLeft = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Left.performed += h,
            h => Inputs.UI.Left.performed -= h);

        var onInputDown = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Down.performed += h,
            h => Inputs.UI.Down.performed -= h);

        var onInputUp = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Up.performed += h,
            h => Inputs.UI.Up.performed -= h);

        AddEvents(
            ComBotsSaveSystem.OnLoadSuccess(RefreshQuestItems),
            PlannerQuestItem.OnSelected(UpdateSelected),
            onInputRight.Subscribe(_ => SetQuestType(QuestType.Elective)),
            onInputLeft.Subscribe(_ => SetQuestType(QuestType.Requirement)),
            onInputDown.Subscribe(_ => SetSelectedQuest(1)),
            onInputUp.Subscribe(_ => SetSelectedQuest(-1)),

            // Bad for performance but we can worry about that after the MVP
            PersistentGameData.GameEvents.OnQuestUpdated(_ => RefreshQuestItems()));
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private new void OnEnable()
    {
        base.OnEnable();
        Inputs.Enable();
    }

    private void OnDisable()
    {
        Inputs.Disable();
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

        if (QuestItems.Count <= 1) return;

        var selectedQuest = QuestItems.First(x => x.IsSelected);
        int selectedQuestIndex = QuestItems.IndexOf(selectedQuest);

        var newIndex = selectedQuestIndex + increment;
        if (newIndex < 0)
        {
            // Wrap back to bottom of quest list
            newIndex = QuestItems.Count - 1;
        }
        else if (newIndex > QuestItems.Count - 1)
        {
            // Wrap to top of quest list
            newIndex = 0;
        }

        selectedQuest.Deselect();
        QuestItems[newIndex].Select();
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

    // I'm sure we can make this more performance friendly later
    private async void RefreshQuestItems()
    {
        while (RefreshInProgress)
        {
            await Task.Yield();
            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        RefreshInProgress = true;

        try
        {
            Log("Refreshing quest items");
            QuestItems.ForEach(x => Destroy(x));
            QuestItems.Clear();

            var gameData = await PersistentGameData.GetInstanceAsync();

            // Just awaiting this to make sure its not null before proceeding
            var staticGameDataInstance = await StaticGameData.GetInstanceAsync();

            var questsFilteredByType = gameData.PlayerQuestTrackingData
                .Where(x => x.GetQuestData().QuestType == QuestType);

            var completed = questsFilteredByType
                .Where(x => x.IsCompleted)
                .OrderBy(x => x.QuestId)
                .ToList();

            var notCompleted = questsFilteredByType
                .Where(x => !x.IsCompleted)
                .OrderBy(x => x.QuestId)
                .ToList();

            var all = notCompleted.Concat(completed);

            QuestItemTemplate.SetActive(true);

            foreach (var item in all)
            {
                var newObj = Instantiate(
                    QuestItemTemplate, QuestItemTemplate.transform.parent);

                var comp = newObj.GetComponent<PlannerQuestItem>();

                // This guarantees that 
                await comp.SetQuest(item);
                QuestItems.Add(comp);
            }

            QuestItemTemplate.SetActive(false);

            int selected = QuestType == QuestType.Elective ? 
                SelectedQuestElective : SelectedQuestRequirement;

            if (selected == -1)
            {
                QuestItems.First().Select();
            }
            else
            {
                QuestItems.First(x => x.Quest.QuestId == selected).Select();
            }
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