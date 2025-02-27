using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NpcMovementRotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        // Get movement direction
        Vector3 direction = GetComponent<NavMeshAgent>().velocity.normalized;

        
        // Rotate only if moving
        if (direction.sqrMagnitude > 0.01f)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 100f);
        }
    }
}
