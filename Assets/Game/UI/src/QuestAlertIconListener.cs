using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src
{
    public class QuestAlertIconListener : MonoBehaviour, IMessageHandler
    {
        public Image alertIcon;
        public Sprite newIcon;
        public Sprite updateIcon;
        public Sprite completeIcon;
        const string TAG = "SwapQuestIcon";
        
        
        void OnEnable()
        {
            MessageSystem.AddListener(this, TAG, "");
        }

        void OnDisable()
        {
            MessageSystem.RemoveListener(this);
        }

        public void OnMessage(MessageArgs messageArgs)
        {
            string param = messageArgs.parameter;
            var color = alertIcon.color;
            color.a = 1;
            alertIcon.color = color;
            
            switch (param)
            {
                case "new":
                    alertIcon.sprite = newIcon;
                    break;
                case "update":
                    alertIcon.sprite = updateIcon;
                    break;
                case "complete":
                    alertIcon.sprite = completeIcon;
                    break;
                default:
                    alertIcon.sprite = null;
                    break;
            }
        }
    }
}