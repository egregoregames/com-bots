using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI.src.PlannerUI
{
    public class PlannerTab : MenuTab
    {
        public GameObject selectedGameObject;
        
        GameObject _newCircleGameObject;
        Image _buttonBackground;
        TextMeshProUGUI _buttonText;
        Sprite _originalSprite;
        Sprite _selectedSprite;
        Sprite _completedSprite;
        bool _isCompleted;
        
        protected override void Awake()
        {
            base.Awake();
            
            _buttonBackground = transform.GetComponent<Image>();
            _buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            selectedGameObject = transform.GetChild(1).gameObject;
            _newCircleGameObject = transform.GetChild(2).gameObject;
            _originalSprite = _buttonBackground.sprite;
            _selectedSprite = spriteState.highlightedSprite;
            _completedSprite = spriteState.disabledSprite;
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

            if (!_isCompleted)
            {
                _buttonText.color = Color.white;
                _buttonBackground.sprite = _selectedSprite;
            }
            
            _newCircleGameObject.SetActive(false);
        }

        public override void DeselectEffect()
        {
            base.DeselectEffect();
            if (!_isCompleted)
            {
                _buttonText.color = Color.black;
                _buttonBackground.sprite = _originalSprite;
            }
        }

        public void SetQuestStatuses(bool isNewQuest, bool isCompleted)
        {
            _newCircleGameObject.SetActive(isNewQuest);
            _isCompleted = isCompleted;

            if (_isCompleted)
            {
                _buttonBackground.sprite = _completedSprite;
                _buttonText.color = Color.white;
            }
            else
            {
                _buttonBackground.sprite = _originalSprite;
                _buttonText.color = Color.black;
            }
        }
        
        
    }
}
