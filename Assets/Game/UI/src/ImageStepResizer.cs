using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class ImageStepResizer : MonoBehaviour
{
    [SerializeField] private float animationStepDelay = 0.1f;
    [SerializeField] private int animationSteps = 5;
    [SerializeField] private float shrinkScale = 0.6f;

    private RectTransform rect;
    private Vector3 originalScale;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
    }

    private void OnEnable()
    {
        StartCoroutine(LoopAnimation());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rect.localScale = originalScale;
    }

    private IEnumerator LoopAnimation()
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
}