using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private InputSO _inputSo;
        private IInteractable _currentInteractable;
        private Collider _currentCollider;

        private void OnEnable()
        {
            _inputSo.OnInteract += InteractWithNPC;
        }

        private void OnDisable()
        {
            _inputSo.OnInteract -= InteractWithNPC;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.CompareTag("Interactable")) return;

            _currentCollider = collider;
            _currentInteractable = collider.GetComponent<IInteractable>();
            _currentInteractable?.OnHoverEnter();
        }

        private void OnTriggerStay(Collider collider)
        {
            if (_currentCollider != collider || _currentInteractable == null) return;

            _currentInteractable.OnHoverStay();
        }

        private void OnTriggerExit(Collider collider)
        {
            if (_currentCollider != collider) return;

            _currentInteractable?.OnHoverExit();
            _currentInteractable = null;
            _currentCollider = null;
        }

        private void InteractWithNPC()
        {
            _currentInteractable?.Interact(gameObject);
        }
    }
}