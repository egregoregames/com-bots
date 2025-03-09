using Game.Battles.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battles.Effects
{
    [CreateAssetMenu(menuName = "Effects/Modify Stat", fileName = "Modify Stat")]
    public class ModifyStatEffect : SoftwareEffect
    {
        public StatDefinition statDef;
        public int modifier;
        public override string ApplyEffect(BotSo attackingBot, BotSo targettedBot)
        {
            targettedBot.Stats[statDef].Stage += modifier;
            return GetBattleText(targettedBot, modifier);
        }

        private string GetBattleText(BotSo targettedBot, int modifier)
        {
            if (modifier < 0)
            {
                return $"{targettedBot.name}'s {statDef.name} was lowered by 1!";
            }

            if (this.modifier > 0)
            {
                return $"{targettedBot.name}'s {statDef.name} was raised by 1!";
            }
            Debug.LogError("Should never get here");
            return "";
        }
    }
}
