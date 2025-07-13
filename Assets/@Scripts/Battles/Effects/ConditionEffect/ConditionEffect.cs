using UnityEngine;

namespace Game.Battles.Effects
{
    [CreateAssetMenu(menuName = "Effects/Condition Effect", fileName = "Condition Effect")]
    public class ConditionEffect : SoftwareEffect
    {
        public Condition Condition;
        public ConditionEffectVerb ConditionEffectVerb;

        [Range(0, 1)]
        public float chance;
        public override void ApplyEffect(BotSo attackingBot, BotSo targettedBot, string applierName, Software attack)
        {
            if (ConditionEffectVerb == ConditionEffectVerb.Give)
            {
                bool doesNotHaveConditionAlready = !targettedBot.Conditions.Contains(Condition);
                
                if (doesNotHaveConditionAlready)
                {
                    if (DiceRoll.Roll(chance))
                    {
                        targettedBot.GiveCondition(Condition);
                        Debug.Log($"{targettedBot.name} was given: {Condition.name}!");
                        return;
                    }
                    
                    Debug.Log( $"{targettedBot.name} was NOT given: {Condition.name}!");
                    return;
                }
                Debug.Log( $"{targettedBot.name} already has: {Condition.name}!");
                return;

            }
            if (ConditionEffectVerb == ConditionEffectVerb.Cure)
            {
                if (targettedBot.Conditions.Contains(Condition))
                {
                    targettedBot.Conditions.Remove(Condition);
                    Debug.Log( $"{targettedBot.name} was cured of: {Condition.name}!");
                    return;

                }
                else
                {
                    Debug.Log( $"{targettedBot.name} does not have {Condition.name}," +
                               $"so it could not be cured of it!");
                    return;

                }
            }

            Debug.LogError("Should never get here");
            
        }
    }

    public enum ConditionEffectVerb
    {
        Give,
        Cure
    }
}