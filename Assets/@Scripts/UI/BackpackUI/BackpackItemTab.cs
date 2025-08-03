using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ComBots.UI.src.BackpackUI
{
    public class BackpackItemTab : MenuTab
    {
        public TextMeshProUGUI amountText;
        public Image circleImage;
        public Image itemImage;
        Image _buttonFrame;
        
        protected override void Awake()
        {
            base.Awake();
            _buttonFrame = GetComponent<Image>();
            amountText = GetComponentInChildren<TextMeshProUGUI>();
            circleImage = transform.GetChild(1).GetComponent<Image>();
            itemImage = transform.GetChild(2).GetComponent<Image>();
        }

        public override void SelectEffect()
        {
            _buttonFrame.enabled = true;
            onSelect?.Invoke();
        }

        public override void DeselectEffect()
        {
            _buttonFrame.enabled = false;
        }
    }
}
