using DG.Tweening;
using R3;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SignUI : MonoBehaviourR3
{
    private static SignUI _instance;

    private static UnityEventR3 _onClosed = new();
    public static IDisposable OnClosed(Action x) => _onClosed.Subscribe(x);

    public static bool IsOpen => _instance._widget.activeInHierarchy;

    [SerializeField] private GameObject _widget;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("End Icon")]
    [SerializeField] private Transform _endIcon;
    [SerializeField] private float _iconAnimDuration;
    [SerializeField] private float _endIcon_scaleAmount;
    [SerializeField] private float _iconAppearDelay = 0.5f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _sfx_read;
    [SerializeField] private AudioClip _sfx_end;
        
    [Header("Animation Settings")]
    [SerializeField] private float _widgetExpandDuration = 0.3f;
    [SerializeField] private float _widgetCollapseDuration = 0.2f;

    private InputSystem_Actions Inputs { get; set; }

    #region Monobehaviour

    private new void Awake()
    {
        base.Awake();
        _endIcon.gameObject.SetActive(false);

        // Set initial scale for widget (collapsed horizontally)
        _widget.transform.localScale = new Vector3(0f, 1f, 1f);
        _widget.SetActive(false);
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        // Kill any ongoing DOTween animations to prevent memory leaks
        _widget.transform.DOKill();
        _endIcon.DOKill();
        DOTween.Kill(this);
    }

    protected override void Initialize()
    {
        base.Initialize();
        Inputs = new();
        _instance = this;
        var onInteract = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.Player.Interact.performed += h,
            h => Inputs.Player.Interact.performed -= h);

        AddEvents(
            Sign.OnSignActivated(SetActive),
            onInteract.Subscribe(x => SetInactive()));
    }

    private new void OnEnable()
    {
        base.OnEnable();
        Inputs.Enable();
    }

    private void OnDisable()
    {
        Inputs.Disable();
    }

    #endregion

    private void SetActive(string signText)
    {
        // Kill any ongoing animations and delayed calls first
        _widget.transform.DOKill();
        _endIcon.DOKill();
        DOTween.Kill(this);

        Debug.LogError("Fix this");
        //AudioManager.I.PlaySFX(_sfx_read);

        _text.text = signText;
            
        // Ensure widget is active and properly reset
        _widget.SetActive(true);
        _endIcon.gameObject.SetActive(false);
            
        // Reset scales to initial state
        _widget.transform.localScale = new Vector3(0f, 1f, 1f);
        _endIcon.localScale = Vector3.zero;
            
        // Animate widget expanding horizontally from the middle
        _widget.transform.DOScaleX(1f, _widgetExpandDuration)
            .SetEase(Ease.OutBack)
            .SetTarget(this);

        // Show and animate the end icon with delay
        DOVirtual.DelayedCall(_iconAppearDelay, () =>
        {
            if (_widget.activeInHierarchy) // Check if widget is still active
            {
                _endIcon.gameObject.SetActive(true);
                _endIcon.localScale = Vector3.zero;
                _endIcon.DOScale(Vector3.one, 0.2f)
                    .SetEase(Ease.OutBack)
                    .SetTarget(this)
                    .OnComplete(() =>
                    {
                        // Start the pulsing animation
                        _endIcon.DOScale(_endIcon.localScale * _endIcon_scaleAmount, _iconAnimDuration)
                            .SetEase(Ease.InOutSine)
                            .SetLoops(-1, LoopType.Yoyo)
                            .SetTarget(this);
                    });
            }
        }).SetTarget(this);
    }

    private void SetInactive()
    {
        if (!_widget.activeInHierarchy)
            return;

        // Sound

        Debug.LogError("Fix this");
        //AudioManager.I.PlaySFX(_sfx_end);

        // Stop any ongoing animations and delayed calls
        _widget.transform.DOKill();
        _endIcon.DOKill();
        DOTween.Kill(this);
            
        // Animate widget collapsing horizontally to the edges
        _widget.transform.DOScaleX(0f, _widgetCollapseDuration)
            .SetEase(Ease.InBack)
            .SetTarget(this)
            .OnComplete(() =>
            {
                _widget.SetActive(false);
                _endIcon.gameObject.SetActive(false);
                _onClosed?.Invoke();
            });
    }
}