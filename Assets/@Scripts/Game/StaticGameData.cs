using R3;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Self-instantiating singleton that acts as a data library for static, 
/// non-changing in-game data, such as quests, bot data, etc. For data that changes
/// and is saved/loaded, see <see cref="PersistentGameData"/>
/// </summary>
public class StaticGameData : MonoBehaviourR3
{
    public static StaticGameData Instance { get; private set; }

    [field: SerializeField]
    public StaticQuestDatum[] QuestData { get; private set; }

    [field: SerializeField]
    public StaticItemDatum[] ItemData { get; private set; }

    [field: SerializeField]
    public SocialyteProfileStaticDatum[] SocialyteData { get; private set; }

    [field: SerializeField]
    public StaticSolexDatum[] SolexData { get; private set; }

    /// <summary>
    /// Singletons that will be instantiated on app launch and 
    /// made <see cref="Object.DontDestroyOnLoad(Object)"/>
    /// </summary>
    [field: SerializeField]
    private GameObject[] Singletons { get; set; }

    [RuntimeInitializeOnLoadMethod]
    private static void OnApplicationStart()
    {
        if (Instance == null)
        {
            var obj = Resources.Load<GameObject>("StaticGameData");
            DontDestroyOnLoad(Instantiate(obj));
        }
    }

    public static int GetMaxInventoryItemQuantity(int itemId)
    {
        return Instance.ItemData.First(x => x.ItemId == itemId).MaxQuantity;
    }

    private new void Awake()
    {
        base.Awake();

        if (Instance != this)
        {
            throw new System.Exception("StaticGameData is not a singleton");
        }

        foreach (var item in Singletons)
        {
            DontDestroyOnLoad(Instantiate(item));
        }

        // Quest validation
        List<int> ids = new();
        foreach (var item in QuestData)
        {
            if (ids.Contains(item.QuestID))
            {
                throw new System.Exception($"Quest ID collision detected: {item.QuestID} - {item.QuestName}");
            }

            ids.Add(item.QuestID);
        }

        // Item validation 
        ids.Clear();
        foreach (var item in ItemData)
        {
            if (ids.Contains(item.ItemId))
            {
                throw new System.Exception($"Item ID collision detected: {item.ItemId} - {item.ItemName}");
            }

            if (item.MaxQuantity == 0)
            {
                Debug.LogWarning($"Max quantity has not been set for item {item.ItemId} - {item.ItemName}");
            }

            ids.Add(item.ItemId);
        }

        // Socialyte validation
        ids.Clear();
        foreach (var item in SocialyteData)
        {
            if (ids.Contains(item.ProfileId))
            {
                throw new System.Exception($"Socialyte profile ID collision detected: {item.ProfileId} - {item.ProfileName}");
            }

            ids.Add(item.ProfileId);
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        if (Instance != null)
        {
            throw new System.Exception(
                "Too many StaticGameData instance detected in scene");
        }

        Instance = this;
    }

    public static async Task<StaticGameData> GetInstanceAsync()
    {
        while (Instance == null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        return Instance;
    }
}