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

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PersistentGameData.GameEvents.OnSocialyteProfileAdded(UpdateUI));
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
    }

    public override void Select()
    {
        base.Select();
        NewUpdateIndicator.SetActive(false);

        if (Datum != null)
        {
            Datum.HasNewUpdates = false;
        }

        TextMain.color = TextColorLight;
    }

    public override void Deselect()
    {
        base.Deselect();
        TextMain.color = TextColorDark;
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