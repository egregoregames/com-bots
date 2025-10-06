using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that must exist as soon as the game starts. Stores frequently 
/// accessed global variables. When testing, an instance of this can exist in 
/// the scene to supply variables manually.
/// </summary>
public partial class PersistentGameData : MonoBehaviourR3
{
    public static PersistentGameData Instance { get; private set; }

    [field: SerializeField, ComBotsSave(SaveKeys.PlayerName, "Player")]
    public string PlayerName { get; set; } = "Player";

    /// <summary>
    /// The player’s Student ID number is generated automatically upon 
    /// starting a new game. The ID number is alphanumeric in the following 
    /// format: A00-A00-A00-A00, where A is a letter (except I) and 0 is a 
    /// number from 1 to 9.
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerStudentId, "")]
    public string PlayerStudentId { get; set; } = "";

    /// <summary>
    /// Used to calculate the player’s rank
    /// </summary>
    [field: SerializeField, ComBotsSave(SaveKeys.PlayerRankExperience, 0)]
    public int PlayerRankExperience { get; private set; } = 0;

    [RuntimeInitializeOnLoadMethod]
    private static void OnGameStart()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(
                new GameObject("PersistentGameData", typeof(PersistentGameData)));
        }
    }

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