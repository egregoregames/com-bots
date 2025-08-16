using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ComBots.UI.src.SettingsUI
{
    public class SettingsCategoryTab : MenuTab
    {
        readonly List<MenuTab> _subButtons = new();
        Image _backgroundImage;
        Slider _associatedSlider;
        int _currentSubIndex;
        
        protected override void Awake()
        {
            base.Awake();
            _backgroundImage = GetComponent<Image>();
            _associatedSlider = GetComponentInChildren<Slider>();
            _subButtons.AddRange(GetComponentsInChildren<MenuTab>().Where(tab => tab != this).OrderBy(tab => tab.transform.GetSiblingIndex()));
        }

        protected override void Start()
        {
            base.Start();
            
            if (_subButtons.Count > 0)
                _subButtons[0].SelectEffect(); //Default to first button in list for now

            SetDefaultValuesFromSave();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SelectEffect();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            DeselectEffect();
        }

        public override void SelectEffect()
        {
            isSelected = true;
            _backgroundImage.enabled = true;
            onSelect?.Invoke();
        }

        public override void DeselectEffect()
        {
            isSelected = false;
            _backgroundImage.enabled = false;
        }
        
        public override void HandleHorizontalInput(int direction)
        {
            if (!isSelected || _subButtons == null) return;

            if (_associatedSlider == null)
            {
                SetActiveSubButton(direction);
            }
            else
            {
                _associatedSlider.value += direction * (_associatedSlider.wholeNumbers ? 1 : 0.1f);
            }
        }

        void SetActiveSubButton(int direction)
        {
            int newIndex = Mathf.Clamp(_currentSubIndex + direction, 0, _subButtons.Count - 1);
            _currentSubIndex = newIndex;
                
            for (int i = 0; i < _subButtons.Count; i++)
            {
                if (i == _currentSubIndex)
                {
                    _subButtons[i].SelectEffect();
                }
                else
                {
                    _subButtons[i].DeselectEffect();
                }
            }
        }

        void SetDefaultValuesFromSave()
        {
            //TODO: Load saved settings and set those buttons as the active ones
        }
    }
}
