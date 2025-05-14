using UnityEngine;
using UnityEngine.UI;

public class ScrollArrowsController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject arrowUp;
    public GameObject arrowDown;

    void Update()
    {
        UpdateArrows();
    }

    void UpdateArrows()
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        float contentTop = content.anchoredPosition.y;
        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        // Clamp due to potential floating point precision issues
        contentTop = Mathf.Clamp(contentTop, 0, Mathf.Infinity);

        // Check if there's content above (can scroll up)
        bool showArrowUp = contentTop > 1f;

        // Check if there's content below (can scroll down)
        bool showArrowDown = contentTop + viewportHeight < contentHeight - 1f;

        arrowUp.SetActive(showArrowUp);
        arrowDown.SetActive(showArrowDown);
    }
}