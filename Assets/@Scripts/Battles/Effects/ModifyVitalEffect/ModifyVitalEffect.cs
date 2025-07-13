using Game.Battles.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battles.Effects
{
    [CreateAssetMenu(menuName = "Effects/Modify Vital", fileName = "Modify Vital")]
    public class ModifyVitalEffect : SoftwareEffect
    {
        [FormerlySerializedAs("statDef")] public VitalDefinition vitalDef;
        public float percentage;
        public override void ApplyEffect(BotSo attackingBot, BotSo targettedBot, string applierName, Software attack = null)
        {
            int amountToMod = Mathf.CeilToInt(percentage * targettedBot.Vitals[vitalDef].Max);
            
            targettedBot.Vitals[vitalDef].Current += amountToMod;
            
            Debug.Log(GetBattleText(targettedBot, applierName, amountToMod));
        }

        private string GetBattleText(BotSo targettedBot, string applierName, float modifier)
        {
            if (modifier < 0)
            {
                return $"{targettedBot.name}'s {vitalDef.name} was lowered by {modifier} by {applierName}!";
            }

            if (modifier > 0)
            {
                return $"{targettedBot.name}'s {vitalDef.name} was raised by {modifier} by {applierName}!";
            }
            Debug.LogError("Should never get here");
            return "";
        }
    }
}