using R3;
using System;
using System.Collections;
using UnityEngine;

public class StageDoorTransitions : MonoBehaviourR3
{
    public static StageDoorTransitions Instance { get; private set; }
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public float transitionTime = 0.5f;
    public UISo uiSo;
    public float delayTime = 0.5f;
    private Vector2 leftStartPos, rightStartPos;
    private Vector2 leftClosePos, rightClosePos;

    private bool _isClosed = false;
    private bool _isOpen = true;

    private new void Awake()
    {
        base.Awake();
        leftStartPos = leftPanel.anchoredPosition;
        rightStartPos = rightPanel.anchoredPosition;
        leftClosePos = Vector2.zero;
        rightClosePos = Vector2.zero;
    }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = this;
        uiSo.TriggerAreaChangeTransition += DoTransition;
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        uiSo.TriggerAreaChangeTransition -= DoTransition;
    }

    public void DoTransition(Action onTransitionMidPoint, Action onTransitionEnd, string bannerLabel)
    {
        StartCoroutine(Async_Transition(onTransitionMidPoint, onTransitionEnd, bannerLabel));
    }

    private IEnumerator Async_Transition(Action onTransitionMidPoint, Action onTransitionEnd, string bannerLabel)
    {
        CloseTransition();

        yield return new WaitUntil(() => _isClosed);

        onTransitionMidPoint?.Invoke();

        yield return new WaitForSeconds(delayTime);

        OpenTransition();

        yield return new WaitUntil(() => _isOpen);

        if (!string.IsNullOrEmpty(bannerLabel))
        {
            AreaDisplayPanels.Instance.DoTransition(bannerLabel);
        }
        
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
        LeanTween.move(leftPanel, leftClosePos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightClosePos, transitionTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(SetClosed);
    }

    [ContextMenu("Open")]
    public void OpenTransition()
    {
        LeanTween.move(leftPanel, leftStartPos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightStartPos, transitionTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(SetOpen);
    }
}
