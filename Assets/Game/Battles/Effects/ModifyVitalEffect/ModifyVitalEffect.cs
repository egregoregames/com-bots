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
        public override string ApplyEffect(BotSo attackingBot, BotSo targettedBot)
        {
            int amountToMod = Mathf.CeilToInt(percentage * targettedBot.Vitals[vitalDef].Max);
            
            targettedBot.Vitals[vitalDef].Current += amountToMod;
            
            return GetBattleText(targettedBot, amountToMod);
        }

        private string GetBattleText(BotSo targettedBot, float modifier)
        {
            if (modifier < 0)
            {
                return $"{targettedBot.name}'s {vitalDef.name} was lowered by {modifier}!";
            }

            if (this.percentage > 0)
            {
                return $"{targettedBot.name}'s {vitalDef.name} was raised by {modifier}!";
            }
            Debug.LogError("Should never get here");
            return "";
        }
    }
}