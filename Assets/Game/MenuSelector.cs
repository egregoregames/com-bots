using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] InputSO inputSo;
    [FormerlySerializedAs("MenuButtonObjects")] public List<MenuButtonObject> menuButtonObjects;
    [SerializeField] MenuDescriptionPanel menuDescriptionPanel;
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Store initial UI position

        menuButtonObjects.ForEach(o => o.Init(menuDescriptionPanel, inputSo, SetButtonsInteractable));
        
    }

    public void SetButtonsInteractable(bool status)
    {
        menuButtonObjects.ForEach(t => t.menuButton.interactable = status);
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
        EventSystem.current.SetSelectedGameObject(menuButtonObjects[0].menuButton.gameObject);

    }
    [ContextMenu("Hide")]
    public void Hide()
    {
        rectTransform.anchoredPosition = initialPosition;
        EventSystem.current.SetSelectedGameObject(null);
    }
}

[Serializable]
public class MenuButtonObject
{

    public void Init(MenuDescriptionPanel menuPanelDescription, InputSO inputSo, Action<bool> setMenuSelectorInteractability)
    {
        // set menu panel to open from button
        menuButton.onClick.AddListener(() => menuPanel.gameObject.SetActive(true));
        menuButton.onClick.AddListener(() => EventSystem.current.SetSelectedGameObject(menuPanel.buttons[0].gameObject));

        menuButton.onClick.AddListener(() => setMenuSelectorInteractability?.Invoke(false));

        // and to close from 'S' Key
        inputSo.OnCancel += () => menuPanel.gameObject.SetActive(false);
        inputSo.OnCancel += () => setMenuSelectorInteractability?.Invoke(true);

        menuButton.onSelect += () =>
        {
            menuPanelDescription.menuPanelDescription.text = description;
            menuPanelDescription.menuPanelIcon = icon;
        };
        
    }
    
    [FormerlySerializedAs("button")] public ScalableButton menuButton;
    public MenuPanel menuPanel;
    public string description;
    public Image icon;
}


