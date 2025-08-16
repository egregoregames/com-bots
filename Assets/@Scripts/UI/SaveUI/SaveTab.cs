using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ComBots.UI.src.SaveUI
{
    public class SaveTab : MenuTab
    {
        TextMeshProUGUI _buttonText;
        
        protected override void Awake()
        {
            base.Awake();
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
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
            _buttonText.color = Color.white;
            onSelect?.Invoke();
        }
        public override void DeselectEffect()
        {
            _buttonText.color = Color.black;
        }
    }
}
