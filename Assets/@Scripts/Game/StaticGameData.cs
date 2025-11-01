using System.Threading.Tasks;
using UnityEngine;

public class StaticGameData : MonoBehaviourR3
{
    private static StaticGameData _instance;

    [field: SerializeField]
    public StaticQuestData[] QuestData { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void OnApplicationStart()
    {
        if (_instance == null)
        {
            var obj = Resources.Load<GameObject>("StaticGameData");
            Instantiate(obj);
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        if (_instance != null)
        {
            Debug.LogError("Too many StaticGameData instance detected in scene");
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static async Task<StaticGameData> GetInstanceAsync()
    {
        while (_instance != null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        return _instance;
    }
}