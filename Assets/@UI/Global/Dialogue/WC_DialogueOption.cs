using ComBots.UI.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ComBots.Global.UI.Dialogue
{
    public class WC_DialogueOption : WC_ListerOption
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _bg_highlighted;

        public void Setup(string optionText)
        {
            _text.text = optionText;
        }

        void OnDestroy()
        {
            _text = null;
        }

        override public void SetIsHighlighted(bool highlighted)
        {
            base.SetIsHighlighted(highlighted);
            _text.color = highlighted ? Color.white : Color.black;
            _bg_highlighted.enabled = highlighted;
        }
    }
}