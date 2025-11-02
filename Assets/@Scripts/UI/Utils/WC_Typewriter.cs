using ComBots.Global.Audio;
using ComBots.Logs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ComBots.UI.Utils
{
    public class WC_Typewriter : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _text;
        [Header("Settings")]
        [SerializeField] private float _speedPerCharacter;
        // =============== Sound Effects =============== //
        [Header("Sound Effects")]
        public TypeWriterSoundEffects _defaultSoundEffects;
        [SerializeField] private AudioSource _audioSource;

        // ============ State ============ //
        public bool IsTyping { get; private set; }
        private float _timer;

        // ============ Cache ============ //
        private string _fullText;
        private UnityAction _onComplete;
        private int _currentIndex;

        #region Unity Lifecycle
        // ----------------------------------------
        // Unity Lifecycle
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
                _audioSource.Stop();
                AudioManager.I.PushAudioSource(_audioSource);
                _audioSource = null;
                _onComplete?.Invoke();
            }
        }
        #endregion

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public void SetActive(string text, UnityAction onComplete, TypeWriterSoundEffects soundEffects = null)
        {
            _fullText = text;
            _onComplete = onComplete;
            _currentIndex = 0;
            _timer = 0f;
            _text.text = string.Empty;
            IsTyping = true;
            // SFX
            if (!_audioSource)
            {
                _audioSource = AudioManager.I.PullAudioSource();
                _audioSource.loop = true;
            }
            if (soundEffects != null)
            {
                _audioSource.clip = soundEffects.TypeWriterLoop;
            }
            else
            {
                _audioSource.clip = _defaultSoundEffects.TypeWriterLoop;
            }
            MyLogger<WC_Typewriter>.StaticLog($"{_audioSource}");
            MyLogger<WC_Typewriter>.StaticLog($"{_audioSource.clip}");
            MyLogger<WC_Typewriter>.StaticLog($"{_audioSource.loop}");
            _audioSource.Play();
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