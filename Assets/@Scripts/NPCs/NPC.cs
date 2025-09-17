using UnityEngine;
using ComBots.Game.Interactions;
using ComBots.Game.StateMachine;
using PixelCrushers.DialogueSystem;
using ComBots.Game.Players;
using ComBots.Logs;
using ComBots.Utils.ObjectPooling;
using ComBots.UI.OverheadWidgets;
using ComBots.Cameras;

namespace ComBots.NPCs
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueActor _dialogueActor;
        [SerializeField] private string _conversationTitle;

        [Header("Overhead Widget")]
        [SerializeField] private Vector3 _overheadWidgetOffset;

        [Header("Cameras")]
        public CameraTarget CameraTarget;

        private const string overheadWidgetKey = "NPC_Overhead_Widget";
        private OverheadWidget _overheadWidget;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + _overheadWidgetOffset, 0.1f);
        }

        public void Interact(IInteractor interactor)
        {
            if (interactor is not Player player)
            {
                MyLogger<NPC>.StaticLogWarning($"Only players can interact with NPCs. {interactor} is not a player.");
                return;
            }
            // Remove overhead widget
            if (_overheadWidget)
            {
                PoolManager.I.Push(overheadWidgetKey, _overheadWidget);
                _overheadWidget = null;
            }
            // Start dialogue
            State_Dialogue_PixelCrushers_Args args = new(_conversationTitle, _dialogueActor, player.DialogueActor, CameraTarget);
            GameStateMachine.I.SetState<GameStateMachine.State_Dialogue>(args);
        }

        public void OnInteractorFar(IInteractor interactor)
        {
            if (_overheadWidget)
            {
                PoolManager.I.Push(overheadWidgetKey, _overheadWidget);
                _overheadWidget = null;
            }
        }

        public void OnInteractorNearby(IInteractor interactor)
        {
            if (_overheadWidget == null)
            {
                _overheadWidget = PoolManager.I.Pull<OverheadWidget>(overheadWidgetKey);
                _overheadWidget.transform.position = transform.position + _overheadWidgetOffset;
            }
        }
    }
}