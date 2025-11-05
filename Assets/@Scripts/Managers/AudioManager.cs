using ComBots.Game;
using ComBots.Utils.EntryPoints;
using R3;
using Sirenix.Utilities;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton that manages the game's background music
/// </summary>
public class AudioManager : MonoBehaviourR3
{
    private static AudioManager Instance { get; set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource _musicSource;

    [field: SerializeField]
    private UISo UISo { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        Instance = this;

        var onOpenMenu = Observable.FromEvent<AudioClip>(
            h => UISo.OnBackgroundMusicSelected += h,
            h => UISo.OnBackgroundMusicSelected -= h);

        AddEvents(
            onOpenMenu.Subscribe(TrySetBackgroundMusic)
        );
    }

    /// <summary>
    /// Sets the background music if it's not already playing or if it's 
    /// a different clip.
    /// </summary>
    /// <param name="clip"></param>
    public static async void TrySetBackgroundMusic(AudioClip clip)
    {
        while (Instance == null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        if (clip == Instance._musicSource.clip 
            && Instance._musicSource.isPlaying)
        {
            return;
        }

        Instance._musicSource.clip = clip;
        Instance._musicSource.Play();
    }
}
