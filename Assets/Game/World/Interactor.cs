using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private InputSO _inputSo;
        //todo cache the interactable to prevent update GetComponent
        private void OnTriggerStay(Collider collider)
        {
            if (!collider.CompareTag("Interactable"))
                return;
            collider.GetComponent<IInteractable>().OnHoverStay();
            if (_inputSo.interact)
            {
                collider.GetComponent<IInteractable>().Interact(gameObject);
            }
        }
        private void OnTriggerExit(Collider collider)
        {
            if (!collider.CompareTag("Interactable"))
                return;
            collider.GetComponent<IInteractable>().OnHoverExit();
        }
        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.CompareTag("Interactable"))
                return;
            collider.GetComponent<IInteractable>().OnHoverEnter();
        }

        public void OnInteract(InputValue value)
        {
            Debug.Log("Interact");
        }
        // void Update()
        // {
        //     // Define the ray
        //     Ray ray = new Ray(transform.position, transform.forward);
        //
        //     // Define the layer mask
        //     int layerMask = LayerMask.GetMask("Interactable");
        //
        //     // Perform the raycast
        //     if (Physics.Raycast(ray, out RaycastHit hit, 1f, layerMask))
        //     {
        //         Debug.Log("Hit: " + hit.collider.name);
        //     }
        //
        //     // Visualize the ray in the scene view
        //     Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red);
        // }
    }
}
