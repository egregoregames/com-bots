using System.Collections;
using TMPro;
using UnityEngine;

public class AreaDisplayPanels : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private Coroutine moveRoutine;
    //public Vector2 moveVector; // How far to move in UI space
    public float moveDuration = 0.2f; // Time to move
    public float holdTime = 1f; // Time to stay visible
    private string _currentAreaName;

    void Start()
    {
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Store initial UI position
    }

    [ContextMenu("Move")]
    public void TestMove()
    {
        DoTransition("");
    }

    public void DoTransition(string areaName)
    {
        if(_currentAreaName == areaName)
            return; // No need to do anything if the area name hasn't changed

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine); // Stop ongoing animation
            rectTransform.anchoredPosition = initialPosition; // Reset instantly
        }

        moveRoutine = StartCoroutine(MoveRoutine(areaName));
    }

    private IEnumerator MoveRoutine(string areaName)
    {
        _currentAreaName = areaName;
        GetComponentInChildren<TextMeshProUGUI>().text = areaName;
        Vector2 targetPosition = new (0, -180);

        // Move down
        yield return MoveToPosition(targetPosition, moveDuration);

        // Hold
        yield return new WaitForSeconds(holdTime);

        // Move back up
        yield return MoveToPosition(initialPosition, moveDuration);
    }

    private IEnumerator MoveToPosition(Vector2 target, float duration)
    {
        float elapsed = 0f;
        Vector2 start = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = target; // Ensure exact position at the end
    }
}