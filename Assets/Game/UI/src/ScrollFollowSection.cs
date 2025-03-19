using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollFollowSelection : MonoBehaviour
{
    public ScrollRect scrollRect; // Assign your ScrollRect in Inspector
    public RectTransform contentPanel; // The panel holding the buttons
    public RectTransform parentRect;  // Assign the parent panel
    public RectTransform childRect;  
    
    private List<GameObject> contentTabs;
    private void Awake()
    {
        parentRect = GetComponent<RectTransform>();
        contentTabs = contentPanel.GetComponentsInChildren<MenuTab>().Select(m => m.gameObject).ToList();
    }

    public float scrollStep = 0.1f; // Adjust this value to control scroll speed

    [ContextMenu("Up")]
    public void ScrollUp()
    {
        scrollRect.verticalNormalizedPosition += scrollStep;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }
    [ContextMenu("Down")]
    public void ScrollDown()
    {
        scrollRect.verticalNormalizedPosition -= scrollStep;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }
    // void Update()
    // {
    //     GameObject selected = EventSystem.current.currentSelectedGameObject;
    //     if (selected != null && selected.transform.IsChildOf(contentPanel))
    //     {
    //         ScrollToSelected(selected.GetComponent<RectTransform>());
    //     }
    // }
    
    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        
        if(selected == null)
            return;
        var tab = selected.GetComponent<MenuTab>();
        if(tab == null)
            return;

        childRect = tab.GetComponent<RectTransform>();
        float normalizedPosition = GetNormalizedPosition(childRect, parentRect);

        if (normalizedPosition < 0.1f) // Example: Check if near the bottom
        {
            ScrollDown();
        }
        if (normalizedPosition > 0.9f) // Example: Check if near the bottom
        {
            ScrollUp();
        }
    }

    float GetNormalizedPosition(RectTransform child, RectTransform parent)
    {
        // Convert child's position to local space of parent
        Vector3 localPos = parent.InverseTransformPoint(child.position);

        // Get the parent's height
        float parentHeight = parent.rect.height;

        // Convert to normalized range (0 = bottom, 1 = top)
        return Mathf.InverseLerp(-parentHeight / 2, parentHeight / 2, localPos.y);
    }
    //
    // void ScrollToSelected(RectTransform selected)
    // {
    //     RectTransform viewport = scrollRect.viewport;
    //
    //     // Convert selected button's position to local space of viewport
    //     Vector3 localPos = viewport.InverseTransformPoint(selected.position);
    //     Vector3 viewportCenter = viewport.InverseTransformPoint(contentPanel.position);
    //
    //     // Check if the selected button is outside the visible area
    //     float offset = localPos.y - viewportCenter.y;
    //
    //     if (Mathf.Abs(offset) > viewport.rect.height / 2)
    //     {
    //         // Adjust scroll position to bring the selected button into view
    //         float normalizedPosition = Mathf.Clamp01(
    //             (contentPanel.rect.height / 2 - selected.localPosition.y) /
    //             (contentPanel.rect.height - viewport.rect.height)
    //         );
    //         scrollRect.verticalNormalizedPosition = normalizedPosition;
    //     }
    // }
}