using StarterAssets;
using UnityEngine;


public class SelectionPortal : Portal
{
    public SelectionPortal nextPortal;
    public UISo uiSo;
    public Room[] rooms;
    public string cancelText;
    
    Room _roomSelected;
    protected override void OnPlayerEnter(GameObject playerWhoEntered)
    {
        player = playerWhoEntered;
        
        uiSo.PlayerEnteredRoomSelector?.Invoke(rooms, OnRoomSelected, cancelText);
    }
    
    private void OnRoomSelected(Room room)
    {
        _roomSelected = room;
        player.GetComponent<ThirdPersonController>().enabled = false;
        
        uiSo.TriggerAreaChangeTransition?.Invoke(TeleportPlayerToRoom, ReleasePlayerMovement, room.name);
    }

    void ReleasePlayerMovement()
    {
        
        player.GetComponent<ThirdPersonController>().AllowPlayerInput();

    }
    
    private void TeleportPlayerToRoom()
    {
        _roomSelected.TeleportPlayerToRoom(player);
        
        uiSo.SoundSelected?.Invoke(_roomSelected.clip);
    }
}