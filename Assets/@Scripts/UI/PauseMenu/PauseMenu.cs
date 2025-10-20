using PixelCrushers.DialogueSystem;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class PauseMenu : MonoBehaviourR3
    {
        public static PauseMenu Instance { get; private set; }

        private static UnityEventR3 _onButtonsVisible = new();
        public static IDisposable OnButtonsVisible(Action x) => _onButtonsVisible.Subscribe(x);

        private static UnityEventR3 _onButtonsMinimized = new();
        public static IDisposable OnButtonsMinimized(Action x) => _onButtonsMinimized.Subscribe(x);

        public bool IsOpen { get; private set; }

        private InputSystem_Actions Inputs { get; set; }

        [field: SerializeField]
        private RectTransform MenuSelector { get; set; }

        [field: SerializeField]
        private RectTransform TopBar { get; set; }

        [field: SerializeField]
        private float MenuSelectorYPartial { get; set; } = -108f;

        [field: SerializeField]
        private float MenuSelectorYHidden { get; set; } = -200f;

        [field: SerializeField]
        private float MenuTopBarYHidden { get; set; } = 200f;

        [field: SerializeField]
        private float MenuSelectorYVisible { get; set; } = 0f;

        [field: SerializeField]
        private float MovementSpeed { get; set; } = 5f;

        [field: SerializeField]
        private List<PauseMenuButton> Buttons { get; set; }

        [Serializable]
        private class PauseMenuButton
        {
            [field: SerializeField]
            public ScalableButton Button { get; private set; }

            [field: SerializeField]
            public GameObject MenuToOpen { get; private set; }
        }

        private Visibility _currentBottomState = Visibility.Partial;
        private int _selectedButtonIndex;
        private bool _isMoving;
        private float _movementProgress = 0;
        private float _startingYPositionBottom;
        private float _startingYPositionTop;

        protected override void Initialize()
        {
            base.Initialize();
            Instance = this;
            Inputs = new();

            var onOpenMenu = Observable.FromEvent<InputAction.CallbackContext>(
                h => Inputs.Player.OpenMenu.performed += h,
                h => Inputs.Player.OpenMenu.performed -= h);

            DialogueManager.instance.conversationStarted += ConversationStarted;
            DialogueManager.instance.conversationEnded += ConversationEnded;

            AddEvents(
                onOpenMenu.Subscribe(_ => ToggleIsOpen(!IsOpen)),
                PauseMenuApp.OnMenuOpened(UpdateVisibility),
                PauseMenuApp.OnMenuClosed(UpdateVisibility));

            InitializeButtons();
            SetVisibility(Visibility.Partial);
        }

        private void ConversationEnded(Transform t)
        {
            SetBottomBarVisible(true);
        }

        private void ConversationStarted(Transform t)
        {
            SetBottomBarVisible(false);
        }

        private new void OnEnable()
        {
            base.OnEnable();
            Inputs.Enable();
        }

        private void OnDisable()
        {
            Inputs.Disable();
        }

        private new void OnDestroy()
        {
            base.OnDestroy();
            DialogueManager.instance.conversationStarted -= ConversationStarted;
            DialogueManager.instance.conversationEnded -= ConversationEnded;
        }

        private void ToggleIsOpen(bool isOpen)
        {
            IsOpen = isOpen;
            UpdateVisibility();
        }

        public void UpdateVisibility()
        {
            
            if (DialogueManager.instance.activeConversation != null || PauseMenuApp.IsAnyOpen)
            {
                SetVisibility(Visibility.Hidden);
            }
            else
            {
                SetVisibility(IsOpen ? Visibility.Visible : Visibility.Partial);
            }
                
            UpdateButtonSelection();

            if (_currentBottomState == Visibility.Visible)
            {
                _onButtonsVisible?.Invoke();
            }
            else
            {
                _onButtonsMinimized?.Invoke();
            }
        }

        private void InitializeButtons()
        {
            foreach (var button in Buttons)
            {
                var menu = button.MenuToOpen;
                button.Button.onClick.AddListener(() =>
                {
                    Buttons.ForEach(x => x.MenuToOpen.SetActive(false));
                    menu.SetActive(true);
                });
            }
        }

        private void UpdateButtonSelection()
        {
            if (_currentBottomState == Visibility.Visible)
            {
                Buttons[_selectedButtonIndex].Button.Select();
            }
            else
            {
                var selected = EventSystem.current.currentSelectedGameObject;

                if (selected != null)
                {
                    var matching = Buttons
                        .FirstOrDefault(x => x.Button.gameObject == selected);

                    if (matching != null)
                    {
                        _selectedButtonIndex = Buttons.IndexOf(matching);
                    }
                }

                foreach (var item in Buttons)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        public void SetBottomBarVisible(bool isVisible)
        {
            if (isVisible)
            {
                SetVisibility(IsOpen ? Visibility.Visible : Visibility.Partial);
            }
            else
            {
                SetVisibility(Visibility.Hidden);
            }
        }

        private enum Visibility
        {
            Partial,
            Hidden,
            Visible
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (!_isMoving) return;

            _movementProgress += Time.deltaTime * MovementSpeed;
            float destinationYBottom = 0;
            float destinationYTop = 0;
            switch (_currentBottomState)
            {
                case Visibility.Partial:
                    destinationYBottom = MenuSelectorYPartial;
                    destinationYTop = MenuTopBarYHidden;
                    break;
                case Visibility.Hidden:
                    destinationYBottom = MenuSelectorYHidden;
                    destinationYTop = MenuTopBarYHidden;
                    break;
                case Visibility.Visible:
                    destinationYBottom = MenuSelectorYVisible;
                    destinationYTop = 0;
                    break;
            }

            float yBottom = Mathf.Lerp(_startingYPositionBottom, destinationYBottom, _movementProgress);
            float yTop = Mathf.Lerp(_startingYPositionTop, destinationYTop, _movementProgress);
            MenuSelector.anchoredPosition = new Vector2(MenuSelector.anchoredPosition.x, yBottom);
            TopBar.anchoredPosition = new Vector2(TopBar.anchoredPosition.x, yTop);

            if (_movementProgress >= 1)
            {
                _isMoving = false;
                _movementProgress = 0;
            }
        }

        private void SetVisibility(Visibility state)
        {
            _currentBottomState = state;
            _isMoving = true;
            _startingYPositionBottom = MenuSelector.anchoredPosition.y;
            _startingYPositionTop = TopBar.anchoredPosition.y;
            _movementProgress = 0;
        }

        //public void Open()
        //{
        //    UpdateVisibility();
        //}

        //public void Close()
        //{
        //    UpdateVisibility();
        //}
    }
}