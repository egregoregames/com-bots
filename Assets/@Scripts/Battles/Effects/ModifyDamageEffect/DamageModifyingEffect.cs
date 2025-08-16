using UnityEngine;
using UnityEngine.Serialization;

namespace ComBots.Battles.Effects
{
    [CreateAssetMenu(menuName = "Effects/Damage Modifying Effect", fileName = "Damage Modifying Effect")]
    public class DamageModifyingEffect : ScriptableObject
    {
        public string battleText;
        public SoftwareBaseStat AttackStatToModify;
        public float amountToModify;
        public string ApplyAttackModifier(Software attack)
        {
            attack.tempBaseStatModifiers[AttackStatToModify] += amountToModify;
            return battleText;
        }
    }

    
} 