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
    [SerializeField] private AudioClip[] MusicSounds;
    [SerializeField] private Slider musicVolumeSlider, sfxVolumeSlider, masterVolumeSlider;
    [SerializeField] private bool toggleMusic, toggleSfx, toggleMaster; // for the mute buttons  
    [SerializeField] private Sprite volumeNormal, volumeMuted;
    
    public float masterVolume, musicVolume, sfxVolume;


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
        musicVolumeSlider.value = -15;
        sfxVolumeSlider.value = -10;
        masterVolumeSlider.value = -10;

        SetVolume();

        musicSource.clip = MusicSounds[0];
        musicSource.Play();
    }


    void SetVolume()
    {
        audioMixer.SetFloat("masterVolume", masterVolumeSlider.value);
        audioMixer.SetFloat("musicVolume", musicVolumeSlider.value);
        audioMixer.SetFloat("sfxVolume", sfxVolumeSlider.value);
    }

    public void SetSliders()
    {
        Debug.Log("volume: "+ musicVolume + " " + sfxVolume + " " + masterVolume);
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
        masterVolumeSlider.value = masterVolume;
        // gets terrible error if invoked; TODO
    }


    public void ChangeMasterVolume(float value)
    {
        audioMixer.SetFloat("masterVolume", masterVolumeSlider.value);
        masterVolume = masterVolumeSlider.value;
        Debug.Log("masterVolume " + masterVolume);
    }

    public void ChangeMusicVolume(float value)
    {
        audioMixer.SetFloat("musicVolume", musicVolumeSlider.value);
        musicVolume = musicVolumeSlider.value;
        Debug.Log("musicVolume " + musicVolume);
    }

    public void ChangeSfxVolume(float value)
    {
        audioMixer.SetFloat("sfxVolume", sfxVolumeSlider.value);
        sfxVolume = sfxVolumeSlider.value;
        Debug.Log("sfxVolume " + sfxVolume);
    }


    public void ToggleMusic() // muting and un-muting music
    {
        musicSource.mute = !musicSource.mute;

        Image currentIcon = GameObject.Find("MuteButtonMusic").GetComponent<Image>();
        ChangeIcon(currentIcon);
    }

    public void ToggleSfx() // muting and un-muting sfx
    {
        sfxSource.mute = !sfxSource.mute;

        Image currentIcon = GameObject.Find("MuteButtonSfx").GetComponent<Image>();
        ChangeIcon(currentIcon);
    }
    


    public void ToggleMaster() // muting and unmuting music&sfx
    {
        musicSource.mute = !musicSource.mute;
        sfxSource.mute = !sfxSource.mute;
        sfxSource.mute = !sfxSource.mute;

        Image currentIcon = GameObject.Find("MuteButtonMaster").GetComponent<Image>();
        ChangeIcon(currentIcon);
    }

    public void ChangeIcon(Image currentIcon)
    {

        if (currentIcon.sprite == volumeNormal)
        {
            currentIcon.sprite = volumeMuted;
        }
        else
        {
            currentIcon.sprite = volumeNormal;
        }
    }
}
