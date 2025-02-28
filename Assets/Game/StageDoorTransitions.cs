using System;
using System.Collections;
using Game.src;
using UnityEngine;

public class StageDoorTransitions : MonoBehaviour
{
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public float transitionTime = 0.5f;
    public UISo uiSo;
    public float delayTime = 0.5f;
    public AreaDisplayPanels AreaDisplayPanels;
    private Vector2 leftStartPos, rightStartPos;
    private Vector2 leftClosePos, rightClosePos;

    private void Awake()
    {
        uiSo.AreaSelected += DoStageDoorTransition;
    }

    void Start()
    {
        // Store initial positions
        leftStartPos = leftPanel.anchoredPosition;
        rightStartPos = rightPanel.anchoredPosition;

        // Compute closed positions (panels meet at center)
        float screenWidth = Screen.width;
        leftClosePos = new Vector2(-480, 0);
        rightClosePos = new Vector2(480, 0);
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

    public void CloseTransition()
    {
        LeanTween.move(leftPanel, leftClosePos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightClosePos, transitionTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(SetClosed);
    }

    public void OpenTransition()
    {
        LeanTween.move(leftPanel, leftStartPos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightStartPos, transitionTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(SetOpen);
    }
}
