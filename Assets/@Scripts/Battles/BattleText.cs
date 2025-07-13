using System.Collections.Generic;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Battles.BattleUI
{
    public class BattleText : Singleton<BattleText>
    {
        [SerializeField] private TextMeshProUGUI energyCost;
        [SerializeField] private TextMeshProUGUI softwareName;
        [SerializeField] private TextMeshProUGUI softwareDescription;

        [SerializeField] private Image auraIconImage;

        public Dictionary<AuraType, Sprite> auraIcons;

        public Software testSoftware;
        
        [ContextMenu("Update")]
        public void UpdateIt()
        {
            UpdateBattleText(testSoftware);
        }
        
        public void UpdateBattleText(Software software)
        {
            energyCost.text = software.baseStats[SoftwareBaseStat.EnergyCost].ToString();
            softwareName.text = software.name;
            softwareDescription.text = software.description;

            auraIconImage.sprite = auraIcons[software.SoftwareAura];
        }
    }
}
