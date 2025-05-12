using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuTab : Button
{
    protected RectTransform rectTransform;
    protected Vector2 initialPosition;
    
    public Action onSelect;
    public bool isSelected;

    protected override void Awake()
    {
        base.Awake();
        rectTransform = transform.GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Store initial UI position
    }

    public virtual void SelectEffect()
    {
        var offset = transform.parent.GetComponent<RectTransform>().rect.width * .07f;
        
        if(!rectTransform || !rectTransform.anchoredPosition.Equals(initialPosition)) return;
        
        rectTransform.anchoredPosition += new Vector2(offset, 0);
        onSelect?.Invoke();
    }
    
    public virtual void DeselectEffect()
    {
        rectTransform.anchoredPosition = initialPosition;
    }
    
    public virtual void HandleHorizontalInput(int direction)
    {
        
    }

    public virtual void HandleVerticalInput(int direction)
    {
        
    }
}
