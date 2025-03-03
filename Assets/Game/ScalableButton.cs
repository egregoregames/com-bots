using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScalableButton : Button
{
    public Action onSelect;
    
    float scaleFactor = 1.5f; // How much to scale (e.g., 1.2 = 120% size)
    float scaleSpeed = 10f;   // Speed of scaling

    private Vector3 originalScale;
    private Vector3 targetScale;
    private float t = 0; // Time tracker
    private bool isScaling = false;

    
    protected override void Start()
    {
        base.Start();
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        targetScale = originalScale * scaleFactor;
        t = 0; // Reset transition time
        isScaling = true;
        onSelect?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        targetScale = originalScale;
        t = 0; // Reset transition time
        isScaling = true;
    }

    private void Update()
    {
        if (isScaling)
        {
            t += Time.deltaTime * scaleSpeed; // Increment transition time
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);

            // Stop updating when close to target scale
            if (Vector3.Distance(transform.localScale, targetScale) < 0.001f)
            {
                transform.localScale = targetScale;
                isScaling = false;
            }
        }
    }
}