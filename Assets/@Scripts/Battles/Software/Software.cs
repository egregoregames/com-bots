using System;
using System.Collections.Generic;
using System.Linq;
using Game.Battles.Effects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Game.Battles
{
    [CreateAssetMenu(menuName = "Software/Standard Software", fileName = "Standard Software")]
    public class Software : SerializedScriptableObject
    {
        public Dictionary<SoftwareBaseStat, float> baseStats = new Dictionary<SoftwareBaseStat, float>();
        //[HideInInspector]
        public Dictionary<SoftwareBaseStat, float> tempBaseStatModifiers = new Dictionary<SoftwareBaseStat, float>();

        public float Power => baseStats[SoftwareBaseStat.Power];


        private void OnEnable()
        {
            foreach (var baseStat in baseStats)
            {
                tempBaseStatModifiers.TryAdd(baseStat.Key, 0);
            }
        }

        [Header("Documentation")]
        [TextArea(2, 5)]
        public string description = "Enter a description.";
        
        public List<SoftwareEffect> preamageSelfAppliedEffects;
        [FormerlySerializedAs("effects")] public List<SoftwareEffect> targetAppliedEffects;
        public List<SoftwareEffect> postDamageSelfAppliedEffects;
        public List<DamageModifyingEffect> attackModifierEffects;

        //public Dictionary<float, float> attackStats;
        public SimpleDamage damage;

        [HideInInspector]
        public List<SimpleDamage> internalDamageList = new List<SimpleDamage>();
        
        [HideInInspector]
        public int strikeCount = 1;

        float _totalDamageApplied;

        private List<string> _battleText;
        public void ApplyAttack(BotSo attackingBot, List<BotSo> targets)
        {

            foreach (var attackModifierEffect in attackModifierEffects)
            {
                attackModifierEffect.ApplyAttackModifier(this);
            }

            if (damage)
            {
                internalDamageList.Add(damage);
            }
            
            PayEnergyCostForAttack(attackingBot);
            
            UseSoftwareOnTargets(attackingBot, targets);
            
            
            // rebound effects
            ApplyAnyPostDamageSelfAppliedEffectsToSelf(attackingBot);

            ResetTempAttackModifiers();
            
            internalDamageList.Clear();
        }

        private void ResetTempAttackModifiers()
        {
            foreach (var key in tempBaseStatModifiers.Keys.ToList()) 
            {
                tempBaseStatModifiers[key] = 0f;
            }
        }

        private void ApplyAnyPostDamageSelfAppliedEffectsToSelf(BotSo attackingBot)
        {
            foreach (var effect in postDamageSelfAppliedEffects)
            {
                effect.ApplyEffect(attackingBot, attackingBot, name, this);
            }
        }
        private void ApplyAnyPreDamageSelfAppliedEffectsToSelf(BotSo attackingBot)
        {
            foreach (var effect in postDamageSelfAppliedEffects)
            {
                effect.ApplyEffect(attackingBot, attackingBot, name, this);
            }
        }

        private void UseSoftwareOnTargets(BotSo attackingBot, List<BotSo> targets)
        {

            attackingBot.CurrentAura = SoftwareAura;
            
            foreach (var target in targets)
            {
                Debug.Log($"{attackingBot.name} is attacking {target.name}");

                ApplyHitDamages(attackingBot, target);
                
                foreach (var effect in targetAppliedEffects)
                {
                    effect.ApplyEffect(attackingBot, target, name, this);
                }
                
                _totalDamageApplied = 0;
                
            }

        }

        public float GetCalculatedSoftwareDamage(BotSo attackingBot, BotSo targettedBot, Software software)
        {
            
            // var hasAdvantage = BotSo.CalculateAdvantagePower(software.SoftwareAura, targettedBot.CurrentAura);
            //
            // if(hasAdvantage)
            //     Debug.Log($"{attackingBot}'s aura boosted its attack power!");
            
            //todo
            // calculate advantage power
            
            float basePower = baseStats[SoftwareBaseStat.Power];

            var roll = DiceRoll.Roll(baseStats[SoftwareBaseStat.Critical]);

            if (roll)
            {
                Debug.Log($"A critical hit!");
                //todo 
                // critical damage calculation
                return basePower * 1.5f;
            }

            return basePower;
        }

        

        private void ApplyHitDamages(BotSo attackingBot, BotSo target)
        {
            foreach (var dam in internalDamageList)
            {
                dam.ApplyDamage(attackingBot, target, this);
                _totalDamageApplied += baseStats[SoftwareBaseStat.Power];
            }
        }

        private void PayEnergyCostForAttack(BotSo attackingBot)
        {
            attackingBot.Vitals[attackingBot.Vitals.FirstOrDefault(v => v.Key.name == "Energy").Key].Current -= baseStats[SoftwareBaseStat.EnergyCost];
        }


        [field: SerializeField] public Priority Priority { get; set; }
        [field: SerializeField] public TargetType TargetType { get; set; }
    
        [field: SerializeField] public AuraType SoftwareAura { get; set; }
        public float RamCost { get; set; }
    
        public string EffectDescription { get; set; }
        public string MethodOfObtaining { get; set; }


        #region Validation

        private void OnValidate()
        {
            if (targetAppliedEffects != null)
            {
                if (targetAppliedEffects.Any(e => e == null))
                {
                    Debug.LogError($"{name} Effect cannot be null", this);
                }
            }
        }

        #endregion
    }

    public enum SoftwareBaseStat
    {
        Power,
        Accuracy,
        Critical,
        EnergyCost
        
    }
}



