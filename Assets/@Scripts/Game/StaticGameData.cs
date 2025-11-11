using R3;
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
    public StaticQuestData[] QuestData { get; private set; }

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