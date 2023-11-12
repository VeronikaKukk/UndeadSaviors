using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private AudioClip[] MusicSounds, SfxSounds;

    private void Awake()
    {   
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        musicSource.clip = MusicSounds[0];
        musicSource.Play();
    }


    public void ChangeMasterVolume(float value)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(value) * 20); // converting since audiomixer uses dB
    }


    public void ToggleMusic() // muting and un-muting music
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSfx() // muting and un-muting sfx
    {
        sfxSource.mute = !sfxSource.mute;
    }

}
