using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battles.Stats;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Battles
{
    public class BattleSystem : MonoBehaviour
    {
        public StatDefinition speedDefinition;
    
        public List<BotSo> alliedBots;

        public List<BotSo> enemyBots;

        public List<AttackData> attacks = new List<AttackData>();

        [SerializeField] private BattleSceneReferences _battleSceneReferences;

        [SerializeField] private TextMeshProUGUI battleDescription;
    
        [ContextMenu("Battle")]
        void Start()
        {
            _battleSceneReferences.Init(alliedBots, enemyBots);
            _battleSceneReferences.UpdateUi();
            StartCoroutine(BattleCor());
            RestoreBots();
        }

        private void RestoreBots()
        {
            alliedBots.ForEach(b => b.RestoreStats());
            enemyBots.ForEach(b => b.RestoreStats());
        }


        IEnumerator BattleCor()
        {
            //  1. Player chooses attack (Software) for bot
            attacks.Clear();
            foreach (var bot in alliedBots)
            {
                // bot chooses attack software
                var chosenAttack = ChooseAttack(bot);
            
                List<BotSo> targets = ChooseAttackTarget(bot, chosenAttack, alliedBots, enemyBots);
                attacks.Add(new AttackData(bot, targets, chosenAttack, new BotTeam(alliedBots, 0)));
            }
        
            foreach (var bot in enemyBots)
            {
                // bot chooses attack software
                var chosenAttack = ChooseAttack(bot);
            
                List<BotSo> targets = ChooseAttackTarget(bot, chosenAttack, enemyBots, alliedBots);
                attacks.Add(new AttackData(bot, targets, chosenAttack, new BotTeam(enemyBots, 0)));

            }
        
            // group attacks by priority
            var g = GroupAndSortAttacks(attacks);

            var basicPriorityAttacks = g[5];
        
            // sort these by speed

            foreach (var attack in basicPriorityAttacks)
            {
                var attackingBot = attack.AttackingBot;


                yield return LogBattleText($"Attacking bot is: {attackingBot.name}");
            
                var battleText = attack.Attack.ApplyAttack(attackingBot, attack.Targets);

                yield return LogBattleText(battleText);
                // if its online
                if (attackingBot.OnlineOffline)
                {
                    if (!attackingBot.StasisStatus)
                    {
                        // atttack or item
                        // if (!attackingBot.FreezeStatus)
                        // {
                        //     if (!attackingBot.ShockStatus)
                        //     {
                        //         
                        //     }
                        // }
                    }
                }
                
                _battleSceneReferences.UpdateUi();
            }
        
        

            bool stasis;
        
            //if(stasis)
        
        
            yield return null;
        }

        private IEnumerator LogBattleText(string t)
        {
            battleDescription.text = t;
            yield return new WaitForSeconds(3);
        }
        
        private IEnumerator LogBattleText(List<string> t)
        {
            foreach (var text in t)
            {
                battleDescription.text = text;
                yield return new WaitForSeconds(3);
            }
        }

        BotAttack ChooseAttack(BotSo botSo)
        {
            return botSo.Attacks[0];

            return botSo.Attacks[Random.Range(0, botSo.Attacks.Count - 1)];
        }

        List<BotSo> ChooseAttackTarget(BotSo bot, BotAttack attack, List<BotSo> allys, List<BotSo> enemies)
        {
            switch (attack.TargetType)
            {
                case TargetType.NoTarget:
                    return null;
                case TargetType.Self:
                    return new List<BotSo> { bot };
                case TargetType.OneNonSelfTeammate:
                    return new List<BotSo> { allys.GetRandomElement(bot) };
                case TargetType.OneTeammate:
                    return new List<BotSo> { allys.GetRandomElement() };
                case TargetType.AllNonSelfTeammates:
                    return  allys.Without(bot);
                case TargetType.OneOpponent:
                    return new List<BotSo> { enemies.GetRandomElement() };
                case TargetType.AllOpponents:
                    return enemies;
                case TargetType.OneNonSelfCombatant:
                    return  allys.Concat(enemies).ToList().Without(bot);
                case TargetType.OneCombatant:
                    return  new List<BotSo> { allys.Concat(enemies).ToList().GetRandomElement()};
                case TargetType.AllNonSelfCombatants:
                    return  allys.Concat(enemies).ToList().Without(bot);
                case TargetType.All:
                    return  allys.Concat(enemies).ToList();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

    

        bool AttackRequiresTarget(BotAttack attack)
        {
            return attack.TargetType == TargetType.NoTarget;
        }
    
    
        public Dictionary<int, List<AttackData>> GroupAndSortAttacks(IEnumerable<AttackData> attacks)
        {
            return attacks
                .GroupBy(a => (int)a.Attack.Priority) // Group by priority
                .OrderByDescending(g => g.Key) // Higher priority first
                .ToDictionary(
                    g => g.Key, 
                    g => g.OrderByDescending(a => a.AttackingBot.Stats[speedDefinition].Current).ToList() // Sort each group by speed
                );
        }



    
    
    
    }

    [Serializable]
    public class AttackData
    {
        public AttackData(BotSo attackingBot, List<BotSo> targets, BotAttack attack, BotTeam botTeam)
        {
            AttackingBot = attackingBot;
            Targets = targets;
            Attack = attack;
            BotTeam = botTeam;
        }

        [field: SerializeField] public BotTeam BotTeam { get; set; }
        [field: SerializeField] public BotSo AttackingBot { get; set; }
        [field: SerializeField] public List<BotSo> Targets { get; set; }
        [field: SerializeField] public BotAttack Attack { get; set; }
    }

    public class BotTeam
    {
        public BotTeam(List<BotSo> bots, float omegaEnergy)
        {
            Bots = bots;
            OmegaEnergy = omegaEnergy;
        }

        [field: SerializeField] public List<BotSo> Bots { get; set; }
        [field: SerializeField] public float OmegaEnergy { get; set; }

    }
}