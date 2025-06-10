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
            
            inputSo.OnCancel += () => SetHudButtonsInteractability(true);
            inputSo.OnCancel += () => SetupNavigationWrapHorizontal(buttonToMenuPairs);;
            inputSo.OnCancel += () => activePanel?.CloseMenu();
        }

        public void SetHudButtonsInteractability(bool status)
        {
            buttonToMenuPairs.ForEach(t => t.menuButton.interactable = status);
        }
        
        public void SetupNavigationWrapHorizontal(List<HudButtonToMenuPair> buttons)
        {
            List<HudButtonToMenuPair> activeButtons = buttons
                .Where(b => b.menuButton != null && b.menuButton.gameObject.activeInHierarchy)
                .ToList();

            for (int i = 0; i < activeButtons.Count; i++)
            {
                var currentButton = activeButtons[i].menuButton;
                Navigation nav = currentButton.navigation;
                nav.mode = Navigation.Mode.Explicit;

                int leftIndex = (i - 1 + activeButtons.Count) % activeButtons.Count;
                int rightIndex = (i + 1) % activeButtons.Count;

                nav.selectOnLeft = activeButtons[leftIndex].menuButton;
                nav.selectOnRight = activeButtons[rightIndex].menuButton;

                currentButton.navigation = nav;
            }
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