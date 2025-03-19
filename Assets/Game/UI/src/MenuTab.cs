using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTab : Button
{
    public Action onSelect;
    
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    public NpcSo connection;
    
    protected override void Start()
    {
        base.Start();
        rectTransform = transform.GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition; // Store initial UI position
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        SelectEffect();
    }

    protected virtual void SelectEffect()
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

    public void DeselectEffect()
    {
        rectTransform.anchoredPosition = initialPosition;
    }
}
