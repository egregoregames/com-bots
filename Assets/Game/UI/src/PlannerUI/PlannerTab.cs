using UnityEngine;

namespace Game.UI.src.PlannerUI
{
    public class PlannerTab : MenuTab
    {
        RectTransform rectTransform;
        Vector2 initialPosition;
        
        protected override void Awake()
        {
            rectTransform = transform.GetComponent<RectTransform>();
            initialPosition = rectTransform.anchoredPosition; // Store initial UI position
        }
        
        public override void SelectEffect()
        {
            var offset = transform.parent.GetComponent<RectTransform>().rect.width * .07f;
        
            if(!rectTransform || !rectTransform.anchoredPosition.Equals(initialPosition)) return;
        
            rectTransform.anchoredPosition += new Vector2(offset, 0);
            onSelect?.Invoke();
        }
        

        public override void DeselectEffect()
        {
            rectTransform.anchoredPosition = initialPosition;
        }
    }
}
