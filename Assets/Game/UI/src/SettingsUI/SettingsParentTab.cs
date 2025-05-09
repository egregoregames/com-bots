using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src.SettingsUI
{
    public class SettingsParentTab : MenuTab
    {
        Image _frameImage;
        
        protected override void Awake()
        {
            base.Awake();
            _frameImage = GetComponent<Image>();
        }

        protected override void SelectEffect()
        {
            _frameImage.enabled = true;
            onSelect?.Invoke();
        }

        public override void DeselectEffect()
        {
            _frameImage.enabled = false;
        }
    }
}
