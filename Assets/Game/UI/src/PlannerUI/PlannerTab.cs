using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src.PlannerUI
{
    public class PlannerTab : MenuTab
    {
        const string CompletedColorString = "#8D989E";
        
        GameObject newCircleGameObject;
        GameObject completedLabelGameObject;
        Image buttonBackground;
        TextMeshProUGUI buttonText;
        Color _completedBackgroundColor;
        
        protected override void Awake()
        {
            base.Awake();
            
            ColorUtility.TryParseHtmlString(CompletedColorString, out _completedBackgroundColor);
            buttonBackground = transform.GetComponent<Image>();
            buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            completedLabelGameObject = transform.GetChild(1).gameObject;
            newCircleGameObject = transform.GetChild(2).gameObject;
        }
        
        public override void SelectEffect()
        {
            base.SelectEffect();
            newCircleGameObject.SetActive(false);
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
