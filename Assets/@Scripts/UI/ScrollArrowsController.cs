using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src
{
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
            RectTransform viewport = scrollRect.viewport;
            RectTransform content = scrollRect.content;

            // Get viewport bounds in local space
            Vector3[] viewportCorners = new Vector3[4];
            viewport.GetWorldCorners(viewportCorners);
            float viewportTop = viewportCorners[1].y;
            float viewportBottom = viewportCorners[0].y;

            bool showArrowUp = false;
            bool showArrowDown = false;

            // Loop through all active buttons
            foreach (Button button in content.GetComponentsInChildren<Button>(true))
            {
                if (!button.gameObject.activeInHierarchy || !button.interactable)
                    continue;

                RectTransform buttonRect = button.GetComponent<RectTransform>();

                // Get button corners in world space
                Vector3[] buttonCorners = new Vector3[4];
                buttonRect.GetWorldCorners(buttonCorners);

                float buttonTop = buttonCorners[1].y;
                float buttonBottom = buttonCorners[0].y;

                if (buttonTop > viewportTop + 1f)
                    showArrowUp = true;
                if (buttonBottom < viewportBottom - 1f)
                    showArrowDown = true;

                // Early exit if both arrows need to be shown
                if (showArrowUp && showArrowDown)
                    break;
            }

            arrowUp.SetActive(showArrowUp);
            arrowDown.SetActive(showArrowDown);
        }
    }
}