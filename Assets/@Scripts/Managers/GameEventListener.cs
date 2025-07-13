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

    void OnConnectionMade(dataHolderSocialyteProfile connection)
    {

        if (!playerData.KnownConnections.Contains(connection))
        {
            Debug.Log($"Connection made with {connection.name}");
            playerData.KnownConnections.Add(connection);
        }
    }
}
