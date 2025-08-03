using System;
using System.Collections.Generic;
using ComBots.Managers;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ComBots.Battles.BattleUI
{
    public class BotPanel : SerializedMonoBehaviour
    {
        public BotSo _bot;

        public Dictionary<Condition, GameObject> conditionsPanel;
        [SerializeField] private TextMeshProUGUI botName;
        [SerializeField] private TextMeshProUGUI botRank;
        [SerializeField] private Slider energySlider;
        [SerializeField] private Slider enduranceSlider;
        [SerializeField] private PanelAuraSwitcher panelAuraSwitcher;

        public GameObject target;


        public void AssignBot(BotSo bot)
        {
            _bot = bot;
        }
        
        [ContextMenu("Update bot panel")]
        public void UpdateBotPanel()
        {
            botName.text = _bot.name;
            botRank.text = _bot.Rank;
            
            energySlider.maxValue = _bot.Vitals[GlobalDefinitions.Instance.EnergyVital].Max;
            energySlider.value = _bot.Vitals[GlobalDefinitions.Instance.EnergyVital].Current;

            enduranceSlider.maxValue = _bot.Vitals[GlobalDefinitions.Instance.EnduranceVital].Max;
            enduranceSlider.value = _bot.Vitals[GlobalDefinitions.Instance.EnduranceVital].Current;

            panelAuraSwitcher.SwitchAuraBackground(_bot.CurrentAura);
            
            UpdateConditionsPanel(_bot.Conditions);
        }

        void UpdateConditionsPanel(List<Condition> botsConditions)
        {
            foreach (var panelCondition in conditionsPanel)
            {
                panelCondition.Value.SetActive(botsConditions.Contains(panelCondition.Key));
            }
        }
    }
}
