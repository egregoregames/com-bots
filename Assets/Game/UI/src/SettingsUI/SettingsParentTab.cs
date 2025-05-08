using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src.SettingsUI
{
    public class SettingsParentTab : MenuTab
    {
        Image _frameImage;
        
        protected override void Start()
        {
            base.Start();
            _frameImage = GetComponent<Image>();
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            SelectEffect();
        }

        protected override void SelectEffect()
        {
            _frameImage.enabled = true;
            onSelect?.Invoke();
        }

    

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            DeselectEffect();
        }

        public override void DeselectEffect()
        {
            _frameImage.enabled = false;
        }
    }
}
