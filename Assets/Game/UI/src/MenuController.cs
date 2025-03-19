using System;
using Game.src;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour
{
    [SerializeField] private UISo uiSo;
    [SerializeField] private InputSO inputSo;
    [SerializeField] NewRoomSelectionPanel roomSelectionPanel;
    [FormerlySerializedAs("_menuSelector")] [SerializeField] MainHudButtons mainHudButtons;
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
            mainHudButtons.Hide();
            _menuOpen = false;
            menuContentParent.SetActive(false);
        }
        else
        {
            inputSo.SwitchToUIInput();
            mainHudButtons.Show();
            _menuOpen = true;
            menuContentParent.SetActive(true);

        }
    }

    
    public void RoomSelectPanel(Room[] rooms, Action<Room> roomSelected, string cancelText)
    {
        inputSo.SwitchToUIInput();
        mainHudButtons.SetHudButtonsInteractability(false);
        roomSelected += room => mainHudButtons.SetHudButtonsInteractability(true); 
        roomSelectionPanel.PopWindow(rooms, roomSelected, cancelText);
        chatBubble.SendDialogue(new string[] {"What class do you want?"});
        
    }
}
