using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Logic for the individual items in the quest selection section of the
/// Planner App
/// </summary>
public class PlannerQuestItem : PauseMenuAppSelectableListItem<QuestTrackingDatum>
{
    [field: SerializeField]
    private Color TextColorDark { get; set; }

    [field: SerializeField]
    private Color TextColorLight { get; set; }

    [field: SerializeField]
    private GameObject ActiveQuestIndicator { get; set; }

    [field: SerializeField]
    private GameObject BackgroundCompleted { get; set; }

    [field: SerializeField]
    private GameObject NewUpdateIndicator { get; set; }

    [field: SerializeField]
    private GameObject QuestCompleteIndicator { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PersistentGameData.GameEvents.OnQuestUpdated(UpdateUI));
    }

    /// <inheritdoc/>
    public override async Task SetDatum(QuestTrackingDatum value)
    {
        await base.SetDatum(value);
        var questData = await Datum.GetQuestDataAsync();
        TextMain.text = questData.QuestName;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (Datum.IsCompleted)
        {
            NewUpdateIndicator.SetActive(Datum.HasUnreadUpdates);
            Complete();
            return;
        }

        QuestCompleteIndicator.SetActive(Datum.IsCompleted);
        BackgroundCompleted.SetActive(Datum.IsCompleted);
        ActiveQuestIndicator.SetActive(Datum.IsActive);
        NewUpdateIndicator.SetActive(Datum.HasUnreadUpdates && !IsSelected);
    }

    private void UpdateUI(QuestTrackingDatum quest)
    {
        if (quest.IsActive && Datum != quest)
        {
            ActiveQuestIndicator.SetActive(false);
        }

        if (quest != Datum && !IsSelected)
        {
            string message = $"Activating NewUpdateIndicator " +
                $"for QuestItem {TextMain.text}";

            Log(message, LogLevel.Verbose);

            NewUpdateIndicator.SetActive(true);
        }

        if (quest != Datum) return;

        UpdateUI();
    }

    public override void Select()
    {
        base.Select();
        NewUpdateIndicator.SetActive(false);

        if (Datum != null)
        {
            Datum.HasUnreadUpdates = false;
        }
        
        TextMain.color = TextColorLight;
    }

    public override void Deselect()
    {
        base.Deselect();

        if (Datum.IsCompleted)
        {
            TextMain.color = TextColorLight;
        }
        else
        {
            TextMain.color = TextColorDark;
        }
    }

    public void Complete()
    {
        QuestCompleteIndicator.SetActive(true);
        BackgroundCompleted.SetActive(true);
        ActiveQuestIndicator.SetActive(false);
        TextMain.color = TextColorLight;
    }

    public void MakeQuestInactive()
    {
        ActiveQuestIndicator.SetActive(false);
    }

    public void MakeQuestActive()
    {
        ActiveQuestIndicator.SetActive(true);
    }
}