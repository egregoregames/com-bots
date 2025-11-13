using UnityEngine;

[CreateAssetMenu(fileName = "StaticQuestData", menuName = "Scriptable Objects/Static Quest Data")]
public class StaticQuestData : ScriptableObject
{
    [field: SerializeField]
    public int QuestID { get; set; }

    [field: SerializeField]
    public string QuestName { get; private set; }

    [field: SerializeField, TextArea]
    public string[] Steps { get; private set; }

    [field: SerializeField]
    public QuestType QuestType { get; private set; }

    [field: SerializeField]
    public int RewardCredits { get; private set; }
}