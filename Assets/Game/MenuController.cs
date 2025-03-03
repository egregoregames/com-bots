using System;
using Game.src;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [SerializeField] private UISo uiSo;
    [SerializeField] private InputSO inputSo;
    [SerializeField] NewRoomSelectionPanel roomSelectionPanel;
    [SerializeField] MenuSelector _menuSelector;
    [SerializeField] GameObject menuContentParent;
    [SerializeField] ChatBubble chatBubble;
    private bool _menuOpen;
    private void Awake()
    {
        inputSo.OnOpenMenu += ShowMenu;
    }

    public void HandleDialogue(string[] dialogue)
    {
        chatBubble.SendDialogue(dialogue);
        
    }

    public void ShowMenu()
    {
        if (_menuOpen)
        {
            inputSo.SwitchToPlayerInput();
            _menuSelector.Hide();
            _menuOpen = false;
            menuContentParent.SetActive(false);
        }
        else
        {
            inputSo.SwitchToUIInput();
            _menuSelector.Show();
            _menuOpen = true;
            menuContentParent.SetActive(true);

        }
    }

    
    public void RoomSelectPanel(Room[] rooms, Action<Room> roomSelected, string cancelText)
    {
        inputSo.SwitchToUIInput();
        _menuSelector.SetButtonsInteractable(false);
        roomSelected += room => _menuSelector.SetButtonsInteractable(true); 
        roomSelectionPanel.PopWindow(rooms, roomSelected, cancelText);
        chatBubble.SendDialogue(new string[] {"What class do you want?"});
        
    }
}
