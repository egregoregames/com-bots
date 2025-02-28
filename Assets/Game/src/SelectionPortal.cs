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
        uiSo.OnSelectionPortal?.Invoke(rooms, OnRoomSelected, cancelText);
    }
    void OnRoomSelected(int roomIndex)
    {
        OnTriggerPortal(roomIndex);
    }

    private void OnTriggerPortal(int roomIndex)
    {
        if(roomIndex == rooms.Length)
            return;
        
        _roomSelected = rooms[roomIndex];

        player.GetComponent<ThirdPersonController>().enabled = false;
        uiSo.AreaSelected?.Invoke(TeleportPlayerToRoom, ReleasePlayerMovement, _roomSelected.name);
    }

    void ReleasePlayerMovement()
    {
        player.GetComponent<ThirdPersonController>().enabled = true;
    }
    

    private void TeleportPlayerToRoom()
    {
        var portal = _roomSelected.Portal;
        
        portal.player = player;

        portal.SpawnPlayerAtPortal();
        
        uiSo.SoundSelected?.Invoke(_roomSelected.clip);
    }
}