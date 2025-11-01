using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticQuestData", menuName = "Scriptable Objects/Static Quest Data")]
public class StaticQuestData : ScriptableObject
{
    [field: SerializeField]
    public string QuestName { get; private set; }

    [field: SerializeField]
    public string[] Steps { get; private set; }
}