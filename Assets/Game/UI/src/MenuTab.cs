using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTab : Button
{
    public Action onSelect;
    
    protected bool isSelected;

    RectTransform rectTransform;
    Vector2 initialPosition;

    public NpcSo connection;

    protected override void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Store initial UI position
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        SelectEffect();
    }

    public virtual void SelectEffect()
    {
        var offset = transform.parent.GetComponent<RectTransform>().rect.width * .12f;
        
        if(!rectTransform)
            return;
        
        rectTransform.anchoredPosition += new Vector2(offset, 0);
        onSelect?.Invoke();
    }

    

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        DeselectEffect();
    }

    public virtual void DeselectEffect()
    {
        rectTransform.anchoredPosition = initialPosition;
    }
    
    public virtual void HandleHorizontalInput(int direction)
    {
        
    }
}
