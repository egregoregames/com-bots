using UnityEngine.EventSystems;

namespace ComBots.UI.src.BotlinkUI
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
