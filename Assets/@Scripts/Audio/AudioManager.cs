using System;
using System.Collections;
using ComBots.Game;
using ComBots.Utils.EntryPoints;
using ComBots.Utils.ObjectPooling;
using UnityEngine;

namespace ComBots.Global.Audio
{
    public class AudioManager : EntryPointMono
    {
        public static AudioManager I { get; private set; }

        // =============== EntryPointMono Implementation =============== //
        public override Dependency Dependency => Dependency.Independent;

        // =============== Music =============== //
        [Header("Music")]
        [SerializeField] private AudioSource _musicSource;

        // =============== Audio Source Pool =============== //
        private const string PK_AUDIO_SOURCE = "Audio_Source";        

        protected override void Init()
        {
            I = this;
        }

        public override void Dispose()
        {
            if (I == this)
            {
                I = null;
            }
        }

        private void Start()
        {
            GlobalConfig.I.UISo.OnBackgroundMusicSelected += TrySetBackgroundMusic;
        }

        /// <summary>
        /// Sets the background music if it's not already playing or if it's a different clip.
        /// </summary>
        /// <param name="clip"></param>
        public void TrySetBackgroundMusic(AudioClip clip)
        {
            if (clip == _musicSource.clip && _musicSource.isPlaying)
            {
                return;
            }

            _musicSource.clip = clip;
            _musicSource.Play();
        }

        #region Private Methods
        // ----------------------------------------
        // Private Methods 
        // ----------------------------------------
        
        private IEnumerator PlaySFXAsync(AudioClip audioClip)
        {
            AudioSource audioSource = PoolManager.I.Pull<AudioSource>(PK_AUDIO_SOURCE);
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = audioClip;
            audioSource.Play();

            while (audioSource.isPlaying)
            {
                yield return null;
            }
            PoolManager.I.Push(PK_AUDIO_SOURCE, audioSource);
        }

        #endregion

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public void PlaySFX(AudioClip audioClip)
        {
            StartCoroutine(PlaySFXAsync(audioClip));
        }

        /// <summary>
        /// Pulls an Audio Source from the Audio Sources Pool.
        /// </summary>
        /// <returns></returns>
        public AudioSource PullAudioSource()
        {
            return PoolManager.I.Pull<AudioSource>(PK_AUDIO_SOURCE);
        }
        
        /// <summary>
        /// Pushes back an audio source to the Audio Sources Pool.
        /// </summary>
        /// <param name="audioSource"></param>
        public void PushAudioSource(AudioSource audioSource)
        {
            PoolManager.I.Push(PK_AUDIO_SOURCE, audioSource);
        }

        #endregion
    }
}