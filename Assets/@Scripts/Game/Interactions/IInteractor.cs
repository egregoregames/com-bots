using UnityEngine;

namespace ComBots.Game.Interactions
{
    public interface IInteractor
    {
        public Transform T { get; }

        public enum InteractorState
        {
            Idle,
            Interacting
        }
        public InteractorState State { get; }

        public void OnInteractionStart(IInteractable interactable);
        public void OnInteractionEnd(IInteractable interactable);
    }
}