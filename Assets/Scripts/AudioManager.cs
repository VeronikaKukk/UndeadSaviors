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
    [SerializeField] private AudioSource musicSource;
    public AudioSource sfxSource;
    [SerializeField] private AudioClip[] MusicSounds;
    [SerializeField] private Slider musicVolumeSlider, sfxVolumeSlider, masterVolumeSlider;
    [SerializeField] private bool toggleMusic, toggleSfx, toggleMaster; // for the mute buttons 

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
            audioMixer.SetFloat("SfxVolume", PlayerPrefs.GetFloat("SfxVolume")); //assign sounds to groups
        }
        else {
            SetSliders();
        }


        musicSource.clip = MusicSounds[0];
        musicSource.Play();
    }


    void SetSliders()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume");
       
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

    public void ToggleMaster() // muting and unmuting music&sfx
    {
        musicSource.mute = !musicSource.mute;
        sfxSource.mute = !sfxSource.mute;
    }

}
