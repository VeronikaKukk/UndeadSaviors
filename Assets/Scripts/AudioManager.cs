using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private AudioClip[] MusicSounds, SfxSounds;
    [SerializeField] private Slider musicVolumeSlider, sfxVolumeSlider, masterVolumeSlider;
    [SerializeField] private bool toggleMusic, toggleSfx; // for the mute buttons 

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
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            audioMixer.SetFloat("SfxVolume", PlayerPrefs.GetFloat("SfxVolume"));
        }
        else {
            SetSliders();
        }


        musicSource.clip = MusicSounds[0];
        musicSource.Play();
    }


    void SetSliders()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
    }


    public void ChangeMasterVolume(float value)
    {
        audioMixer.SetFloat("masterVolume", masterVolumeSlider.value);
    }

    public void ChangeMusicVolume(float value)
        {
            audioMixer.SetFloat("musicVolume", musicVolumeSlider.value);
        }

    public void ChangeSfxVolume(float value)
    {
        audioMixer.SetFloat("sfxVolume", sfxVolumeSlider.value);
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
