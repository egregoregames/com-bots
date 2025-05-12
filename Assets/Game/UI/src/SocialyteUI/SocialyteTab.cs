using UnityEngine.EventSystems;

namespace Game.UI.src.SocialyteUI
{
    public class SocialyteTab : MenuTab
    {
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
    }
}
