
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Save
{
    public class SaveTab : MenuTab
    {
        GameObject _fingerGameObject;
        
        protected override void Awake()
        {
            base.Awake();
            _fingerGameObject = transform.GetChild(1).gameObject;
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
            _fingerGameObject.SetActive(true);
            onSelect?.Invoke();
        }
        public override void DeselectEffect()
        {
            _fingerGameObject.SetActive(false);
        }
    }
}
