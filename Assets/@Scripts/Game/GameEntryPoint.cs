using System.Collections;
using ComBots.Game.Interactions;
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

        [Header("Interactions")]
        [SerializeField] private InteractionManager _interactionManager;

        IEnumerator Start()
        {
            // Interaction Manager
            _interactionManager.TryInit();
            // Game State Machine
            _gameStateMachine.TryInit();
            yield break;
        }
    }
}