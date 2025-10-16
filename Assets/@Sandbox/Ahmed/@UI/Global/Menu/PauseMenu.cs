using System;
using System.Collections.Generic;
using ComBots.Game;
using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Logs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class PauseMenu : MonoBehaviourR3
    {
        public static PauseMenu Instance { get; private set; }
        [SerializeField] MenuNavigationController _navigationController;
        private bool _isMenuOpen;

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

        private BottomState _currentBottomState = BottomState.Partial;
        private bool _isMoving;
        private float _movementProgress = 0;
        private float _startingYPosition;

        protected override void Initialize()
        {
            base.Initialize();
            Instance = this;
        }

        public void SetActive(bool isActive)
        {
            MyLogger<PauseMenu>.StaticLog($"SetActive({isActive})");
            //gameObject.SetActive(isActive);
            SetBottomState(isActive ? BottomState.Visible : BottomState.Partial);
            _isMenuOpen = isActive;
            _navigationController.SetActive(isActive);
        }

        public void SetBottomBarVisible(bool isVisible)
        {
            if (isVisible)
            {
                SetBottomState(_isMenuOpen ? BottomState.Visible : BottomState.Partial);
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