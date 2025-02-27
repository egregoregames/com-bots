using UnityEngine;


public class SelectionPortal : Portal
{
    public SelectionPortal nextPortal;
    public UISo uiSo;
    public Room[] rooms;

    Room _roomSelected;
    protected override void OnPlayerEnter(GameObject playerWhoEntered)
    {
        player = playerWhoEntered;
        uiSo.OnSelectionPortal?.Invoke(rooms, OnRoomSelected);
    }
    void OnRoomSelected(int roomIndex)
    {
        OnTriggerPortal(roomIndex);
    }

    private void OnTriggerPortal(int roomIndex)
    {
        _roomSelected = rooms[roomIndex];

        uiSo.AreaSelected?.Invoke(HandlePlayerChangeArea, _roomSelected.name);
    }

    private void HandlePlayerChangeArea()
    {
        var portal = _roomSelected.Portal;
        
        portal.player = player;
        
        portal.SpawnPlayerAtPortal();
        
        uiSo.SoundSelected?.Invoke(_roomSelected.clip);
    }
}