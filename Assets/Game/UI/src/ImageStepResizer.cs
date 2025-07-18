using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(CanvasGroup))]
    public class ImageStepResizer : MonoBehaviour
    {
        [SerializeField] private float animationStepDelay = 0.1f;
        [SerializeField] private int animationSteps = 5;
        [SerializeField] private float shrinkScale = 0.6f;
        [SerializeField] private float alphaThreshold = 0.99f; // Allow some float tolerance

        RectTransform rect;
        Vector3 originalScale;
        CanvasGroup canvasGroup;
        Button button;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            button = GetComponent<Button>();
            originalScale = rect.localScale;
        }

        void OnEnable()
        {
            StartCoroutine(LoopAnimation());
            StartCoroutine(WatchCanvasAlpha());
        }

        void OnDisable()
        {
            StopAllCoroutines();
            rect.localScale = originalScale;
        }

        IEnumerator LoopAnimation()
        {
            while (true)
            {
                // Shrink steps
                for (int i = 0; i < animationSteps; i++)
                {
                    float t = 1 - (1 - shrinkScale) * ((float)(i + 1) / animationSteps);
                    rect.localScale = new Vector3(t, t, 1);
                    yield return new WaitForSeconds(animationStepDelay);
                }

                // Expand steps
                for (int i = animationSteps - 1; i >= 0; i--)
                {
                    float t = 1 - (1 - shrinkScale) * ((float)(i) / animationSteps);
                    rect.localScale = new Vector3(t, t, 1);
                    yield return new WaitForSeconds(animationStepDelay);
                }
            }
        }

        IEnumerator WatchCanvasAlpha()
        {
            while (true)
            {
                button.interactable = canvasGroup.alpha >= alphaThreshold;
                yield return null; // check every frame
            }
        }
    }
}