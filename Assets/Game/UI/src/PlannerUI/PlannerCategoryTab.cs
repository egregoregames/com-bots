using UnityEngine.UI;

namespace Game.UI.src.PlannerUI
{
    public class PlannerCategoryTab : MenuTab
    {
        Image _backgroundImage;
        
        protected override void Awake()
        {
            base.Awake();
            _backgroundImage = GetComponent<Image>();
        }

        public override void SelectEffect()
        {
            _backgroundImage.enabled = true;
            onSelect?.Invoke();
        }

        public override void DeselectEffect()
        {
            _backgroundImage.enabled = false;
        }
    }
}
