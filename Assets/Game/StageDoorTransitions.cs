using System;
using System.Collections;
using UnityEngine;

public class StageDoorTransitions : MonoBehaviour
{
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public float transitionTime = 0.5f;
    public UISo uiSo;
    public float delayTime = 0.5f;
    
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

    public void DoStageDoorTransition(Action callback, string name)
    {
        StartCoroutine(TransitionCoRo(callback));
    }

    private bool _isClosed = false;
    IEnumerator TransitionCoRo(Action teleportPlayerCallback = null)
    {
        CloseTransition();
        
        yield return new WaitUntil(() => _isClosed);
        teleportPlayerCallback?.Invoke();
        yield return new WaitForSeconds(delayTime);
        
        OpenTransition();
        _isClosed = false;
    }

    public void CloseTransition()
    {
        LeanTween.move(leftPanel, leftClosePos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightClosePos, transitionTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() => _isClosed = true);
    }

    public void OpenTransition()
    {
        LeanTween.move(leftPanel, leftStartPos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(rightPanel, rightStartPos, transitionTime).setEase(LeanTweenType.easeInOutQuad);
    }
}
