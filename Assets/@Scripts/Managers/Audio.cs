using System;
using ComBots.Game;
using ComBots.Utils.EntryPoints;
using UnityEngine;

namespace ComBots.Global.Audio
{
    public class AudioManager : EntryPointMono
    {
        public static AudioManager I { get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [Header("Audio Source")]
        [SerializeField] private AudioSource _musicSource;

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
    }
}