using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _backgroundFX = null;
    AudioSource _audioSource;

    bool _songIsPlaying;

    private void Awake()
    {
        _songIsPlaying = false;
        _audioSource = GetComponent<AudioSource>();
        PlaySong(_backgroundFX);
    }

    public void PlaySong(AudioClip clip)
    {
        if (!_songIsPlaying)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
            _songIsPlaying = true;
        }
    }
}