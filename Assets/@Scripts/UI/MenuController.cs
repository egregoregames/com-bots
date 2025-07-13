using System;
using Game.src;
using Game.UI.src;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private UISo uiSo;
    [SerializeField] private InputSO inputSo;
    [SerializeField] NewRoomSelectionPanel roomSelectionPanel;
    [SerializeField] MainHudButtons mainHudButtons;
    [SerializeField] GameObject menuContentParent;
    [SerializeField] ChatBubble chatBubble;
    [SerializeField] GameObject topHud;
    private bool _menuOpen;
    private void Awake()
    {
        inputSo.OnOpenMenu += () => mainHudButtons.SetHudButtonsInteractability(true);
        inputSo.OnOpenMenu += () => mainHudButtons.SetupNavigationWrapHorizontal(mainHudButtons.buttonToMenuPairs);;
        inputSo.OnOpenMenu += () => mainHudButtons.activePanel?.CloseMenu();
        inputSo.OnOpenMenu += ShowMenu;
    }

    public void HandleDialogue(string[] dialogue)
    {
        chatBubble.SendDialogue(dialogue);
        
    }

    void ShowMenu()
    {
        uiSo.OnCameraTransition?.Invoke(_menuOpen);
        if (_menuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    void OpenMenu()
    {
        inputSo.SwitchToUIInput();
        mainHudButtons.Show();
        _menuOpen = true;
        menuContentParent.SetActive(true);
        topHud.SetActive(true);
    }

    void CloseMenu()
    {
        inputSo.SwitchToPlayerInput();
        mainHudButtons.Hide();
        _menuOpen = false;
        menuContentParent.SetActive(false);
        topHud.SetActive(false);
    }


    public void RoomSelectPanel(Room[] rooms, Action<Room> roomSelected, string cancelText, string optionMessage)
    {
        inputSo.SwitchToUIInput();
        mainHudButtons.SetHudButtonsInteractability(false);
        roomSelected += room => mainHudButtons.SetHudButtonsInteractability(true); 
        roomSelectionPanel.PopWindow(rooms, roomSelected, cancelText);
        chatBubble.SendDialogue(new string[] {optionMessage});
        
    }
}
