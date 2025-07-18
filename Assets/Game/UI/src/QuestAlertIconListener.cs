using PixelCrushers;
using UnityEngine;


namespace Game.UI.src
{
    public class QuestAlertIconListener : MonoBehaviour, IMessageHandler
    {
        [SerializeField] Sprite newIcon;
        [SerializeField] Sprite updateIcon;
        [SerializeField] Sprite completeIcon;
        QuestIconUpdater _questIconUpdater;
        const string TAG = "SwapQuestIcon";
        
        void OnEnable()
        {
            _questIconUpdater = transform.GetComponentInChildren<QuestIconUpdater>();
            MessageSystem.AddListener(this, TAG, "");
        }

        void OnDisable()
        {
            MessageSystem.RemoveListener(this);
        }

        public void OnMessage(MessageArgs messageArgs)
        {
            string param = messageArgs.parameter.ToLower();
            switch (param)
            {
                case "new":
                    _questIconUpdater.EnqueueIcon(newIcon);
                    break;
                case "update":
                    _questIconUpdater.EnqueueIcon(updateIcon);
                    break;
                case "complete":
                    _questIconUpdater.EnqueueIcon(completeIcon);
                    break;
            }
        }
    }
}