using System.Collections.Generic;
using System.Linq;
using Game.Battles.Stats;
using Game.Battles.Vitals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Battles
{
    [CreateAssetMenu(fileName = "Bot")]
    public class BotSo : SerializedScriptableObject
    {
        //Software load
        [field: SerializeField] public List<Software> Attacks { get; set; }
    
        public Dictionary<StatDefinition, Stat> Stats;
    
        public Dictionary<VitalDefinition, Vital> Vitals;

        public float CurrentEndurance
        {
            get => Vitals[Vitals.FirstOrDefault(v => v.Key.name == "Endurance").Key].Current;
            set => Vitals[Vitals.FirstOrDefault(v => v.Key.name == "Endurance").Key].Current = value;
        }
        public float CurrentEnergy
        {
            get => Vitals[Vitals.FirstOrDefault(v => v.Key.name == "Energy").Key].Current;
            set => Vitals[Vitals.FirstOrDefault(v => v.Key.name == "Energy").Key].Current = value;
        }

        public List<Condition> Conditions;

        public AuraType CurrentAura;

        public void RestoreStats()
        {
            Stats.Values.ToList().ForEach(s => s.Restore());
            Vitals.Values.ToList().ForEach(v => v.Restore());

        }

        public void GetHit(BotSo attackingBot, BotSo targettedBot, Software software)
        {
            var damageToInflict = software.GetCalculatedSoftwareDamage(attackingBot, targettedBot, software);
            CurrentEndurance -= damageToInflict;
            Debug.Log($"{attackingBot.name} hit {targettedBot.name} with {software.name}, {targettedBot.name} took {damageToInflict} damage!");
        }

        public void GiveCondition(Condition condition)
        {
            if(!Conditions.Contains(condition))
                Conditions.Add(condition);
            
            condition.OnGiveCondition();
        }

        public bool CanBotBattle(out string message)
        {
            foreach (var condition in Conditions)
            {
                if (condition.preventsBattle)
                {
                    return condition.CanBotBattle(out message);
                    break;
                }
            }

            message = "";
            return true;
        }
        public static float CalculateAdvantagePower(AuraType attackingAura, AuraType targetsAura)
        {
            return 0f;
        }
        
        

        public void OnEndOfTurn()
        {
            for (int i = Conditions.Count - 1; i >= 0; i--)
            {
                Conditions[i].OnEndOfTurn(this);
            }
        }


        private string RemoveCondition(Condition c)
        { 
            Conditions.Remove(c);
            return $"{name} was cured of {c.name}!";
        }


        [field: SerializeField] public string Rank { get; set; }
        [field: SerializeField] public string Meister { get; set; }
        [field: SerializeField] public bool OnlineOffline { get; set; }
        [field: SerializeField] public string CurrentBlueprint { get; set; }
        [field: SerializeField] public string BotForm { get; set; }
        [field: SerializeField] public string CurrentAbility { get; set; }
        [field: SerializeField] public string SoftwareLoad { get; set; }
        [field: SerializeField] public string HardwareLoad { get; set; }
        [field: SerializeField] public float TotalWeight { get; set; }
    
        [field: SerializeField] public int MeleeAttackModifierStage { get; set; } = 0;
        [field: SerializeField] public int BeamAttackModifierStage { get; set; } = 0;
        [field: SerializeField] public int BlastAttackModifierStage { get; set; } = 0;
        [field: SerializeField] public int MeleeDefenseModifierStage { get; set; } = 0;
        [field: SerializeField] public int BeamDefenseModifierStage { get; set; } = 0;
        [field: SerializeField] public int BlastDefenseModifierStage { get; set; } = 0;
        [field: SerializeField] public int SpeedModifierStage { get; set; } = 0;
        [field: SerializeField] public int AccuracyModifierStage { get; set; } = 0;
        [field: SerializeField] public int EvasivenessModifierStage { get; set; } = 0;
        [field: SerializeField] public int CriticalHitRateModifierStage { get; set; } = 0;
    
    
        [field: SerializeField] public bool StasisStatus { get; set; } = false;
        [field: SerializeField] public bool ZeroedInStatus { get; set; } = false;
        [field: SerializeField] public bool ScannedStatus { get; set; } = false;
        [field: SerializeField] public bool StoringPowerStatus { get; set; } = false;
    
        [field: SerializeField] public bool GuardStatus { get; set; } = false;
        [field: SerializeField] public bool SpinyGuardStatus { get; set; } = false;
        [field: SerializeField] public bool InterceptStatus { get; set; } = false;
        [field: SerializeField] public string InterceptGrantor { get; set; }
        [field: SerializeField] public bool FilterStatus { get; set; } = false;
        [field: SerializeField] public bool HideStatus { get; set; } = false;
        [field: SerializeField] public bool DeflectionFieldStatus { get; set; } = false;
        [field: SerializeField] public bool GuardedAttackStatus { get; set; } = false;
        [field: SerializeField] public bool RiposteStatus { get; set; } = false;
        [field: SerializeField] public bool CoveringFireStatus { get; set; } = false;
        [field: SerializeField] public string CoveringFireGrantor { get; set; }
        [field: SerializeField] public bool ImplantedStatus { get; set; } = false;
        [field: SerializeField] public bool EnduraBatteryStatus { get; set; } = false;
        [field: SerializeField] public int EnduraBatteryCounter { get; set; } = 0;
        [field: SerializeField] public string LastSoftwareUsed { get; set; }
        [field: SerializeField] public string LastSoftwareToHitBot { get; set; }
        [field: SerializeField] public int TotalDamageTakenLastTurn { get; set; } = 0;
        [field: SerializeField] public bool BlockedStatus { get; set; } = false;
        [field: SerializeField] public string LastItemUsedByTeam { get; set; }
        [field: SerializeField] public string OmegaEnergy { get; set; }
        [field: SerializeField] public string LastSoftwareUsedByAnyCombatant { get; set; }
        [field: SerializeField] public bool SmokescreenEffect { get; set; } = false;
        [field: SerializeField] public bool SafetyFoamEffect { get; set; } = false;
        [field: SerializeField] public bool TimeBombStatus { get; set; } = false;
        [field: SerializeField] public int CurrentTurn { get; set; } = 1;
    }
}
