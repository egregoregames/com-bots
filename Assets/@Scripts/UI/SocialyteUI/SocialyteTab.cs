using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ComBots.UI.src.SocialyteUI
{
    public class SocialyteTab : MenuTab
    {
        GameObject _newCircleGameObject;
        GameObject _inPartySelectedObject;
        GameObject _inPartyUnselectedObject;
        public TextMeshProUGUI buttonText;
        public dataHolderSocialyteProfile connection;

        protected override void Awake()
        {
            base.Awake();
            _newCircleGameObject = transform.GetChild(1).gameObject;
            _inPartySelectedObject = transform.GetChild(2).gameObject;
            _inPartyUnselectedObject = transform.GetChild(3).gameObject;
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
            buttonText.color = Color.white;

            if (connection == null) return;
            
            _inPartySelectedObject.SetActive(connection.inParty);
            _inPartyUnselectedObject.SetActive(false);
            _newCircleGameObject.SetActive(false);
            connection.newConnection = false;
        }

        public override void DeselectEffect()
        {
            base.DeselectEffect();
            buttonText.color = Color.black;
            
            if (connection == null) return;
            
            _inPartyUnselectedObject.SetActive(connection.inParty);
            _inPartySelectedObject.SetActive(false);
        }

        public void SetConnectionStatus()
        {
            if (connection == null) return;
            
            _newCircleGameObject.SetActive(connection.newConnection);
            buttonText.text = connection.profileName;
            _inPartyUnselectedObject.SetActive(connection.inParty);
            _inPartySelectedObject.SetActive(false);
        }
    }
}
