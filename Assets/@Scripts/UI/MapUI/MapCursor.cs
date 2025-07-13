using UnityEngine;

public class MapCursor : MonoBehaviour
{
    [SerializeField] InputSO inputSO;
    [SerializeField] RectTransform mapBounds; // The parent map image
    [SerializeField] float moveSpeed = 300f;   // Pixels per second

    RectTransform _cursorRect;

    void Awake()
    {
        _cursorRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 direction = inputSO.navigate;

        if (direction != Vector2.zero)
        {
            MoveCursor(direction.normalized);
        }
    }

    void MoveCursor(Vector2 direction)
    {
        Vector2 newPosition = _cursorRect.anchoredPosition + direction * (moveSpeed * Time.deltaTime);

        // Clamp based on the edges of the cursor, not its center
        Vector2 halfSize = _cursorRect.rect.size * 0.5f;
        Vector2 minBounds = mapBounds.rect.min + halfSize;
        Vector2 maxBounds = mapBounds.rect.max - halfSize;

        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y)
        );

        _cursorRect.anchoredPosition = clampedPosition;
    }
}