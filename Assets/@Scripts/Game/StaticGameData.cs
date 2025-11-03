using System.Threading.Tasks;
using UnityEngine;

public class StaticGameData : MonoBehaviourR3
{
    public static StaticGameData Instance { get; private set; }

    [field: SerializeField]
    public StaticQuestData[] QuestData { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void OnApplicationStart()
    {
        if (Instance == null)
        {
            var obj = Resources.Load<GameObject>("StaticGameData");
            Instantiate(obj);
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        if (Instance != null)
        {
            Debug.LogError("Too many StaticGameData instance detected in scene");
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static async Task<StaticGameData> GetInstanceAsync()
    {
        while (Instance != null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        return Instance;
    }
}