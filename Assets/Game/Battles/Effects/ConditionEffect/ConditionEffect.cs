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
        public override string ApplyEffect(BotSo attackingBot, BotSo targettedBot)
        {
            if (ConditionEffectVerb == ConditionEffectVerb.Give)
            {
                if (!targettedBot.Conditions.Contains(Condition))
                {
                    targettedBot.Conditions.Add(Condition);
                    return $"{targettedBot.name} was given: {Condition.name}!";
                }
                
            }
            if (ConditionEffectVerb == ConditionEffectVerb.Cure)
            {
                if (targettedBot.Conditions.Contains(Condition))
                {
                    targettedBot.Conditions.Remove(Condition);
                    return $"{targettedBot.name} was cured of: {Condition.name}!";

                }
                else
                {
                    return $"{targettedBot.name} does not have {Condition.name}," +
                           $"so it could not be cured of it!";
                }
            }

            Debug.LogError("Should never get here");
            return "";
        }
    }

    public enum ConditionEffectVerb
    {
        Give,
        Cure
    }
}