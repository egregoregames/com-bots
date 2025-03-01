using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private UISo uiSo;
    [SerializeField] private InputSO inputSo;
    [SerializeField] NewRoomSelectionPanel roomSelectionPanel;
    
    private void Awake()
    {
        //inputSo.OnOpenMenu +=
    }

    public void RoomSelectPanel(Room[] rooms, Action<Room> roomSelected, string cancelText)
    {
        inputSo.SwitchToUIInput();
        
        roomSelectionPanel.PopWindow(rooms, roomSelected, cancelText);
    }
}
