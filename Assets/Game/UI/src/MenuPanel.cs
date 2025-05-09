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
        
        inputSO.OnLeft += HandleLeftInput;
        inputSO.OnRight += HandleRightInput;
    }
    public void CloseMenu()
    {
        menuContent.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_previouslySelectedGameObject);
        DeselectAllButtons();
        
        inputSO.OnLeft -= HandleLeftInput;
        inputSO.OnRight -= HandleRightInput;
    }
    

    void DeselectAllButtons()
    {
        categoryButtons.ForEach(b => b.DeselectEffect());
    }
    
    private void HandleLeftInput()
    {
        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null && selected.TryGetComponent(out MenuTab tab))
        {
            tab.HandleHorizontalInput(-1);
        }
    }

    private void HandleRightInput()
    {
        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null && selected.TryGetComponent(out MenuTab tab))
        {
            tab.HandleHorizontalInput(1);
        }
    }
}
