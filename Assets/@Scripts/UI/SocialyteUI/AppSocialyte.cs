using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Contains logic for the socialyte app. Data from connections is pulled 
/// from <see cref="PersistentGameData.Socialyte"/>
/// </summary>
public class AppSocialyte : PauseMenuAppSingleton<AppSocialyte>
{
    [field: SerializeField]
    private TextMeshProUGUI TextBio { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextCheckInLocation { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextContactName { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextOccupation { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextOrigin { get; set; }

    [field: SerializeField]
    private GameObject[] BondHearts { get; set; }

    [field: SerializeField]
    private GameObject BondHeartContainer { get; set; }

    [field: SerializeField]
    private PauseMenuAppScrollList<NpcConnectionDatum> ScrollList { get; set; }

    [field: SerializeField]
    private GameObject ImageSelectedConnections { get; set; }

    [field: SerializeField]
    private GameObject ImageSelectedFeed { get; set; }

    [field: SerializeField, ReadOnly]
    private SocialyteTab CurrentTab { get; set; }

    [field: SerializeField, ReadOnly]
    private int SelectedConnectionNpcId { get; set; } = -1;

    private List<PauseMenuAppSelectableListItem<NpcConnectionDatum>> Items
        => ScrollList.InstantiatedItems;

    protected override void Awake()
    {
        base.Awake();
        ScrollList.ClearItems();
        SelectedConnectionNpcId = -1;
        CurrentTab = SocialyteTab.Connections;
    }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            ComBotsSaveSystem.OnLoadSuccess(RefreshItems),
            AppSocialyteListItem.OnSelected(UpdateSelected),
            Inputs.UI_Right(_ => SetTab(SocialyteTab.Feed)),
            Inputs.UI_Left(_ => SetTab(SocialyteTab.Connections)),
            Inputs.UI_Down(_ => SetSelected(1)),
            Inputs.UI_Up(_ => SetSelected(-1)),

            // Bad for performance but we can worry about that after the MVP
            PersistentGameData.GameEvents.OnSocialyteProfileUpdated(_ => RefreshItems()));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshItems();
        UpdateSelectedTabUI();
    }

    private void SetSelected(int increment)
    {
        ScrollList.SetSelected(increment);
        PlaySoundNavigation();
    }

    private void SetTab(SocialyteTab type)
    {
        CurrentTab = type;
        UpdateSelectedTabUI();
        RefreshItems();
        PlaySoundNavigation();
    }

    private void UpdateSelectedTabUI()
    {
        ImageSelectedConnections.SetActive(
            CurrentTab == SocialyteTab.Connections);

        ImageSelectedFeed.SetActive(CurrentTab == SocialyteTab.Feed);
    }

    private void UpdateDetails(NpcConnectionDatum datum, 
        SocialyteProfileStaticDatum staticDatum)
    {
        UpdateFromStringArray(staticDatum.Bios, datum.CurrentBioStep, 
            TextBio, datum.NpcId);

        UpdateFromStringArray(staticDatum.CheckInLocations, 
            datum.CurrentCheckInLocationStep, TextCheckInLocation, datum.NpcId);

        UpdateBonds(staticDatum);

        TextOrigin.text = staticDatum.Origin;
        TextOccupation.text = staticDatum.Occupation;
        TextContactName.text = staticDatum.ProfileName;
        AppSocialyteNpcBroadcaster.BroadcastNpc(staticDatum);
    }

    private void UpdateBonds(SocialyteProfileStaticDatum staticDatum)
    {
        // TODO hardcode an exception for Simon
        BondHeartContainer.SetActive(staticDatum.IsPotentialTeammate);

        if (staticDatum.IsPotentialTeammate)
        {
            int bonds = (int)PersistentGameData.Bonds.Get(staticDatum.ProfileId);

            for (int i = 1; i <= 3; i++)
            {
                BondHearts[i - 1].SetActive(bonds >= i);
            }
        }
    }

    private void UpdateFromStringArray(string[] stringArray, int index, 
        TextMeshProUGUI text, int dataId)
    {
        if (stringArray == null || stringArray.Length == 0)
        {
            Log($"String array empty for data object with id {dataId}", LogLevel.Warning);

            text.text = "Unknown";
        }
        else if (stringArray.Length - 1 < index)
        {
            Log($"Index {index} is out of range for data object with id {dataId}", LogLevel.Warning);

            text.text = stringArray.Last();
        }
        else
        {
            text.text = stringArray[index];
        }
    }

    private async void UpdateSelected(NpcConnectionDatum datum)
    {
        Log($"Updating selected item details (ID:{datum.NpcId})", LogLevel.Verbose);

        var data = await datum.GetStaticDataAsync();
        SelectedConnectionNpcId = datum.NpcId;

        UpdateDetails(datum, data);

        AppSocialyteListItem selected = (AppSocialyteListItem)Items
            .First(x => x.IsSelected);

        ScrollList.UpdateItemList(selected, Items);
    }

    private void RestoreSelection()
    {
        if (Items.Count < 1)
        {
            ScrollList.UpdateItemList(null, Items);
            return;
        }

        if (SelectedConnectionNpcId == -1)
        {
            Items.First().Select();
            Log($"Selected first connection in list", LogLevel.Verbose);
        }
        else
        {
            Items
                .First(x => x.Datum.NpcId == SelectedConnectionNpcId)
                .Select();

            Log($"Selected last selected quest", LogLevel.Verbose);
        }
    }

    private async Task<IEnumerable<NpcConnectionDatum>> GetData()
    {
        // Await these to avoid null reference exceptions
        var gameData = await PersistentGameData.GetInstanceAsync();
        var staticGameDataInstance = await StaticGameData.GetInstanceAsync();

        var all = PersistentGameData.Socialyte.GetAll();
        return all.OrderBy(x => x.NpcId);
    }

    private async void RefreshItems()
    {
        if (!gameObject.activeSelf) return;
        await WaitForQuestRefreshToComplete();
        RefreshInProgress = true;

        try
        {
            Log($"Refreshing items: {CurrentTab}", LogLevel.Verbose);
            ScrollList.ClearItems();
            if (CurrentTab == SocialyteTab.Feed)
            {
                ScrollList.UpArrow.SetActive(false);
                ScrollList.DownArrow.SetActive(false);
                return;
            }
            var all = await GetData();

            var filtered = all
                .Where(x => x.IsVisible);

            await ScrollList.InstantiateItems(filtered);
            RestoreSelection();
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
