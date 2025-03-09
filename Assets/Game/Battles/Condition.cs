using Game.Battles;
using Game.Battles.Stats;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Condition")]
public class Condition : ScriptableObject
{

   [Header("Documentation")] [TextArea(2, 5)]
   public string documentation;
   
   
   public string BattleMessage;
   
   public Result Result;
   public TestType Type;
   public Dissolves Dissolves;
   public DissolutionTime DissolutionTime;
   [FormerlySerializedAs("Stat")] public StatDefinition statDefinition;
   public float percentage;
   
   
   
}
