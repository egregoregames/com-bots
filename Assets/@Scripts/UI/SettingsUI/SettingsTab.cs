using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src.SettingsUI
{
    public class SettingsTab : MenuTab
    {
        const string SelectedColorString = "#8E999F";
        
        TextMeshProUGUI _buttonText;
        Image _backgroundImage;
        Sprite _originalSprite;
        Sprite _selectedSprite;
        
        Color _unselectedTextColor;

        protected override void Awake()
        {
            base.Awake();
            ColorUtility.TryParseHtmlString(SelectedColorString, out _unselectedTextColor);
            _backgroundImage = GetComponent<Image>();
            _originalSprite = _backgroundImage.sprite;
            _selectedSprite = spriteState.selectedSprite;
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SelectEffect();
        }

        public override void SelectEffect()
        {
            _backgroundImage.sprite = _selectedSprite;
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
            _backgroundImage.sprite = _originalSprite;
            _buttonText.color = _unselectedTextColor;
        }
    }
}
