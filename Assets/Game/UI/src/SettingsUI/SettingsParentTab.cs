using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src.SettingsUI
{
    public class SettingsParentTab : MenuTab
    {
        Image _frameImage;
        List<MenuTab> subButtons = new();
        Slider _associatedSlider;
        int currentSubIndex;
        
        protected override void Awake()
        {
            base.Awake();
            _frameImage = GetComponent<Image>();
            subButtons.AddRange(GetComponentsInChildren<MenuTab>().Where(tab => tab != this));
            _associatedSlider = GetComponentInChildren<Slider>();
        }

        public override void SelectEffect()
        {
            isSelected = true;
            _frameImage.enabled = true;
            onSelect?.Invoke();
        }

        public override void DeselectEffect()
        {
            isSelected = false;
            _frameImage.enabled = false;
        }
        
        public override void HandleHorizontalInput(int direction)
        {
            if (!isSelected || subButtons == null) return;

            if (_associatedSlider == null)
            {
                int newIndex = Mathf.Clamp(currentSubIndex + direction, 0, subButtons.Count - 1);
                currentSubIndex = newIndex;
                
                for (int i = 0; i < subButtons.Count; i++)
                {
                    if (i == currentSubIndex)
                    {
                        subButtons[i].SelectEffect();
                    }
                    else
                    {
                        subButtons[i].DeselectEffect();
                    }
                }
            }
            else
            {
                _associatedSlider.value += direction * (_associatedSlider.wholeNumbers ? 1 : 0.1f);
            }
        }
    }
}
