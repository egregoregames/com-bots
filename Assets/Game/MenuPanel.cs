using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public string description;
    public Image icon;
    public List<MenuTab> buttons;

    [SerializeField] MenuDescriptionPanel descriptionPanel;
    [SerializeField] Button menuButton;
    [SerializeField] GameObject menuContent;

    private GameObject previouslySelectedGameObject;

    public void SetupButtons()
    {
        foreach (var menutab in buttons)
        {
            
        }
    }

    public void OpenMenu()
    {
        menuContent.SetActive(true);
        previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }
    public void CloseMenu()
    {
        menuContent.SetActive(false);
        EventSystem.current.SetSelectedGameObject(previouslySelectedGameObject);
        DeselectAllButtons();
    }
    

    void DeselectAllButtons()
    {
        buttons.ForEach(b => b.DeselectEffect());
    }
}
