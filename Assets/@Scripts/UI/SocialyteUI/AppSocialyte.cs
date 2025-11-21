using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private TextMeshProUGUI TextNumberOfConnections { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextPlayerStudentId { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextPlayerRank { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextPlayerExams { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextPlayerBlueprints { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextPlayerSolex { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextPlayerMedals { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextPlayerSoftware { get; set; }

    [field: SerializeField]
    private Image ImageSolex { get; set; }

    [field: SerializeField]
    private GameObject PlayerProfileArea { get; set; }

    [field: SerializeField]
    private GameObject ContainerNumberOfConnections { get; set; }

    [field: SerializeField]
    private GameObject ContainerOrigin { get; set; }

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

    #region Monobehaviour
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
    #endregion

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

    private int GetConnectionCount()
    {
        return PersistentGameData.Socialyte.GetAll()
            .Where(x => x.NpcId != 0 && x.IsVisible)
            .Count();
    }

    private async void UpdateDetails(NpcConnectionDatum datum, 
        SocialyteProfileStaticDatum staticDatum)
    {
        // TODO: LOCALIZATION

        TextOccupation.transform.parent.gameObject.SetActive(true);
        TextCheckInLocation.transform.parent.gameObject.SetActive(true);
        TextOccupation.text = staticDatum.Occupation;
        string checkedInText = "Checked in at ";

        bool isPlayerProfile = datum.NpcId == 0;
        ContainerNumberOfConnections.SetActive(isPlayerProfile);
        TextPlayerStudentId.gameObject.SetActive(isPlayerProfile);
        PlayerProfileArea.SetActive(isPlayerProfile);
        ContainerOrigin.SetActive(!isPlayerProfile);
        TextBio.gameObject.SetActive(!isPlayerProfile);

        if (datum.NpcId == 0)
        {
            var pgdInstance = await PersistentGameData.GetInstanceAsync();
            // Is player profile
            TextContactName.text = pgdInstance.PlayerName;

            int connectionCount = GetConnectionCount();
            string s = connectionCount == 1 ? "" : "s";
            TextNumberOfConnections.text = $"{connectionCount} Connection{s}";

            string playerLoc = pgdInstance.CurrentLocationName;
            TextCheckInLocation.text = checkedInText + playerLoc;

            TextPlayerStudentId.text = PersistentGameData.GetPlayerStudentId();
            BondHeartContainer.SetActive(false);

            int rank = await PersistentGameData.GetPlayerRank();
            TextPlayerRank.text = $"I'm a Rank {rank} Meister!";

            // Only appears after player has obtained quest id 1, The Academy Trial
            bool hasAcademyTrial = await PersistentGameData.Quests.Exists(1);
            TextPlayerExams.transform.parent.gameObject.SetActive(hasAcademyTrial);
            int exams = pgdInstance.PromotionBattleVictoryCount;
            s = exams == 1 ? "" : "s";
            TextPlayerExams.text = $"I've passed {exams} Battle Exam{s}!";

            int blueprints = pgdInstance.PlayerBlueprintData.Count();
            TextPlayerBlueprints.text = $"I've collected {blueprints}/76 Blueprints!";

            // Only appears after player has obtained quest id 1, The Academy Trial
            TextPlayerMedals.transform.parent.gameObject.SetActive(hasAcademyTrial);
            int medals = await PersistentGameData.Medals.GetCountAsync();
            s = medals == 1 ? "" : "s";
            TextPlayerMedals.text = $"I've earned {medals} Medal{s}!";

            int software = await PersistentGameData.Software.GetCountAsync();
            TextPlayerSoftware.text = $"I've collected {software}/250 Software!";

            // Only appears after player has obtained quest Yama's Research Assistant
            var solexId = await PersistentGameData.GetSolexIdAsync();
            bool showSolex = solexId > 0;
            TextPlayerSolex.transform.parent.gameObject.SetActive(showSolex);
            if (showSolex)
            {
                var data = await StaticGameData.GetSolexDatumAsync(solexId);
                ImageSolex.sprite = data.SpriteIconSocialyte;
                TextPlayerSolex.text = $"I wield the {data.Name}!";
            }
        }
        else
        {
            UpdateFromStringArray(staticDatum.Bios, datum.CurrentBioStep,
                TextBio, datum.NpcId);

            UpdateFromStringArray(staticDatum.CheckInLocations,
                datum.CurrentCheckInLocationStep, TextCheckInLocation, 
                datum.NpcId, checkedInText);

            UpdateBonds(staticDatum);

            TextOrigin.text = staticDatum.Origin;
            TextContactName.text = staticDatum.ProfileName;
        }
            
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
        TextMeshProUGUI text, int dataId, string prefix = "")
    {
        if (stringArray == null || stringArray.Length == 0)
        {
            Log($"String array empty for data object with id {dataId}", LogLevel.Warning);

            text.text = $"{prefix}Unknown";
        }
        else if (stringArray.Length - 1 < index)
        {
            Log($"Index {index} is out of range for data object with id {dataId}", LogLevel.Warning);

            text.text = prefix + stringArray.Last();
        }
        else
        {
            text.text = prefix + stringArray[index];
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

    /// <summary>
    /// Temporary measure until Feed feature is developed. 
    /// Clears or turns off all text objects on the right side
    /// </summary>
    private void ClearAllRightSideText()
    {
        TextBio.text = string.Empty;
        TextCheckInLocation.transform.parent.gameObject.SetActive(false);
        TextContactName.text = string.Empty;
        TextOccupation.transform.parent.gameObject.SetActive(false);
        TextPlayerStudentId.text = string.Empty;
        PlayerProfileArea.SetActive(false);
        ContainerOrigin.SetActive(false);
        ContainerNumberOfConnections.SetActive(false);
        BondHeartContainer.SetActive(false);
        AppSocialyteNpcBroadcaster.BroadcastNpc(null);
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
                ClearAllRightSideText();
                return;
            }
            var all = await GetData();

            var filtered = all
                .Where(x => x.IsVisible);

            // Add an extra one for "YOUR PROFILE"
            PersistentGameData.Socialyte.SetConnectionVisible(0, true);

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
