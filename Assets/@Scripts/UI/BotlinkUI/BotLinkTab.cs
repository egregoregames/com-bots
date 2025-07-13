using UnityEngine.EventSystems;

namespace Game.UI.src.BotlinkUI
{
    public class BotLinkTab : MenuTab
    {
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SelectEffect();
        }

        public override void SelectEffect()
        {
            onSelect?.Invoke();
        }
    }
}
