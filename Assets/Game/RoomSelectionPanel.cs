using System;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RoomSelectionPanel : MonoBehaviour
{
    [FormerlySerializedAs("roomsNameTab")] [SerializeField] List<GameObject> roomsNameSelectionObjects;
    [SerializeField] UISo uiSo;
    [SerializeField] InputSO inputSO;

    Action<int> onRoomSelected;
    Room[] _rooms;
    int _roomIndex = 0;
    private void Awake()
    {
        uiSo.OnSelectionPortal += PushRoomList;
        inputSO.OnUp += RoomSelectUp;
        inputSO.OnDown += RoomSelectDown;
        
        inputSO.OnSubmit += OnRoomSelected;
    }
    
    private void OnRoomSelected()
    {
        inputSO.SwitchToPlayerInput();
        ClearArrows();
        var panel = transform.GetChild(0).gameObject;
        panel.SetActive(false);
        onRoomSelected?.Invoke(_roomIndex);
        _roomIndex = 0;

    }


    private void OnDisable()
    {
        inputSO.OnUp -= RoomSelectUp;
        inputSO.OnDown -= RoomSelectDown;
    }
    private void OnDestroy()
    {
        inputSO.OnUp -= RoomSelectUp;
        inputSO.OnDown -= RoomSelectDown;
    }
    void RoomSelectUp()
    {
        if (_roomIndex == 0)
        {
            _roomIndex = _rooms.Length - 1;
        }
        else
        {
            _roomIndex--;
        }
        SetArrowActive();
    }
    void RoomSelectDown()
    {
        if (_roomIndex < _rooms.Length - 1)
        {
            _roomIndex++;
        }
        else
        {
            _roomIndex = 0;
        }
        SetArrowActive();
    }

    void PushRoomList(Room[] roomNames, Action<int> callback)
    {
        onRoomSelected = callback;
        inputSO.SwitchToUIInput();
        var panel = transform.GetChild(0).gameObject;
        panel.SetActive(true);
        
        SetObjectsActive(roomNames);
    }

    private void SetObjectsActive(Room[] rooms)
    {
        _rooms = rooms;
        var firstSelectionArrow = roomsNameSelectionObjects[0].transform.GetChild(1).gameObject;
        firstSelectionArrow.SetActive(true);
        for (int i = 0; i < _rooms.Length; i++)
        {
            var roomNameText = roomsNameSelectionObjects[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = rooms[i].name;
            var selectionArrow = roomsNameSelectionObjects[i].transform.GetChild(1).gameObject;

            roomsNameSelectionObjects[i].gameObject.SetActive(true);
        }
    }
    
    private void SetArrowActive()
    {
        for (int i = 0; i < _rooms.Length; i++)
        {
            var selectionArrow = roomsNameSelectionObjects[i].transform.GetChild(1).gameObject;

            if (i == _roomIndex)
            {
                selectionArrow.SetActive(true);
            }
            else
            {
                selectionArrow.SetActive(false);
            }
        }
    }

    void ClearArrows()
    {
        for (int i = 0; i < _rooms.Length; i++)
        {
            roomsNameSelectionObjects[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
