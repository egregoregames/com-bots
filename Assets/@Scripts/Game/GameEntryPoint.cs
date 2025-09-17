using System.Collections;
using ComBots.Game.Players;
using ComBots.Game.StateMachine;
using ComBots.Global;
using ComBots.Inputs;
using UnityEngine;

namespace ComBots.Game
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerSpawner _playerSpawner;

        [Header("Game")]
        [SerializeField] private GameStateMachine _gameStateMachine;

        IEnumerator Start()
        {
            // Wait for Global entry point to initialize
            while (!GlobalEntryPoint.IsInitialized)
            {
                yield return null;
            }
            // Spawn Player
            if(_playerSpawner.gameObject.activeSelf) _playerSpawner.TryInit();
            int playerInitAttempts = 0;
            while (Player.I == null && playerInitAttempts < 100)
            {
                yield return null;
                playerInitAttempts++;
            }
            // Game State Machine
            _gameStateMachine.TryInit();
        }
    }
}