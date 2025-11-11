using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Logic for the individual items in the quest selection section of the
/// Planner App
/// </summary>
public class PlannerQuestItem : MonoBehaviourR3
{
    private static UnityEventR3<QuestTrackingDatum> _onSelected = new();
    public static IDisposable OnSelected(Action<QuestTrackingDatum> x) => _onSelected.Subscribe(x);

    private static UnityEventR3<QuestTrackingDatum> _onMadeActive = new();
    public static IDisposable OnMadeActive(Action<QuestTrackingDatum> x) => _onMadeActive.Subscribe(x);

    [field: SerializeField]
    private TextMeshProUGUI TextQuestName { get; set; }

    [field: SerializeField]
    private GameObject ActiveQuestIndicator { get; set; }

    [field: SerializeField]
    private GameObject SelectedQuestIndent { get; set; }

    [field: SerializeField]
    private GameObject BackgroundSelected { get; set; }

    [field: SerializeField]
    private GameObject BackgroundCompleted { get; set; }

    [field: SerializeField]
    private GameObject NewUpdateIndicator { get; set; }

    [field: SerializeField]
    private GameObject QuestCompleteIndicator { get; set; }

    /// <summary>
    /// Check for null or use <see cref="GetQuestTrackingDatumAsync"/>
    /// </summary>
    public QuestTrackingDatum Quest { get; private set; }

    public bool IsSelected { get; private set; }

    /// <summary>
    /// Should only be called once immediately after instantiation
    /// </summary>
    /// <param name="value"></param>
    public async Task SetQuest(QuestTrackingDatum value)
    {
        Quest = value;
        var questData = await Quest.GetQuestDataAsync();
        TextQuestName.text = questData.QuestName;
        UpdateUI();
    }

    public async Task<QuestTrackingDatum> GetQuestTrackingDatumAsync()
    {
        while (Quest == null)
        {
            await Task.Yield();
            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        return Quest;
    }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PersistentGameData.GameEvents.OnQuestUpdated(UpdateUI));
    }

    private void UpdateUI()
    {
        if (Quest.IsCompleted)
        {
            Complete();
            return;
        }

        QuestCompleteIndicator.SetActive(Quest.IsCompleted);
        BackgroundCompleted.SetActive(Quest.IsCompleted);
        ActiveQuestIndicator.SetActive(Quest.IsActive);
        NewUpdateIndicator.SetActive(Quest.HasUnreadUpdates && !IsSelected);
    }

    private void UpdateUI(QuestTrackingDatum quest)
    {
        if (quest.IsActive && Quest != quest)
        {
            ActiveQuestIndicator.SetActive(false);
        }

        if (quest != Quest && !BackgroundSelected.activeSelf)
        {
            NewUpdateIndicator.SetActive(true);
        }

        if (quest != Quest) return;

        UpdateUI();
    }

    public void Select()
    {
        BackgroundSelected.SetActive(true);
        SelectedQuestIndent.SetActive(true);
        NewUpdateIndicator.SetActive(false);
        IsSelected = true;
        _onSelected?.Invoke(Quest);
    }

    public void Deselect()
    {
        BackgroundSelected.SetActive(false);
        SelectedQuestIndent.SetActive(false);
        IsSelected = false;
    }

    public void Complete()
    {
        QuestCompleteIndicator.SetActive(true);
        BackgroundCompleted.SetActive(true);
        ActiveQuestIndicator.SetActive(false);
    }

    public void MakeQuestActive()
    {
        ActiveQuestIndicator.SetActive(true);
        _onMadeActive?.Invoke(Quest);
    }
}