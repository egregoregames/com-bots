using UnityEngine;

/// <summary>
/// Contains immutable data for in-game quests. Accessible via 
/// <see cref="StaticGameData.QuestData"/>
/// </summary>
[CreateAssetMenu(
    fileName = "StaticQuestDatum", 
    menuName = "Scriptable Objects/Static Quest Datum")]
public class StaticQuestDatum : ScriptableObject
{
    [field: SerializeField]
    public int QuestID { get; private set; }

    [field: SerializeField]
    public string QuestName { get; private set; }

    [field: SerializeField, TextArea]
    public string[] Steps { get; private set; }

    [field: SerializeField]
    public QuestType QuestType { get; private set; }

    [field: SerializeField]
    public int RewardCredits { get; private set; }
}