using UnityEngine;

namespace ComBots
{
    public interface IInteractable 
    {
        public void Interact(GameObject interactor);
        public void OnHoverStay();
        public void OnHoverEnter();
        public void OnHoverExit();

    }
}
