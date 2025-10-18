using ComBots.Game;
using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Logs;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class PauseMenu : MonoBehaviourR3
    {
        public static PauseMenu Instance { get; private set; }
        public bool IsOpen { get; private set; }

        [field: SerializeField]
        private InputSystem_Actions Inputs { get; set; }

        [field: SerializeField]
        private RectTransform MenuSelector { get; set; }

        [field: SerializeField]
        private float MenuSelectorYPartial { get; set; } = -108f;

        [field: SerializeField]
        private float MenuSelectorYHidden { get; set; } = -200f;

        [field: SerializeField]
        private float MenuSelectorYVisible { get; set; } = 0f;

        [field: SerializeField]
        private float MovementSpeed { get; set; } = 5f;

        [field: SerializeField]
        private List<ScalableButton> Buttons { get; set; }

        private BottomState _currentBottomState = BottomState.Partial;
        private int _selectedButtonIndex;
        private bool _isMoving;
        private float _movementProgress = 0;
        private float _startingYPosition;

        protected override void Initialize()
        {
            base.Initialize();
            Instance = this;
            Inputs = new();

            var onOpenMenu = Observable.FromEvent<InputAction.CallbackContext>(
                h => Inputs.Player.OpenMenu.performed += h,
                h => Inputs.Player.OpenMenu.performed -= h);

            AddEvents(
                onOpenMenu.Subscribe(OpenMenu_performed));
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

        private void OpenMenu_performed(InputAction.CallbackContext obj)
        {
            SetActive(!IsOpen);
        }

        public void SetActive(bool isActive)
        {
            MyLogger<PauseMenu>.StaticLog($"SetActive({isActive})");
            //gameObject.SetActive(isActive);
            SetBottomState(isActive ? BottomState.Visible : BottomState.Partial);
            IsOpen = isActive;
            UpdateButtonSelection();
        }

        private void UpdateButtonSelection()
        {
            if (IsOpen)
            {
                Buttons[_selectedButtonIndex].Select();
            }
            else
            {
                var selected = EventSystem.current.currentSelectedGameObject;

                if (selected != null)
                {
                    var matching = Buttons
                        .FirstOrDefault(x => x.gameObject == selected);

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
                SetBottomState(IsOpen ? BottomState.Visible : BottomState.Partial);
            }
            else
            {
                SetBottomState(BottomState.Hidden);
            }
        }

        private enum BottomState
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
            float destinationY = 0;
            switch (_currentBottomState)
            {
                case BottomState.Partial:
                    destinationY = MenuSelectorYPartial;
                    break;
                case BottomState.Hidden:
                    destinationY = MenuSelectorYHidden;
                    break;
                case BottomState.Visible:
                    destinationY = MenuSelectorYVisible;
                    break;
            }

            float y = Mathf.Lerp(_startingYPosition, destinationY, _movementProgress);
            MenuSelector.anchoredPosition = new Vector2(MenuSelector.anchoredPosition.x, y);

            if (_movementProgress >= 1)
            {
                _isMoving = false;
                _movementProgress = 0;
            }
        }

        private void SetBottomState(BottomState state)
        {
            _currentBottomState = state;
            _isMoving = true;
            _startingYPosition = MenuSelector.anchoredPosition.y;
            _movementProgress = 0;
        }

        public void Open()
        {
            SetActive(true);
        }

        public void Close()
        {
            SetActive(false);
        }
    }
}