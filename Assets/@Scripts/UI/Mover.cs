using System.Collections;
using TMPro;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Vector3 initialPosition;
    private Coroutine moveRoutine;
    public Vector3 moveVector;
    //public float moveDistance = 50f; // How far down to move
    public float moveDuration = 0.2f; // Time to move up/down
    public float holdTime = 1f; // Time to stay down

    void Start()
    {
        initialPosition = transform.position; // Store initial position
    }
    
    
    // public void DoTransition(string areaName)
    // {
    //     if (moveRoutine != null)
    //     {
    //         StopCoroutine(moveRoutine); // Stop ongoing animation
    //         transform.position = initialPosition; // Reset instantly
    //     }
    //
    //     moveRoutine = StartCoroutine(MoveRoutine(areaName));
    // }

    [ContextMenu("MoveTo")]
    public void MoveTo()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine); // Stop ongoing animation
            transform.position = initialPosition; // Reset instantly
        }
        
        Vector3 targetPosition = initialPosition + moveVector;
        StartCoroutine(MoveTo(targetPosition, moveDuration));
    }
    [ContextMenu("MoveFrom")]
    public void MoveFrom()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine); // Stop ongoing animation
            transform.position = initialPosition; // Reset instantly
        }
        
        StartCoroutine(MoveTo(initialPosition, moveDuration));
    }

    // private IEnumerator MoveRoutine(string areaName)
    // {
    //     GetComponentInChildren<TextMeshProUGUI>().text = areaName;
    //     Vector3 targetPosition = initialPosition + moveVector;
    //
    //     // Move to
    //     yield return MoveTo(targetPosition, moveDuration);
    //
    //     // Hold
    //     yield return new WaitForSeconds(holdTime);
    //
    //     // Move from
    //     yield return MoveTo(initialPosition, moveDuration);
    // }

    private IEnumerator MoveTo(Vector3 target, float duration)
    {
        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // Ensure exact position at the end
    }
}