using System.Collections.Generic;
using UnityEngine;

namespace ComBots.Battles.BattleUI
{
    public class BotTeamPanel : MonoBehaviour
    {
        public List<BotPanel> botPanels;
        public Dictionary<BotSo, BotPanel> botPanelDict = new Dictionary<BotSo, BotPanel>();
        public void AssignBots(List<BotSo> bots)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                botPanels[i].AssignBot(bots[i]);
                botPanelDict.Add(bots[i], botPanels[i]);
            }
        }
        
        [ContextMenu("Update Team Panels")]
        public void UpdateTeamUI()
        {
            botPanels.ForEach(p => p.UpdateBotPanel());
        }
    }
}
