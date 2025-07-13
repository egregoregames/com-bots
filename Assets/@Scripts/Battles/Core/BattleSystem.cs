using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battles.BattleUI;
using Game.Battles.Stats;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Battles
{
    public class BattleSystem : MonoBehaviour
    {
        
        
        public List<AttackData> attacks = new List<AttackData>();

        public InputSO so;
        //[SerializeField] private TextMeshProUGUI battleDescription;
    
        [SerializeField] private BotTeamPanel allyTeam;
        [SerializeField] private BotTeamPanel enemyTeam;

        public List<BotSo> allyBots;
        public List<BotSo> enemyBots;

        [FormerlySerializedAs("firstSelected")] public GameObject firstSoftwareHUDButton;

        public SoftwareHUDButtons softwareHUDButtons;

        private BotTargettingUI _botTargettingUI;

        
        
        void Start()
        {
            _botTargettingUI = GetComponent<BotTargettingUI>();
            
            // assign bots to UI
            allyTeam.AssignBots(allyBots);
            enemyTeam.AssignBots(enemyBots);
            // restore bots
            allyBots.ForEach(b => b.RestoreStats());
            enemyBots.ForEach(b => b.RestoreStats());
            //update ui
            allyTeam.UpdateTeamUI();
            enemyTeam.UpdateTeamUI();
            
            
            
            TestAsync();
        }

        [ContextMenu("Switch")]
        public void Switch()
        {
            so.switchToUIInput.Invoke();
            EventSystem.current.SetSelectedGameObject(firstSoftwareHUDButton);

        }
        

        private Software softwareSelected;
        private bool hasBotSelectedSoftware = false;
        
        async void TestAsync()
        {
            foreach (var ally in allyBots)
            {
                Debug.Log($"{ally.name} is up!");
                hasBotSelectedSoftware = false;
                

                softwareSelected = await softwareHUDButtons.AssignSoftwareToButtons(ally);
                

                Debug.Log($"{ally.name} selected {softwareSelected.name}!");

                var targets = await _botTargettingUI.StartTargeting(ally, softwareSelected);

                string targetString = "";
                targets.ForEach(t => targetString += t._bot.name + " ");
                
                Debug.Log($"{ally.name} has targeted {targetString}");
    
            }
            //Debug.Log("");
        }
        // IEnumerator TestCoro()
        // {
        //     foreach (var ally in allyBots)
        //     {
        //         Debug.Log($"{ally.name} is up!");
        //         hasBotSelectedSoftware = false;
        //         
        //         softwareHUDButtons.AssignSoftwareToButtons(ally, SoftwareSelected);
        //         
        //         yield return new WaitUntil(() => hasBotSelectedSoftware);
        //
        //         Debug.Log($"{ally.name} selected {softwareSelected.name}!");
        //
        //         var validTargets = GetValidTargets(ally, softwareSelected, allyBots, enemyBots);
        //
        //         string validTargetsString = "";
        //         validTargets.ForEach(t => validTargetsString += t.name + " ");
        //         
        //         
        //         Debug.Log($"Valid targets for {softwareSelected.name} are: {validTargetsString}!");
        //
        //         //var targetsSelected = _botTargettingUI.StartTargeting(ally, softwareSelected);
        //
        //         yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
        //     }
        //     //Debug.Log("");
        // }

        
        
        IEnumerator BattleCor()
        {
            for (int i = 0; i < 10; i++)
            {
                // group attacks by priority
                var attackPriorityLists = GroupAndSortAttacksByPriority(attacks);

                yield return HandleEachPriorityAttackGroup(attackPriorityLists);

                yield return HandleEndOfTurnEffects(allyBots);
            
                yield return HandleEndOfTurnEffects(enemyBots);
            }
            

            
            yield return null;
        }

        IEnumerator HandleEachPriorityAttackGroup(Dictionary<int, List<AttackData>> attackPriorityLists)
        {
            foreach (var priorityListPair in attackPriorityLists)
            {
                Priority p = (Priority)priorityListPair.Key;
                
                //yield return LogBattleText($"Starting with attacks in priority group: {p.ToString()}", 2);

                var attacksInPriorityGroup = priorityListPair.Value;
                
                var namesOfAttackInThisGroup = attacksInPriorityGroup.Select(ad => ad.Attack.name).ToList();
                
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
                yield return new WaitForEndOfFrame();

                var attacksOrderedByBotSpeed = attacksInPriorityGroup.OrderByDescending(a => a.AttackingBot.Stats[GlobalDefinitions.Instance.Speed].Current).ToList();

                yield return HandleBotAttacks(attacksOrderedByBotSpeed);
                
            }
        }

        IEnumerator HandleBotAttacks(List<AttackData> attacksOrderedByBotSpeed)
        {
            foreach (var attack in attacksOrderedByBotSpeed)
            {
                var attackingBot = attack.AttackingBot;
                    
                
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
                yield return new WaitForEndOfFrame();
                
                bool canBotBattle = attack.AttackingBot.CanBotBattle(out var cannotBattleMessage);

                if (!canBotBattle)
                {
                    
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
                    yield return new WaitForEndOfFrame();       
                    
                    continue;
                }
                
                
                attack.Attack.ApplyAttack(attackingBot, attack.Targets);
                
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
                yield return new WaitForEndOfFrame();
                   
                
            }
        }
        
        IEnumerator HandleEndOfTurnEffects(List<BotSo> bots)
        {
            
            foreach (var bot in bots)
            {
                bot.OnEndOfTurn();
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
            yield return new WaitForEndOfFrame();

            yield return null;
        }

        private IEnumerator LogBattleText(string t, int waitDuration)
        {
            //battleDescription.text = t;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
            yield return new WaitForEndOfFrame();

        }
        

        Software ChooseAttack(BotSo botSo)
        {
            return botSo.Attacks[0];

            return botSo.Attacks[Random.Range(0, botSo.Attacks.Count - 1)];
        }

        List<BotSo> GetValidTargets(BotSo bot, Software attack, List<BotSo> allys, List<BotSo> enemies)
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

    

        bool AttackRequiresTarget(Software attack)
        {
            return attack.TargetType == TargetType.NoTarget;
        }
    
    
        public Dictionary<int, List<AttackData>> GroupAndSortAttacksByPriority(IEnumerable<AttackData> attacks)
        {
            return attacks
                .GroupBy(a => (int)a.Attack.Priority) // Group by priority
                .OrderBy(g => g.Key) // Higher priority first
                .ToDictionary(
                    g => g.Key, 
                    g => g.OrderByDescending(a => a.AttackingBot.Stats[GlobalDefinitions.Instance.Speed].Current).ToList() // Sort each group by speed
                );
        }



    
    
    
    }

    [Serializable]
    public class AttackData
    {
        public AttackData(BotSo attackingBot, List<BotSo> targets, Software attack, BotTeamPanel botTeam)
        {
            AttackingBot = attackingBot;
            Targets = targets;
            Attack = attack;
            BotTeam = botTeam;
        }

        [field: SerializeField] public BotTeamPanel BotTeam { get; set; }
        [field: SerializeField] public BotSo AttackingBot { get; set; }
        [field: SerializeField] public List<BotSo> Targets { get; set; }
        [field: SerializeField] public Software Attack { get; set; }
    }
    
}