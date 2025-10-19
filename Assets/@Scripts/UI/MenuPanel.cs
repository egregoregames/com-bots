using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public string menuName;
    public string description;
    public List<MenuTab> categoryButtons;
    public InputSO inputSO;
    
    [SerializeField] MenuDescriptionPanel descriptionPanel;
    [SerializeField] Button menuButton;
    [SerializeField] GameObject menuContent;
    [SerializeField] GameObject mainButtonsGameObject;
    [SerializeField] GameObject mainDescriptionGameObject;
    [SerializeField] GameObject mainTopGameObject;

    bool _isMenuOpen;

    public virtual void OpenMenu()
    {
        _isMenuOpen = true;
        menuContent.SetActive(true);
        EventSystem.current.SetSelectedGameObject(categoryButtons[0].gameObject);
    }

    public virtual void CloseMenu()
    {
        if (!_isMenuOpen) return;
        _isMenuOpen = false;
        menuContent.SetActive(false);
        DeselectAllButtons();
    }
    
    protected void SetButtonOnSelect(MenuTab menuTab, Action buttonAction)
    {
        menuTab.onSelect += buttonAction;
    }

    protected virtual void DeselectAllButtons()
    {
        categoryButtons.ForEach(b => b.DeselectEffect());
    }
}
