using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            ComBotsSaveSystem.OnLoadSuccess(RefreshQuestItems),

            // Bad for performance but we can worry about that after the MVP
            PersistentGameData.GameEvents.OnQuestUpdated(_ => RefreshQuestItems()));
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
            var newObj = Instantiate(
                QuestItemTemplate, QuestItemTemplate.transform.parent);

            var comp = newObj.GetComponent<PlannerQuestItem>();
            comp.SetQuest(item);
            QuestItems.Add(comp);
        }
    }
}