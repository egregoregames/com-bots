using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ComBots.UI.Utilities
{
    public class WC_Typewriter : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Settings")]
        [SerializeField] private float _speedPerCharacter;

        // ============ State ============ //
        public bool IsTyping { get; private set; }
        private float _timer;

        // ============ Cache ============ //
        private string _fullText;
        private UnityAction _onComplete;
        private int _currentIndex;

        #region Unity Methods
        // ----------------------------------------
        // Unity Methods 
        // ----------------------------------------
        void Update()
        {
            if (!IsTyping)
            {
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= _speedPerCharacter)
            {
                _timer = 0f;
                _currentIndex++;
                _text.text = _fullText[.._currentIndex];
            }

            if (_currentIndex >= _fullText.Length)
            {
                IsTyping = false;
                _onComplete?.Invoke();
            }
        }
        #endregion

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public void SetActive(string text, UnityAction onComplete)
        {
            _fullText = text;
            _onComplete = onComplete;
            _currentIndex = 0;
            _timer = 0f;
            _text.text = string.Empty;
            IsTyping = true;
        }

        public void SetInactive(bool fillTextInstantly, bool invokeCallback)
        {
            if (fillTextInstantly)
            {
                _text.text = _fullText;
                if (invokeCallback)
                {
                    _onComplete?.Invoke();
                }
            }
            else
            {
                _text.text = string.Empty;
            }
            IsTyping = false;
        }
        #endregion
    }
}