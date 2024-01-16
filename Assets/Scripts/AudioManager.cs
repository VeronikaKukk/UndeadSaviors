using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer; 
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private AudioClip[] MusicSounds;
    [SerializeField] private Slider musicVolumeSlider, sfxVolumeSlider, masterVolumeSlider;
    [SerializeField] private bool toggleMusic, toggleSfx, toggleMaster; // for the mute buttons  
    [SerializeField] private Sprite volumeNormal, volumeMuted;
   
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

        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSfxVolume();

        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadMasterVolume();
        } 
        else
        {
            ChangeMasterVolume();
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            ChangeMusicVolume();
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadSfxVolume();
        }
        else
        {
            ChangeSfxVolume();
        }
    }

    public void ChangeMasterVolume()
    {
        float value = masterVolumeSlider.value;
        audioMixer.SetFloat("masterVolume", Mathf.Log10(value) *20);
        Debug.Log("masterVolume " + value);
        PlayerPrefs.SetFloat("masterVolume", value);
    }

    private void LoadMasterVolume()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        ChangeMasterVolume();
    }

    public void ChangeMusicVolume()
    {
        float value = musicVolumeSlider.value;
        audioMixer.SetFloat("musicVolume", Mathf.Log10(value) *20);
        Debug.Log("musicVolume " + value);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    private void LoadMusicVolume()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        ChangeMusicVolume();
    }

    public void ChangeSfxVolume()
    {
        float value = sfxVolumeSlider.value;
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(value) *20);
        Debug.Log("sfxVolume " + value);
        PlayerPrefs.SetFloat("sfxVolume", value);
    }

    private void LoadSfxVolume()
    {
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        ChangeSfxVolume();
    }

    public void SetMusicSlider(Slider slider) {
        musicVolumeSlider = slider;
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        musicVolumeSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });
    }

    public void SetSfxSlider(Slider slider)
    {
        sfxVolumeSlider = slider;
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        sfxVolumeSlider.onValueChanged.AddListener(delegate { ChangeSfxVolume(); });

    }
    public void SetMasterSlider(Slider slider)
    {
        masterVolumeSlider = slider;
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        masterVolumeSlider.onValueChanged.AddListener(delegate { ChangeMasterVolume(); });

    }
}
