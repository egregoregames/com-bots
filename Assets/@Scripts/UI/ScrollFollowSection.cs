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

    List<GameObject> _contentTabs;
    GameObject _lastSelected;
    private void Awake()
    {
        parentRect = GetComponent<RectTransform>();
        _contentTabs = contentPanel.GetComponentsInChildren<MenuTab>().Select(m => m.gameObject).ToList();
    }

    public float scrollStep = 0.225f; // Adjust this value to control scroll speed
    public float topEdgeThreshold = 0.8f;    // Scroll up when near top
    public float bottomEdgeThreshold = 0.2f; // Scroll down when near bottom

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

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null || selected == _lastSelected)
            return;

        _lastSelected = selected;

        var tab = selected.GetComponent<MenuTab>();
        if (tab == null)
            return;

        childRect = tab.GetComponent<RectTransform>();
        float normalizedPosition = GetNormalizedPosition(childRect, parentRect);

        // Trigger scroll slightly earlier — when the item is second-from-edge

        if (normalizedPosition < bottomEdgeThreshold)
        {
            ScrollDown();
        }
        else if (normalizedPosition > topEdgeThreshold)
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
}