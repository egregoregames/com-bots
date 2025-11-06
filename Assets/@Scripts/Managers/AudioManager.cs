using R3;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Singleton that manages the game's background music and sound effects
/// </summary>
public class AudioManager : MonoBehaviourR3
{
    private static AudioManager Instance { get; set; }

    /// <summary>
    /// Should only ever include objects spawned on runtime. Is cleared on Awake
    /// </summary>
    [field: SerializeField, ReadOnly]
    private List<AudioSource> Pool { get; } = new();

    [SerializeField] private AudioSource _musicSource;

    [field: SerializeField]
    private UISo UISo { get; set; }

    #region Monobehaviour
    private new void Awake()
    {
        ClearPool();
        base.Awake();
    }

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
    #endregion

    private void ClearPool()
    {
        foreach (var item in Pool)
        {
            Destroy(item.gameObject);
        }

        Pool.Clear();
    }

    private AudioSource GetAudioSource()
    {
        AudioSource source = Pool.FirstOrDefault(x => !x.isPlaying);

        if (source == null)
        {
            var obj = new GameObject("AudioSource", typeof(AudioSource));
            DontDestroyOnLoad(obj);
            source = obj.GetComponent<AudioSource>();
            source.loop = false;
            Pool.Add(source);
        }

        return source;
    }

    public static async void PlaySoundEffect(AudioClip audioClip)
    {
        while (Instance == null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        var source = Instance.GetAudioSource();
        source.clip = audioClip;
        source.Play();
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
