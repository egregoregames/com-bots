using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src
{
    public class MainHudButtons : MonoBehaviour
    {
        [SerializeField] InputSO inputSo;
        public List<HudButtonToMenuPair> buttonToMenuPairs;
        [SerializeField] MenuDescriptionPanel menuDescriptionPanel;
        RectTransform _rectTransform;
        Vector2 _initialPosition;

        public MenuPanel activePanel;

        void Awake()
        {
            _rectTransform = transform.GetComponent<RectTransform>();
            _initialPosition = _rectTransform.anchoredPosition; // Store initial UI position
        
            for (var i = 0; i < buttonToMenuPairs.Count; i++)
            {
                buttonToMenuPairs[i].Init(menuDescriptionPanel, inputSo, this, i);
            }

            SetupNavigationWrapHorizontal(buttonToMenuPairs);
            
            inputSo.OnCancel += () => SetHudButtonsInteractability(true);
            inputSo.OnCancel += () => activePanel?.CloseMenu();
        }

        public void SetHudButtonsInteractability(bool status)
        {
            buttonToMenuPairs.ForEach(t => t.menuButton.interactable = status);
        }

        [ContextMenu("Show ob")]
        public void ShowSelected()
        {
            if(EventSystem.current.currentSelectedGameObject)
                Debug.Log(EventSystem.current.currentSelectedGameObject);
        }

        [ContextMenu("Show")]
        public void Show()
        {
            Vector2 targetPosition = new Vector2(0, 0);
            _rectTransform.anchoredPosition = targetPosition;
            menuDescriptionPanel.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(buttonToMenuPairs[0].menuButton.gameObject);

        }
        [ContextMenu("Hide")]
        public void Hide()
        {
            _rectTransform.anchoredPosition = _initialPosition;
            menuDescriptionPanel.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
        
        void SetupNavigationWrapHorizontal(List<HudButtonToMenuPair> buttons)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Navigation nav = buttons[i].menuButton.navigation;
                nav.mode = Navigation.Mode.Explicit;

                int leftIndex = (i - 1 + buttons.Count) % buttons.Count;
                int rightIndex = (i + 1) % buttons.Count;

                nav.selectOnLeft = buttons[leftIndex].menuButton;
                nav.selectOnRight = buttons[rightIndex].menuButton;

                buttons[i].menuButton.navigation = nav;
            }
        }


    }

    [Serializable]
    public class HudButtonToMenuPair
    {
        public ScalableButton menuButton;
        public MenuPanel menuPanel;
    
        public void Init(MenuDescriptionPanel menuPanelDescription, InputSO inputSo, MainHudButtons hudButtons, int index)
        {
            // set menu panel to open from button
            menuButton.onClick.AddListener(() => menuPanel.OpenMenu());
            menuButton.onClick.AddListener(() => hudButtons.activePanel = menuPanel);
            menuButton.onClick.AddListener(() =>  hudButtons.SetHudButtonsInteractability(false));

            menuButton.onSelect += () =>
            {
                menuPanelDescription.SetDescription(menuPanel, index);
            };
        
        }
    }
}