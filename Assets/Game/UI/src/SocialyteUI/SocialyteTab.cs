using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.src.SocialyteUI
{
    public class SocialyteTab : MenuTab
    {
        GameObject _newCircleGameObject;
        public TextMeshProUGUI buttonText;
        public dataHolderSocialyteProfile connection;

        protected override void Awake()
        {
            base.Awake();
            _newCircleGameObject = transform.GetChild(1).gameObject;
            buttonText = transform.GetComponentInChildren<TextMeshProUGUI>();
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
            base.SelectEffect();

            if (connection == null || !connection.newConnection) return;
            
            _newCircleGameObject.SetActive(false);
            connection.newConnection = false;
        }
        
        public void SetConnectionStatus()
        {
            if (connection == null) return;
            
            _newCircleGameObject.SetActive(connection.newConnection);
            buttonText.text = connection.profileName;
        }
    }
}
