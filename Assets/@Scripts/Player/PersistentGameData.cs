using ComBots.Game.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Singleton that must exist as soon as the game starts. Stores frequently 
/// accessed global variables. When testing, an instance of this can exist in 
/// the scene to supply variables manually.
/// </summary>
public partial class PersistentGameData : MonoBehaviourR3
{
    /// <summary>
    /// Reference to the singleton
    /// </summary>
    public static PersistentGameData Instance { get; private set; }

    private static UnityEventR3 _onTermUpdated = new();

    private static UnityEventR3 _onSaveDataLoaded = new();

    private static UnityEventR3 _onRankXpUpdated = new();

    private static UnityEventR3 _onCreditsUpdated = new();

    private static UnityEventR3 _onMoneyUpdated = new();

    private static UnityEventR3 _onLocationUpdated = new();

    private static UnityEventR3<QuestTrackingDatum> _onQuestUpdated = new();

    public static class GameEvents
    {
        public static IDisposable OnTermUpdated(Action x)
            => _onTermUpdated.Subscribe(x);

        public static IDisposable OnSaveDataLoaded(Action x)
            => _onSaveDataLoaded.Subscribe(x);

        public static IDisposable OnRankXpUpdated(Action x)
            => _onRankXpUpdated.Subscribe(x);

        public static IDisposable OnCreditsUpdated(Action x)
            => _onCreditsUpdated.Subscribe(x);

        public static IDisposable OnMoneyUpdated(Action x)
            => _onMoneyUpdated.Subscribe(x);

        public static IDisposable OnLocationUpdated(Action x)
            => _onLocationUpdated.Subscribe(x);

        public static IDisposable OnQuestUpdated(Action<QuestTrackingDatum> x)
            => _onQuestUpdated.Subscribe(x);
    }

    /// <summary>
    /// Entered by the player at the start of a new game
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerName, "Player")]
    public string PlayerName { get; set; } = "Player";

    /// <summary>
    /// The player's Student ID number is generated automatically upon 
    /// starting a new game. The ID number is alphanumeric in the following 
    /// format: A00-A00-A00-A00, where A is a letter (except I) and 0 is a 
    /// number from 1 to 9.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerStudentId, "")]
    public string PlayerStudentId { get; set; } = "";

    /// <summary>
    /// Used to calculate the player's rank
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerRankExperience, 64)]
    public int PlayerRankExperience { get; private set; } = 64;

    /// <summary>
    /// Changes after player wins Promotion Battles
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.CurrentTerm, Term.FirstTerm)]
    public Term CurrentTerm { get; private set; }

    /// <summary>
    /// The player's current location shows on the homescreen of the pause 
    /// menu and on Your Profile in the Socialyte App
    /// 
    /// When the player loads their game, the location banner will pull 
    /// down and show the name of the player's current location, as it would 
    /// when the player passes through a door.
    /// 
    /// This field should be updated when the player starts the game and when 
    /// the player passes through a door to a new location.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.CurrentLocationName, "")]
    public string CurrentLocationName { get; private set; } = "";

    /// <summary>
    /// Ranges between 0 and 5. Each time the player wins a Promotion Battle, 
    /// increases by 1. Displays on Your Profile in the Socialyte App if the 
    /// player has received the "The Academy Trial" quest.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PromotionBattleVictoryCount, 0)]
    public int PromotionBattleVictoryCount { get; set; } = 0;

    /// <summary>
    /// The total credits the player has received from completing quests, 
    /// starting at 0 and increasing each time a quest is completed. It 
    /// displays on the homescreen of the pause menu from the start of the 
    /// game (with a '.0' after the value). A minimum credit value is required 
    /// in order to schedule a Promotion Battle.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerCredits, 0)]
    public int PlayerCredits { get; private set; } = 0;

    /// <summary>
    /// The total money the player has (called Cybers in-game). The value 
    /// starts at 0 (and jumps from 0 to 500 upon receiving the Bank Card Key 
    /// Item), and it increases or decreases when the player purchases from a 
    /// Shopkeeper, sells to a Shopkeeper, is assessed a fee for using Omnifix 
    /// (resurrection service after being defeated in a battle), or is 
    /// assessed a fee for using OmniRide (fast-travel service). It displays 
    /// on the homescreen of the pause menu.It also displays when the player 
    /// is interacting with a Shopkeeper or the Omnifix or OmniRide services.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerMoney, 0)]
    public int PlayerMoney { get; private set; } = 0;

    /// <summary>
    /// A currency that is only used to purchase Items at an Arena. It starts 
    /// at 0 and only displays when the player is interacting with a Shopkeeper 
    /// in the Arena. It increases when the player wins an Arena battle and 
    /// decreases when the player purchases from a Shopkeeper in the Arena.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerBattlePoints, 0)]
    public int PlayerBattlePoints { get; set; } = 0;

    /// <summary>
    /// A list of NPC unique IDs that are currently in the player's team. Max of 2
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerNpcTeamMembers, null)]
    public List<string> PlayerNpcTeamMembers { get; private set; } = new();

    /// <summary>
    /// A list of NPC unique IDs that the player has connected with on Socialyte
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerNpcConnections, null)]
    public List<string> PlayerNpcConnections { get; private set; } = new();

    /// <summary>
    /// Defines the player's inventory. Each entry contains an Item ID and a quantity.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerInventoryItemData, null)]
    public List<InventoryItemDatum> PlayerInventoryItemData { get; private set; } = 
        new();

    /// <summary>
    /// List of quests the player has accepted and their current status
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerQuestTrackingData, null)]
    public List<QuestTrackingDatum> PlayerQuestTrackingData { get; private set; } = 
        new();

    /// <summary>
    /// List of current or former NPC teammates and their <see cref="TeammateBond"/> with the 
    /// player. If an NPC is not in this list, consider their bond level to be 0.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerTeammateBonds, null)]
    public List<TeammateBondDatum> PlayerTeammateBonds { get; private set; } = new();

    /// <summary>
    /// List of blueprints the player has either seen or not seen and their 
    /// current status. If a blueprint is not in this list, consider it unseen.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerBlueprintData, null)]
    public List<PlayerBlueprintDatum> PlayerBlueprintData { get; private set; } = new();

    /// <summary>
    /// List of software the player owns. If a software is not in this list, 
    /// consider it unowned.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerOwnedSoftware, null)]
    public List<PlayerSoftwareOwnershipDatum> PlayerOwnedSoftware { get; private set; } = new();

    /// <summary>
    /// Will be no more than 3 entries. Index 0 is the player's bot. Index 1 and 2 are teammates' bots.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerTeamBotStatusData, null)]
    public List<BotStatusDatum> PlayerTeamBotStatusData { get; private set; } = new();

    /// <summary>
    /// When the player first obtains the Cybercast App, only 3 channels are 
    /// visible. 
    /// <para />
    /// Over the course of the game, 5 additional channels can be unlocked, 
    /// becoming visible in the app.
    /// <para />
    /// For some of these channels, additional episodes within the channel are 
    /// also unlockable (e.g., something like each term adds 1 new episode). 
    /// The exact breakdown of episodes has not been designed yet.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerUnlockedCybercastChannels, null)]
    public List<string> PlayerUnlockedCybercastChannelIds { get; private set; } = new();

    [RuntimeInitializeOnLoadMethod]
    private static void OnGameStart()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(
                new GameObject("PersistentGameData", typeof(PersistentGameData)));
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        AddEvents(
            ComBotsSaveSystem.OnLoadSuccess(LoadSavedData),
            ComBotsSaveSystem.OnWillSave(SaveData),
            SimplePortal.OnPortalTriggered(UpdateLocationName)
        );

        LoadSavedData();
        GenerateStudentIdIfNoneExists();

#if UNITY_EDITOR
        // Add some test quests
        UpdateQuest(1, true, 0);
        UpdateQuest(2, false, 0);
        UpdateQuest(3, false, 0);
        UpdateQuest(4, false, 0);
        UpdateQuest(5, false, 0);
#endif
    }

    private void Reset()
    {
        PlayerRankExperience = 64;
        PlayerCredits = 0;
        PlayerBattlePoints = 0;
        PromotionBattleVictoryCount = 0;
        PlayerQuestTrackingData = new();
        PlayerNpcTeamMembers = new();
        PlayerTeammateBonds = new();
        PlayerBlueprintData = new();
        PlayerOwnedSoftware = new();
        PlayerTeamBotStatusData = new();
        PlayerUnlockedCybercastChannelIds = new();
    }

    /// <returns>
    /// An integer that represents the player's rank. A new player 
    /// in the game will have a rank of 5
    /// </returns>
    public static async Task<int> GetPlayerRank()
    {
        var rankXp = (await GetInstanceAsync()).PlayerRankExperience;
        return (int)Math.Floor(Math.Cbrt(rankXp)) + 1;
    }

    /// <summary>
    /// Used for the rank xp bar UI element (<see cref="UnityEngine.UI.Slider"/>)
    /// </summary>
    /// <returns>A percentage value between 0 and 1, inclusive</returns>
    public static async Task<float> GetProgressToNextRank()
    {
        using var block = InputBlocker.GetBlock("Getting progress to next rank");
        int rank = await GetPlayerRank();
        var rankXp = (await GetInstanceAsync()).PlayerRankExperience;
        int rankMinXp = (int)Math.Pow(rank - 1, 3);
        int rankMaxXp = (int)Math.Pow(rank, 3);
        int range = rankMaxXp - rankMinXp;
        int progress = rankXp - rankMinXp;
        return progress / range;
    }

    /// <summary>
    /// Sets the in-game <see cref="Term"/> and invokes 
    /// <see cref="GameEvents.OnTermUpdated(Action)"/>
    /// </summary>
    public static async void SetTerm(Term term)
    {
        using var block = InputBlocker.GetBlock("Setting term");
        (await GetInstanceAsync()).CurrentTerm = term;
        _onTermUpdated.Invoke();
    }

    /// <summary>
    /// Await this to get the instanced singleton of <see cref="PersistentGameData"/>
    /// </summary>
    /// <returns>The instanced singleton of this class</returns>
    public static async Task<PersistentGameData> GetInstanceAsync()
    {
        while (Instance == null)
        {
            await Task.Yield();
        }
        return Instance;
    }

    /// <summary>
    /// Should be called even if it's the first time the user is encountering a quest
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="isActive"></param>
    /// <param name="currentStep"></param>
    public static async void UpdateQuest(int questId, bool isActive, int currentStep)
    {
        using var block = InputBlocker.GetBlock("Updating quests");

        var quest = (await GetInstanceAsync()).PlayerQuestTrackingData
            .FirstOrDefault(x => x.QuestId == questId);

        if (quest == null)
        {
            quest = new QuestTrackingDatum()
            {
                QuestId = questId
            };

            (await GetInstanceAsync()).PlayerQuestTrackingData.Add(quest);
        }

        quest.IsActive = isActive;
        quest.CurrentStep = currentStep;
        if (quest.IsCompleted)
        {
            quest.Complete();
        }

        _onQuestUpdated?.Invoke(quest);
    }

    /// <summary>
    /// Adds to <see cref="PlayerRankExperience"/> and invokes 
    /// <see cref="GameEvents.OnRankXpUpdated(Action)"/>
    /// </summary>
    /// <param name="amount"></param>
    public void AddPlayerRankExperience(int amount)
    {
        PlayerRankExperience += amount;
        _onRankXpUpdated.Invoke();
    }

    /// <summary>
    /// Adds to <see cref="PlayerCredits"/> and invokes 
    /// <see cref="GameEvents.OnCreditsUpdated(Action)"/>
    /// </summary>
    /// <param name="amount">Credits to add</param>
    public void AddPlayerCredits(int amount)
    {
        PlayerCredits += amount;
        _onCreditsUpdated?.Invoke();
    }

    /// <summary>
    /// Sets the location name property and fires the 
    /// <see cref="GameEvents.OnLocationUpdated(Action)"/> event
    /// </summary>
    /// <param name="locationName"></param>
    public void UpdateLocationName(string locationName)
    {
        CurrentLocationName = locationName;
        _onLocationUpdated?.Invoke();
    }

    /// <summary>
    /// Should check to ensure amount will not bring credit balance below 
    /// 0 before calling this. Invokes <see cref="GameEvents.OnCreditsUpdated(Action)"/>
    /// </summary>
    /// <param name="amount">Amount of credits to deduct</param>
    public void DeductPlayerCredits(int amount)
    {
        PlayerCredits -= amount;
        _onCreditsUpdated?.Invoke();
    }

    /// <summary>
    /// Adds money (cybers) and invokes <see cref="GameEvents.OnMoneyUpdated(Action)"/>
    /// </summary>
    /// <param name="amount">Amount of money (cybers) to add</param>
    public void AddPlayerMoney(int amount)
    {
        PlayerMoney += amount;
        _onMoneyUpdated?.Invoke();
    }

    /// <summary>
    /// Deducts money (cybers) and invokes <see cref="GameEvents.OnMoneyUpdated(Action)"/> 
    /// </summary>
    /// <param name="amount">The amount of money (cybers) to deduct</param>
    public void DeductPlayerMoney(int amount)
    {
        PlayerMoney -= amount;
        _onMoneyUpdated?.Invoke();
    }

    private void GenerateStudentIdIfNoneExists()
    {
        if (string.IsNullOrWhiteSpace(PlayerStudentId))
        {
            PlayerStudentId = StudentIdGenerator.Generate();
            ComBotsSaveSystem.SaveData(SaveKeys.PlayerStudentId, PlayerStudentId);
        }
    }

    private void LoadSavedData() 
    {
        Reset();
        ComBotsSaveSystem.LoadData(typeof(PersistentGameData), this);
        _onSaveDataLoaded?.Invoke();
    }
        

    private void SaveData() => 
        ComBotsSaveSystem.SaveData(typeof(PersistentGameData), this);
}