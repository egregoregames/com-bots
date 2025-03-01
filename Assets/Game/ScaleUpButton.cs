using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleUpButton : Button
{
    private Vector3 defaultScale;

    protected override void Start()
    {
        base.Start();
        defaultScale = transform.localScale;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        transform.localScale = defaultScale * 1.1f; // Slightly enlarge
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        transform.localScale = defaultScale; // Reset scale
    }
}
