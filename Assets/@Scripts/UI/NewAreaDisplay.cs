using System;
using System.Collections;
using ComBots.src;
using UnityEngine;
using UnityEngine.Serialization;

public class NewAreaDisplay : MonoBehaviour
{
    [FormerlySerializedAs("leftPanel")] public RectTransform display;
    public RectTransform rightPanel;
    public float transitionTime = 0.5f;
    public UISo uiSo;
    public float delayTime = 0.5f;
    public AreaDisplayPanels AreaDisplayPanels;
    private Vector2 startPos, rightStartPos;
    private Vector2 endPos, rightClosePos;

    private void Awake()
    {
       // uiSo.TriggerAreaChangeTransition += DoStageDoorTransition;
    }

    void Start()
    {
        // Store initial positions
        startPos = display.anchoredPosition;

        // Compute closed positions (panels meet at center)
        endPos = new Vector2(0, 0);
    }

    public void DoStageDoorTransition(Action onTransitionMidPoint, Action onTransitionEnd, string areaName)
    {
        StartCoroutine(TransitionCoRo(onTransitionMidPoint, onTransitionEnd, areaName));
    }

    private bool _isClosed = false;
    private bool _isOpen = true;
    IEnumerator TransitionCoRo(Action onTransitionMidPoint, Action onTransitionEnd, string areaName)
    {
        CloseTransition();
        
        yield return new WaitUntil(() => _isClosed);

        onTransitionMidPoint?.Invoke();
        
        yield return new WaitForSeconds(delayTime);
        
        OpenTransition();
        
        yield return new WaitUntil(() => _isOpen);
        
        AreaDisplayPanels.DoTransition(areaName);

        onTransitionEnd?.Invoke();

    }

    void SetClosed()
    {
        _isClosed = true;
        _isOpen = false;
    }

    void SetOpen()
    {
        _isClosed = false;
        _isOpen = true;
    }

    [ContextMenu("Close")]
    public void CloseTransition()
    {
        LeanTween.move(display, endPos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightClosePos, transitionTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(SetClosed);
    }

    [ContextMenu("Open")]
    public void OpenTransition()
    {
        LeanTween.move(display, startPos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightStartPos, transitionTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(SetOpen);
    }
}
