using System;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Singleton that must exist as soon as the game starts. Stores frequently 
/// accessed global variables. When testing, an instance of this can exist in 
/// the scene to supply variables manually.
/// </summary>
public partial class PersistentGameData : MonoBehaviourR3
{
    public static PersistentGameData Instance { get; private set; }

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
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerRankExperience, 0)]
    public int PlayerRankExperience { get; private set; } = 0;

    /// <summary>
    /// Changes after player wins Promotion Battles
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.CurrentTerm, Term.FirstTerm)]
    public Term CurrentTerm { get; set; }

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
    public string CurrentLocationName { get; set; } = "";

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
    /// game (with a “.0” after the value). A minimum credit value is required 
    /// in order to schedule a Promotion Battle.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerCredits, 0)]
    public int PlayerCredits { get; set; } = 0;

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
    public int PlayerMoney { get; set; } = 0;

    /// <summary>
    /// A currency that is only used to purchase Items at an Arena. It starts 
    /// at 0 and only displays when the player is interacting with a Shopkeeper 
    /// in the Arena. It increases when the player wins an Arena battle and 
    /// decreases when the player purchases from a Shopkeeper in the Arena.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerBattlePoints, 0)]
    public int PlayerBattlePoints { get; set; } = 0;

    [field: SerializeField, ComBotsSave(SaveKeys.CurrentDateTime, 0)]
    private long DateTimeTicks { get; set; } = 0;

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

    [RuntimeInitializeOnLoadMethod]
    private static void OnGameStart()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(
                new GameObject("PersistentGameData", typeof(PersistentGameData)));
        }
    }

    /// <summary>
    /// Sets a specific time of day for the game.
    /// </summary>
    /// <param name="hour">0 to 23</param>
    /// <param name="minute">0 to 59</param>
    /// <param name="second">0 to 59</param>
    public void SetTimeOfDay(int hour, int minute, int second = 0)
    {
        var dt = GetCurrentDateTime().Date; // Midnight
        dt.AddHours(hour);
        dt.AddMinutes(minute);
        dt.AddSeconds(second);
        DateTimeTicks = dt.Ticks;
    }

    /// <summary>
    /// Sets the day of the week for the game
    /// </summary>
    /// <param name="dayOfWeek"></param>
    public void SetDayOfWeek(DayOfWeek dayOfWeek)
    {
        var dt = GetCurrentDateTime();
        int daysToAdd = ((int)dayOfWeek - (int)dt.DayOfWeek + 7) % 7;
        dt = dt.AddDays(daysToAdd);
        DateTimeTicks = dt.Ticks;
    }

    /// <summary>
    /// Retrieves the stored DateTime for the gmae
    /// </summary>
    public DateTime GetCurrentDateTime() => new(DateTimeTicks);

    public void AddPlayerRankExperience(int amount)
    {
        PlayerRankExperience += amount;
        ComBotsSaveSystem.SaveData(SaveKeys.PlayerRankExperience, PlayerRankExperience);
    }

    private void Reset()
    {
        PlayerRankExperience = 0;
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
            ComBotsSaveSystem.OnWillSave(SaveData)
        );

        LoadSavedData();
        GenerateStudentIdIfNoneExists();
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
    }
        

    private void SaveData() => 
        ComBotsSaveSystem.SaveData(typeof(PersistentGameData), this);
}