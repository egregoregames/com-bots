using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonScaler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Vector3 defaultScale;

    private void Start()
    {
        defaultScale = transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = defaultScale * 1.1f; // Slightly enlarge
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = defaultScale; // Reset scale
    }
}