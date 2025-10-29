using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

public class PlannerPanel : MonoBehaviourR3
{
    public static PlannerPanel Instance { get; private set; }

    [field: SerializeField]
    private List<PlannerQuestItem> QuestItems { get; set; }

    [field: SerializeField]
    private GameObject QuestItemTemplate { get; set; }

    private new void Awake()
    {
        base.Awake();
        QuestItems.Clear();
    }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = this;

        AddEvents(
            ComBotsSaveSystem.OnLoadSuccess(RefreshQuestItems));
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private async void RefreshQuestItems()
    {
        QuestItems.ForEach(x => Destroy(x));
        QuestItems.Clear();

        var gameData = await PersistentGameData.GetInstanceAsync();

        foreach (var item in gameData.PlayerQuestTrackingData)
        {
            
        }
    }
}