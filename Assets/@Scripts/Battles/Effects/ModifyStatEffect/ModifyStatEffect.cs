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
        public override void ApplyEffect(BotSo attackingBot, BotSo targettedBot, string applierName, Software attack = null)
        {
            targettedBot.Stats[statDef].Stage += modifier;
            Debug.Log($"{targettedBot}'s {statDef.name} stage was modified by the following {modifier}!");
            //todo
            // specific text info for different types of effects
        }
    }
}
