using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ComBots.Battles.BattleUI
{
    public class BotTargettingUI : MonoBehaviour
    {
        [SerializeField] private BotTeamPanel allyTeamPanels;
        [SerializeField] private BotTeamPanel enemyTeamPanels;

        public async Task<List<BotPanel>> StartTargeting(BotSo bot, Software attack)
        {
            List<BotPanel> validBotPanels = new List<BotPanel>();
        
            validBotPanels = GetAllValidTargets(allyTeamPanels.botPanelDict[bot],
                attack,
                allyTeamPanels.botPanels,
                enemyTeamPanels.botPanels);

            return await GetTargets(validBotPanels);
        
            foreach (var panel in allyTeamPanels.botPanels)
            {
                panel.target.SetActive(false);
            }
            foreach (var panel in enemyTeamPanels.botPanels)
            {
                panel.target.SetActive(false);
            }
        
            foreach (var panel in validBotPanels)
            {
                panel.target.SetActive(true);
        
            }
        }

        public async Task<List<BotPanel>> GetTargets(List<BotPanel> potentialTargets)
        {
            if (targetingMode == UiTargeting.Mass)
            {
                // Mass targeting logic (not specified in your request)
            }
            else
            {
                int selectedIndex = 0;

                // Ensure at least one target exists
                if (potentialTargets.Count == 0)
                    return new List<BotPanel>();

                // Highlight initial selection
                UpdateTargetingUI(potentialTargets, selectedIndex);
                // 🔹 Wait for "A" key to be released first to prevent instant exit
                while (Input.GetKey(KeyCode.A)) 
                    await Task.Yield();
                
                while (true)
                {
                    await Task.Yield(); // Avoid freezing the main thread
                    Debug.Log("Reading input");
                    Debug.Log($"selected index: {selectedIndex} ({potentialTargets[selectedIndex]._bot.name})");

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        selectedIndex = (selectedIndex - 1 + potentialTargets.Count) % potentialTargets.Count;
                        UpdateTargetingUI(potentialTargets, selectedIndex);
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        selectedIndex = (selectedIndex + 1) % potentialTargets.Count;
                        UpdateTargetingUI(potentialTargets, selectedIndex);
                    }
                    else if (Input.GetKeyDown(KeyCode.A)) // Confirm selection
                    {
                        for (int i = 0; i < potentialTargets.Count; i++)
                        {
                            potentialTargets[i].target.SetActive(false);
                        }
                        break;
                    }
                }

                // Return the chosen target
                return new List<BotPanel> { potentialTargets[selectedIndex] };
            }

            return null;
        }

        void UpdateTargetingUI(List<BotPanel> targets, int selectedIndex)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].target.SetActive(i == selectedIndex);
            }
        }

    
        List<BotPanel> GetValidTargetsOld(BotPanel bot, Software attack, List<BotPanel> allys, List<BotPanel> enemies)
        {
            switch (attack.TargetType)
            {
                case TargetType.NoTarget:
                    return null;
                case TargetType.Self:
                    return new List<BotPanel> { bot };
                case TargetType.OneNonSelfTeammate:
                    return new List<BotPanel> { allys.GetRandomElement(bot) };
                case TargetType.OneTeammate:
                    return new List<BotPanel> { allys.GetRandomElement() };
                case TargetType.AllNonSelfTeammates:
                    return  allys.Without(bot);
                case TargetType.OneOpponent:
                    return new List<BotPanel> { enemies.GetRandomElement() };
                case TargetType.AllOpponents:
                    return enemies;
                case TargetType.OneNonSelfCombatant:
                    return  allys.Concat(enemies).ToList().Without(bot);
                case TargetType.OneCombatant:
                    return  new List<BotPanel> { allys.Concat(enemies).ToList().GetRandomElement()};
                case TargetType.AllNonSelfCombatants:
                    return  allys.Concat(enemies).ToList().Without(bot);
                case TargetType.All:
                    return  allys.Concat(enemies).ToList();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        private UiTargeting targetingMode;
        List<BotPanel> GetAllValidTargets(BotPanel bot, Software attack, List<BotPanel> allys, List<BotPanel> enemies)
        {
            switch (attack.TargetType)
            {
                case TargetType.NoTarget:
                    return null;
                case TargetType.Self:
                    targetingMode = UiTargeting.Single;
                    return new List<BotPanel> { bot };
                case TargetType.OneNonSelfTeammate:
                    targetingMode = UiTargeting.Single;
                    return allys.Without(bot);
                case TargetType.OneTeammate:
                    targetingMode = UiTargeting.Single;
                    return allys;
                case TargetType.AllNonSelfTeammates:
                    targetingMode = UiTargeting.Mass;
                    return  allys.Without(bot);
                case TargetType.OneOpponent:
                    targetingMode = UiTargeting.Single;
                    return enemies;
                case TargetType.AllOpponents:
                    targetingMode = UiTargeting.Mass;
                    return enemies;
                case TargetType.OneNonSelfCombatant:
                    targetingMode = UiTargeting.Single;
                    return  allys.Concat(enemies).ToList().Without(bot);
                case TargetType.OneCombatant:
                    targetingMode = UiTargeting.Single;
                    return  allys.Concat(enemies).ToList();
                case TargetType.AllNonSelfCombatants:
                    targetingMode = UiTargeting.Mass;
                    return  allys.Concat(enemies).ToList().Without(bot);
                case TargetType.All:
                    targetingMode = UiTargeting.Mass;
                    return  allys.Concat(enemies).ToList();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }
        IEnumerator SelectionCoro()
        {
            // allow cycling through panels
            while (Input.GetKeyDown(KeyCode.A))
            {
            
                yield return null;
            }
        }
    }

    public enum UiTargeting
    {
        Single,
        Mass
    }
}
