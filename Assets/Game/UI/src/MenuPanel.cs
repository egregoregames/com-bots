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

    GameObject _previouslySelectedGameObject;

    public virtual void OpenMenu()
    {
        menuContent.SetActive(true);
        _previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(categoryButtons[0].gameObject);
        ToggleMainHud(false);
    }
    public virtual void CloseMenu()
    {
        ToggleMainHud(true);
        menuContent.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_previouslySelectedGameObject);
        DeselectAllButtons();
    }
    
    protected void SetButtonOnSelect(MenuTab menuTab, Action buttonAction)
    {
        menuTab.onSelect += buttonAction;
    }

    void DeselectAllButtons()
    {
        categoryButtons.ForEach(b => b.DeselectEffect());
    }
    
    void ToggleMainHud(bool isActive)
    {
        mainButtonsGameObject.SetActive(isActive);
        mainDescriptionGameObject.SetActive(isActive);
        mainTopGameObject.SetActive(isActive);
    }
}
