using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ComBots.UI.src
{
    public class SubMenuPanel : MonoBehaviour
    {
        [SerializeField] InputSO inputSO;
        [SerializeField] GameObject mainMenuPanel;
        [SerializeField] GameObject menuContent;
        [SerializeField] MenuTab parentTab;
        [SerializeField] List<MenuTab> categoryButtons;
        
        GameObject _previouslySelectedGameObject;
        Action _originalOnCancel;
        bool _isMenuOpen;

        public virtual void OpenMenu()
        {
            if (_isMenuOpen) return;

            _isMenuOpen = true;
            
            inputSO.AltCancel += CloseMenu;

            mainMenuPanel.SetActive(false);
            menuContent.SetActive(true);

            _previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(categoryButtons[0].gameObject);
        }

        public virtual void CloseMenu()
        {
            if (!_isMenuOpen) return;

            _isMenuOpen = false;

            inputSO.AltCancel -= CloseMenu;

            mainMenuPanel.SetActive(true);
            menuContent.SetActive(false);

            EventSystem.current.SetSelectedGameObject(_previouslySelectedGameObject);
        }

    }
}
