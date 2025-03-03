using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTab : Button
{
    public Action onSelect;
    
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    
    protected override void Start()
    {
        base.Start();
        rectTransform = transform.GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Store initial UI position
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        var offset = transform.parent.GetComponent<RectTransform>().rect.width * .12f;
        
        rectTransform.anchoredPosition += new Vector2(offset, 0);
        onSelect?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        rectTransform.anchoredPosition = initialPosition;
    }

    
}
