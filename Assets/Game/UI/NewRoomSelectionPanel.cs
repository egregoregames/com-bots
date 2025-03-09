using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewRoomSelectionPanel : MonoBehaviour
{
    [SerializeField] List<GameObject> selectionTabs;
    [SerializeField] GameObject tabsParent;
    [SerializeField] private InputSO _inputSo;

    public void PopWindow(Room[] rooms, Action<Room> roomSelected, string cancelText)
    {
        tabsParent.SetActive(true);
        
        for (int i = 0; i < rooms.Length; i++)
        {
            selectionTabs[i].SetActive(true);
            
            var room = rooms[i];
            var button = selectionTabs[i].GetComponentInChildren<Button>();
            
            button.onClick.AddListener( () => roomSelected.Invoke(room));
            button.onClick.AddListener( () => tabsParent.SetActive(false));

            var tmp = selectionTabs[i].GetComponentInChildren<TextMeshProUGUI>();
            
            tmp.text = rooms[i].name;
        }
        selectionTabs[rooms.Length].SetActive(true);
            
        var cancelTabButton = selectionTabs[rooms.Length].GetComponentInChildren<Button>();
            
        cancelTabButton.onClick.AddListener( () => tabsParent.SetActive(false));
        cancelTabButton.onClick.AddListener( () => _inputSo.SwitchToPlayerInput());

        var cancelTabTmp = selectionTabs[rooms.Length].GetComponentInChildren<TextMeshProUGUI>();
            
        cancelTabTmp.text = cancelText;
    }
}

