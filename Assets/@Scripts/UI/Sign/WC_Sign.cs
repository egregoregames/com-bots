using ComBots.Game.StateMachine;
using ComBots.Global.Audio;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Sign
{
    public class WC_Sign : UIController
    {
        // =============== UIController Implementation =============== //
        public override Dependency Dependency => Dependency.Independent;
        protected override string UserInterfaceName => "Sign";
        // =============== Widget =============== //
        [Header("Widget")]
        [SerializeField] private GameObject _widget;
        // =============== UI =============== //
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _text;
        // =============== End Icon =============== //
        [Header("End Icon")]
        [SerializeField] private Transform _endIcon;
        [SerializeField] private float _iconAnimDuration;
        [SerializeField] private float _endIcon_scaleAmount;
        [SerializeField] private float _iconAppearDelay = 0.5f;
        // =============== Sound Effects =============== //
        [Header("Sound Effects")]
        [SerializeField] private AudioClip _sfx_read;
        [SerializeField] private AudioClip _sfx_end;
        
        // =============== Animation Settings =============== //
        [Header("Animation Settings")]
        [SerializeField] private float _widgetExpandDuration = 0.3f;
        [SerializeField] private float _widgetCollapseDuration = 0.2f;
        // =============== Cache =============== //
        private float _setActiveTime;

        #region Unity Lifecycle
        // ----------------------------------------
        // Unity Lifecycle 
        // ----------------------------------------

        void Awake()
        {
            // Initially hide the end icon and widget
            _endIcon.gameObject.SetActive(false);
            _widget.SetActive(false);
            
            // Set initial scale for widget (collapsed horizontally)
            _widget.transform.localScale = new Vector3(0f, 1f, 1f);
        }

        #endregion

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public void SetActive(string signText)
        {
            // Kill any ongoing animations and delayed calls first
            _widget.transform.DOKill();
            _endIcon.DOKill();
            DOTween.Kill(this);
            
            AudioManager.I.PlaySFX(_sfx_read);
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
            
            _setActiveTime = Time.time;
        }

        public void SetInactive()
        {
            // Sound
            AudioManager.I.PlaySFX(_sfx_end);
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
                });
        }

        #endregion

        #region UIController Implementation
        // ----------------------------------------
        // UIController Implementation
        // ----------------------------------------

        public override void Dispose()
        {
            // Kill any ongoing DOTween animations to prevent memory leaks
            _widget.transform.DOKill();
            _endIcon.DOKill();
            DOTween.Kill(this);
        }

        protected override void Init()
        {

        }

        #endregion

        #region Input Management
        // ----------------------------------------
        // Input Management 
        // ----------------------------------------

        public void Input_Dismiss()
        {
            // Check if enough time has passed since the sign has been open
            if(Time.time < _setActiveTime + _iconAppearDelay + _iconAnimDuration){
                return;
            }
            GameStateMachine.I.ExitState<GameStateMachine.State_Sign>();
        }

        #endregion
    }
}
