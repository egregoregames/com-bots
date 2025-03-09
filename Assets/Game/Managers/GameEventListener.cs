using System;
using UnityEngine;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEventRelay gameEventRelay;
    [SerializeField] private PlayerData playerData;
    private void Awake()
    {
        gameEventRelay.ConnectionMade += OnConnectionMade;
    }

    void OnConnectionMade(NpcSo npcSo)
    {

        if (!playerData.KnownConnections.Contains(npcSo))
        {
            Debug.Log($"Connection made with {npcSo.name}");
            playerData.KnownConnections.Add(npcSo);
        }
    }
}
