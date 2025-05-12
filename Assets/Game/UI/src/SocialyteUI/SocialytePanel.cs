using System;
using TMPro;
using UnityEngine;

public class SocialytePanel : MenuPanel
{
    [SerializeField] PlayerData _playerData;
    public NpcSo connection;
    
    private void Awake()
    {
        foreach (var connection in _playerData.KnownConnections)        
        {
            
        }

        for (int i = 0; i < _playerData.KnownConnections.Count; i++)
        {
            var connection = _playerData.KnownConnections[i];
            var button = categoryButtons[i];
            
            //button.gameObject.SetActive(true);
            this.connection = connection;
        }
    }
}
