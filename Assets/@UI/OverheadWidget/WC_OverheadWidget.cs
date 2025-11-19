using ComBots.Game.Players;
using ComBots.Utils.EntryPoints;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ComBots.UI.OverheadWidgets
{
    /// <summary>
    /// Overhead widget that appears above interactable objects (NPCs, signs).
    /// Features bounce animations on show/hide and camera-facing behavior.
    /// </summary>
    public class WC_OverheadWidget : Controllers.UIController
    {
        // =============== UIController Implementation =============== //
        public override Dependency Dependency => Dependency.Independent;
        protected override string UserInterfaceName => "Overhead Widget";

        // =============== Overhead Widget Type =============== //
        [Header("Overhead Widget Type")]
        [SerializeField] private OverheadWidgetType _widgetType;
        public OverheadWidgetType WidgetType => _widgetType;

        // =============== UI =============== //
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI labelText;

        // =============== Animation Settings =============== //
        [Header("Animation Settings")]
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _bounceScale = 1.2f;

        // =============== Tween References =============== //
        private Tween _scaleTween;
        private Vector3 _originalScale;

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public void SetActive(string label)
        {
            if (labelText)
            {
                labelText.text = label;
            }
        }

        #endregion

        #region MonoBehaviour Lifecycle
        // ----------------------------------------
        // MonoBehaviour Lifecycle
        // ----------------------------------------

        private void OnEnable()
        {
            // Store original scale on first enable
            if (_originalScale == Vector3.zero)
            {
                _originalScale = transform.localScale;
            }

            // Play appear animation
            PlayAppearAnimation();
        }

        private void OnDisable()
        {
            // Clean up tweens when disabled
            CleanupTweens();
        }

        private void LateUpdate()
        {
            // Always face the player camera
            FaceCamera();
        }

        #endregion

        #region UIController Implementation
        // ----------------------------------------
        // UIController Implementation
        // ----------------------------------------

        protected override void Init()
        {
            _originalScale = transform.localScale;
        }

        public override void Dispose()
        {
            CleanupTweens();
            labelText = null;
        }

        #endregion

        #region Animation Methods
        // ----------------------------------------
        // Animation Methods
        // ----------------------------------------

        /// <summary>
        /// Plays a bounce animation when the widget appears.
        /// Widget starts from scale 0 and bounces to original scale.
        /// </summary>
        private void PlayAppearAnimation()
        {
            // Kill any existing tween
            _scaleTween?.Kill();

            // Start from zero scale
            transform.localScale = Vector3.zero;

            // Animate to original scale with bounce
            _scaleTween = transform.DOScale(_originalScale, _animationDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(gameObject)
                .SetAutoKill(true);
        }

        /// <summary>
        /// Plays a bounce animation when the widget disappears.
        /// Widget bounces up slightly then scales down to zero.
        /// Called by OverheadWidgetManager before returning widget to pool.
        /// </summary>
        public void PlayDisappearAnimation(System.Action onComplete = null)
        {
            // Kill any existing tween
            _scaleTween?.Kill();

            // Create sequence: bounce up, then scale down
            Sequence disappearSequence = DOTween.Sequence();

            disappearSequence.Append(
                transform.DOScale(_originalScale * _bounceScale, _animationDuration * 0.3f)
                    .SetEase(Ease.OutQuad)
            );

            disappearSequence.Append(
                transform.DOScale(Vector3.zero, _animationDuration * 0.7f)
                    .SetEase(Ease.InBack)
            );

            disappearSequence.SetLink(gameObject)
                .SetAutoKill(true)
                .OnComplete(() => onComplete?.Invoke());

            _scaleTween = disappearSequence;
        }

        /// <summary>
        /// Makes the widget always face the player camera.
        /// Called every LateUpdate to ensure widget is readable.
        /// </summary>
        private void FaceCamera()
        {
            if (Player.I == null || Player.I.PlayerCamera == null || Player.I.PlayerCamera.Camera == null)
            {
                return;
            }

            // Face the camera
            transform.rotation = Quaternion.LookRotation(
                transform.position - Player.I.PlayerCamera.Camera.transform.position
            );
        }

        /// <summary>
        /// Cleans up all active tweens to prevent memory leaks.
        /// Called on Dispose and OnDisable.
        /// </summary>
        private void CleanupTweens()
        {
            _scaleTween?.Kill();
            _scaleTween = null;
        }

        #endregion
    }
}