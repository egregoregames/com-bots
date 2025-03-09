using System;
using System.Collections.Generic;
using System.Linq;
using Game.Battles.Effects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Game.Battles
{
    [CreateAssetMenu(menuName = "Software/Standard Software", fileName = "Standard Software")]
    public class BotAttack : SerializedScriptableObject
    {
        
        [Header("Documentation")]
        [TextArea(2, 5)]
        public string description = "Enter a description for this character.";
        
        public List<SoftwareEffect> effects;
        [FormerlySerializedAs("damageEffect")] public SimpleDamage damage;
        public virtual List<string> ApplyAttack(BotSo attackingBot, List<BotSo> targets)
        {
            
            List<string> battleText = new List<string>();

            List<string> targetNames = targets.Select(t => t.name).ToList();

            string targetString = "";
            
            targetNames.ForEach(s => targetString += s + ", ");
            
            battleText.Add($"{attackingBot.name} uses {name} on {targetString}");

            attackingBot.Vitals[attackingBot.Vitals.FirstOrDefault(v => v.Key.name == "Energy").Key].Current -= EnergyCost;
            
            foreach (var target in targets)
            {
                foreach (var effect in effects)
                {
                    battleText.Add(effect.ApplyEffect(attackingBot, target));
                }
                    
                if (damage)
                {
                    battleText.Add(damage.ApplyDamage(attackingBot, target, Power));
                }
            }
            return battleText;
        }
    
        [field: SerializeField] public int Power { get; set; }
        [field: SerializeField] public float Accuracy { get; set; }

        [field: SerializeField] public float CriticalChance { get; set; } = .05f;
        [field: SerializeField] public int EnergyCost { get; set; }
        [field: SerializeField] public Priority Priority { get; set; }
        [field: SerializeField] public TargetType TargetType { get; set; }
    
        [field: SerializeField] public AuraType Aura { get; set; }
        public float RamCost { get; set; }
    
        public string EffectDescription { get; set; }
        public string MethodOfObtaining { get; set; }


        #region Validation

        private void OnValidate()
        {
            if (effects != null)
            {
                if (effects.Any(e => e == null))
                {
                    Debug.LogError($"{name} Effect cannot be null", this);
                }
            }
        }

        #endregion
    }
}



