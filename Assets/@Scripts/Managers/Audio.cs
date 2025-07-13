using System;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] UISo uiSo;

    AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        uiSo.SoundSelected += ChangeAudio;
    }

    void ChangeAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
