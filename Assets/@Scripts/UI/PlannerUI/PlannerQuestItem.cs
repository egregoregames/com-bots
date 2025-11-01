using System;
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

    private QuestTrackingDatum Quest { get; set; }

    /// <summary>
    /// Should only be called once immediately after instantiation
    /// </summary>
    /// <param name="value"></param>
    public void SetQuest(QuestTrackingDatum value)
    {
        Quest = value;
        ActiveQuestIndicator.SetActive(value.IsActive);
        //TextQuestName = todo, need to get the persistent data via quest ID
    }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PersistentGameData.GameEvents.OnQuestUpdated(UpdateUI));
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

        if (quest.IsCompleted)
        {
            Complete();
            return;
        }

        if (quest.IsActive)
        {
            
        }
    }

    public void Select()
    {
        BackgroundSelected.SetActive(true);
        SelectedQuestIndent.SetActive(true);
        NewUpdateIndicator.SetActive(false);
        _onSelected?.Invoke(Quest);
    }

    public void Deselect()
    {
        BackgroundSelected.SetActive(false);
        _onSelected?.Invoke(Quest);
    }

    public void Complete()
    {
        BackgroundCompleted.SetActive(true);
        ActiveQuestIndicator.SetActive(false);
    }

    public void MakeQuestActive()
    {
        ActiveQuestIndicator.SetActive(true);
        _onMadeActive?.Invoke(Quest);
    }
}