namespace ComBots.Game.Interactions
{
    public interface IInteractable
    {
        public void Interact(IInteractor interactor);
        public void OnInteractorNearby(IInteractor interactor);
        public void OnInteractorFar(IInteractor interactor);
    }
}