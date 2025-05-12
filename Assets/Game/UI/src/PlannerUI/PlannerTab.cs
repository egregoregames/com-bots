using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src.PlannerUI
{
    public class PlannerTab : MenuTab
    {
        const string CompletedColorString = "#8D989E";

        RectTransform rectTransform;
        GameObject newCircleGameObject;
        GameObject completedLabelGameObject;
        Image buttonBackground;
        TextMeshProUGUI buttonText;
        Vector2 initialPosition;
        Color _completedBackgroundColor;
        
        protected override void Awake()
        {
            base.Awake();
            
            ColorUtility.TryParseHtmlString(CompletedColorString, out _completedBackgroundColor);
            rectTransform = transform.GetComponent<RectTransform>();
            buttonBackground = transform.GetComponent<Image>();
            buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            completedLabelGameObject = transform.GetChild(1).gameObject;
            newCircleGameObject = transform.GetChild(2).gameObject;
            initialPosition = rectTransform.anchoredPosition; // Store initial UI position
        }
        
        public override void SelectEffect()
        {
            var offset = transform.parent.GetComponent<RectTransform>().rect.width * .07f;
        
            if(!rectTransform || !rectTransform.anchoredPosition.Equals(initialPosition)) return;
        
            rectTransform.anchoredPosition += new Vector2(offset, 0);
            newCircleGameObject.SetActive(false);
            onSelect?.Invoke();
        }
        

        public override void DeselectEffect()
        {
            rectTransform.anchoredPosition = initialPosition;
        }

        public void SetQuestStatuses(bool isNewQuest, bool isCompleted)
        {
            newCircleGameObject.SetActive(isNewQuest);

            if (isCompleted)
            {
                completedLabelGameObject.SetActive(true);
                buttonBackground.color = _completedBackgroundColor;
                buttonText.color = Color.white;
            }
            else
            {
                completedLabelGameObject.SetActive(false);
                buttonBackground.color = Color.white;
                buttonText.color = Color.black;
            }
        }
    }
}
