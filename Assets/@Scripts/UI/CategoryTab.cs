using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src
{
    public class CategoryTab : MenuTab
    {
        protected string SelectedColorString = "#FFFFFF";
        
        Image _buttonBackground;
        TextMeshProUGUI _buttonText;
        Sprite _originalSprite;
        Sprite _selectedSprite;
        Color _unselectedTextColor;
        
        protected override void Awake()
        {
            base.Awake();
            ColorUtility.TryParseHtmlString(SelectedColorString, out _unselectedTextColor);
            _buttonBackground = GetComponent<Image>();
            _originalSprite = _buttonBackground.sprite;
            _selectedSprite = spriteState.highlightedSprite;
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
            
        }

        public override void SelectEffect()
        {
            _buttonBackground.sprite = _selectedSprite;
            
            if(_buttonText != null)
                _buttonText.color = Color.white;
            
            onSelect?.Invoke();
        }

        public override void DeselectEffect()
        {
            _buttonBackground.sprite = _originalSprite;
            
            if(_buttonText != null)
                _buttonText.color = _unselectedTextColor;
        }
    }
}
