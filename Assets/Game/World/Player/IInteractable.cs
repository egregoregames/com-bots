using UnityEngine;

namespace Game
{
    public interface IInteractable 
    {
        public void Interact(GameObject interactor);
        public void OnHoverStay();
        public void OnHoverEnter();
        public void OnHoverExit();

    }
}
