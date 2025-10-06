using System.Collections.Generic;
using UnityEngine;

public partial class PersistentGameData : MonoBehaviourR3
{
    public static PersistentGameData Instance { get; private set; }

    /// <summary>
    /// No letter I
    /// </summary>
    const string _playerStudentIdLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";

    [field: SerializeField, ComBotsSave("PlayerName", "Player")]
    public string PlayerName { get; set; } = "Player";

    /// <summary>
    /// The player’s Student ID number is generated automatically upon 
    /// starting a new game. The ID number is alphanumeric in the following 
    /// format: A00-A00-A00-A00, where A is a letter (except I) and 0 is a 
    /// number from 1 to 9.
    /// </summary>
    [field: SerializeField, ComBotsSave("PlayerStudentId", "")]
    public string PlayerStudentId { get; set; } = "";

    [RuntimeInitializeOnLoadMethod]
    private static void OnGameStart()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(
                new GameObject("PersistentGameData", typeof(PersistentGameData)));
        }
    }

    // Todo: needs hooked into new game start event
    public void GenerateStudentId()
    {
        var list = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            
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
            ComBotsSaveSystem.OnWillSave(SaveData)
        );

        LoadSavedData();
    }

    private void LoadSavedData() => 
        ComBotsSaveSystem.LoadData(typeof(PersistentGameData), this);

    private void SaveData() => 
        ComBotsSaveSystem.SaveData(typeof(PersistentGameData), this);
}