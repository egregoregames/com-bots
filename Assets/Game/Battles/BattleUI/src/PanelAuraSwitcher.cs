using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Battles.BattleUI
{
    public class PanelAuraSwitcher : SerializedMonoBehaviour
    {
        public Dictionary<AuraType, Sprite> auraBackGroundDict;
        [SerializeField] private Image backGroundImage;

        public void SwitchAuraBackground(AuraType auraType)
        {
            backGroundImage.sprite = auraBackGroundDict[auraType];
        }
        
    }
}
