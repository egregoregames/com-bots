using UnityEngine;

namespace ComBots.Game.Interactions
{
    public interface IInteractable
    {
        public Transform T { get; }
        public void OnInteractionStart(IInteractor interactor);
        public void OnInteractionEnd(IInteractor interactor);
        public void OnInteractorNearby(IInteractor interactor);
        public void OnInteractorFar(IInteractor interactor);
    }
}