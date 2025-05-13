using System;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainHudButtons : MonoBehaviour
{
    [SerializeField] InputSO inputSo;
    public List<HudButtonToMenuPair> buttonToMenuPairs;
    [SerializeField] MenuDescriptionPanel menuDescriptionPanel;
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    public MenuPanel activePanel;
    private void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Store initial UI position
        
        for (var i = 0; i < buttonToMenuPairs.Count; i++)
        {
            buttonToMenuPairs[i].Init(menuDescriptionPanel, inputSo, this, i);
        }
        
        
        inputSo.OnCancel += () => SetHudButtonsInteractability(true);
        inputSo.OnCancel += () => activePanel?.CloseMenu();
    }

    public void SetHudButtonsInteractability(bool status)
    {
        buttonToMenuPairs.ForEach(t => t.menuButton.interactable = status);
    }

    [ContextMenu("Show ob")]
    public void ShowSelected()
    {
        if(EventSystem.current.currentSelectedGameObject)
            Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    [ContextMenu("Show")]
    public void Show()
    {
        Vector2 targetPosition = new Vector2(0, 0);
        rectTransform.anchoredPosition = targetPosition;
        menuDescriptionPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttonToMenuPairs[0].menuButton.gameObject);

    }
    [ContextMenu("Hide")]
    public void Hide()
    {
        rectTransform.anchoredPosition = initialPosition;
        menuDescriptionPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}

[Serializable]
public class HudButtonToMenuPair
{
    public ScalableButton menuButton;
    public MenuPanel menuPanel;
    
    public void Init(MenuDescriptionPanel menuPanelDescription, InputSO inputSo, MainHudButtons hudButtons, int index)
    {
        // set menu panel to open from button
        menuButton.onClick.AddListener(() => menuPanel.OpenMenu());
        menuButton.onClick.AddListener(() => hudButtons.activePanel = menuPanel);
        menuButton.onClick.AddListener(() =>  hudButtons.SetHudButtonsInteractability(false));

        menuButton.onSelect += () =>
        {
            menuPanelDescription.SetDescription(menuPanel, index);
        };
        
    }
}



