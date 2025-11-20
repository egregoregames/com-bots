using System.Threading.Tasks;
using UnityEngine;

public class AppSocialyteListItem : PauseMenuAppSelectableListItem<NpcConnectionDatum>
{
    [field: SerializeField]
    private Color TextColorDark { get; set; }

    [field: SerializeField]
    private Color TextColorLight { get; set; }

    [field: SerializeField]
    private GameObject NewUpdateIndicator { get; set; }

    [field: SerializeField]
    private GameObject InPartyIndicatorSelected { get; set; }

    [field: SerializeField]
    private GameObject InPartyIndicatorNotSelected { get; set; }

    private bool IsInParty => PersistentGameData.Instance
        .PlayerNpcTeamMembers.Contains(Datum.NpcId);

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PersistentGameData.GameEvents.OnSocialyteProfileAdded(UpdateUI),
            PersistentGameData.GameEvents.OnTeamMembersChanged(UpdateUI));
    }

    /// <inheritdoc/>
    public override async Task SetDatum(NpcConnectionDatum datum)
    {
        await base.SetDatum(datum);
        var staticData = await Datum.GetStaticDataAsync();
        TextMain.text = staticData.ProfileName;
        UpdateUI();
    }

    private void UpdateUI()
    {
        NewUpdateIndicator.SetActive(Datum.HasNewUpdates && !IsSelected);
        InPartyIndicatorSelected.SetActive(IsInParty);
        InPartyIndicatorNotSelected.SetActive(IsInParty);
    }

    public override async void Select()
    {
        base.Select();
        NewUpdateIndicator.SetActive(false);

        if (Datum != null)
        {
            Datum.HasNewUpdates = false;
            await PersistentGameData.GetInstanceAsync();
            InPartyIndicatorSelected.SetActive(IsInParty);
        }

        TextMain.color = TextColorLight;
    }

    public override async void Deselect()
    {
        base.Deselect();
        TextMain.color = TextColorDark;

        await PersistentGameData.GetInstanceAsync();
        InPartyIndicatorNotSelected.SetActive(IsInParty);
    }

    private void UpdateUI(NpcConnectionDatum datum)
    {
        if (datum != Datum && !IsSelected)
        {
            string message = $"Activating NewUpdateIndicator " +
                $"for AppSocialyteListItem {TextMain.text}";

            Log(message, LogLevel.Verbose);

            NewUpdateIndicator.SetActive(true);
        }

        if (datum != Datum) return;

        UpdateUI();
    }
}