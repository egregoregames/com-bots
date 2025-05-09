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

    [SerializeField] MenuDescriptionPanel descriptionPanel;
    [SerializeField] Button menuButton;
    [SerializeField] GameObject menuContent;

    GameObject _previouslySelectedGameObject;

    public void SetupButtons()
    {
        foreach (var menutab in categoryButtons)
        {
            
        }
    }

    public void OpenMenu()
    {
        menuContent.SetActive(true);
        _previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(categoryButtons[0].gameObject);
    }
    public void CloseMenu()
    {
        menuContent.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_previouslySelectedGameObject);
        DeselectAllButtons();
    }
    

    void DeselectAllButtons()
    {
        categoryButtons.ForEach(b => b.DeselectEffect());
    }
}
