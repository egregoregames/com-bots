using UnityEngine;

namespace ComBots.Interact
{
    public interface IInteractable 
    {
        public void Interact(GameObject interactor);
        public void OnHoverStay();
        public void OnHoverEnter();
        public void OnHoverExit();

    }
}
