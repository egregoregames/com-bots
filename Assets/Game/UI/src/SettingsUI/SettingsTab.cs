using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src.SettingsUI
{
    public class SettingsTab : MenuTab
    {
        const string SelectedColorString = "#8D989E";
        
        Image _buttonBackground;
        TextMeshProUGUI _buttonText;
        
        Color _selectedBackgroundColor;

        protected override void Awake()
        {
            base.Awake();
            ColorUtility.TryParseHtmlString(SelectedColorString, out _selectedBackgroundColor);
            _buttonBackground = GetComponent<Image>();
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SelectEffect();
        }

        public override void SelectEffect()
        {
            _buttonBackground.color = _selectedBackgroundColor;
            _buttonText.color = Color.white;
            onSelect?.Invoke();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            DeselectEffect();
        }

        public override void DeselectEffect()
        {
            _buttonBackground.color = Color.white;
            _buttonText.color = Color.black;
        }
    }
}
