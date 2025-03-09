using System.Collections.Generic;
using Game.Battles.Effects;
using UnityEngine;

namespace Game.Battles
{
    [CreateAssetMenu(menuName = "Software/Additional Target Software", fileName = "Additional Target Software")]
    public sealed class AdditionalTargetAttack : BotAttack
    {
        public List<SoftwareEffect> selfAppliedEffects;
    
        public void ApplyAttack(BotSo attackingBot, List<BotSo> targets)
        {
            base.ApplyAttack(attackingBot, targets);
            
            // run apply attack again on self
            base.ApplyAttack(attackingBot, new List<BotSo>() {attackingBot});

        }
    }
}
