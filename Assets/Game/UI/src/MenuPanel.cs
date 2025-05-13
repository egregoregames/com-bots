using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public string menuName;
    public string description;
    public Image icon;
    public List<MenuTab> categoryButtons;
    public InputSO inputSO;
    
    [SerializeField] MenuDescriptionPanel descriptionPanel;
    [SerializeField] Button menuButton;
    [SerializeField] GameObject menuContent;

    GameObject _previouslySelectedGameObject;

    public virtual void OpenMenu()
    {
        menuContent.SetActive(true);
        _previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(categoryButtons[0].gameObject);
    }
    public virtual void CloseMenu()
    {
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
    
}
