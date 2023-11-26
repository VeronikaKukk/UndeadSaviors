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
    
    //public float masterVolume, musicVolume, sfxVolume;


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

    public void SetSliderValues()
    {
        float mVS;
        if (audioMixer.GetFloat("musicVolume", out mVS))
        {
            musicVolumeSlider.value = mVS;
        }
        if (audioMixer.GetFloat("masterVolume", out mVS))
        {
            masterVolumeSlider.value = mVS;
        }
        if (audioMixer.GetFloat("sfxVolume", out mVS))
        {
            sfxVolumeSlider.value = mVS;
        }
    }

    public void SetMusicSlider(Slider slider) {
        musicVolumeSlider = slider;
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }
    public void RemoveSliderListeners() {
        musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(ChangeSfxVolume);
        masterVolumeSlider.onValueChanged.RemoveListener(ChangeMasterVolume);

    }
    public void SetSfxSlider(Slider slider)
    {
        sfxVolumeSlider = slider;
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSfxVolume);

    }
    public void SetMasterSlider(Slider slider)
    {
        masterVolumeSlider = slider;
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);

    }
    public void ChangeMasterVolume(float value)
    {
        audioMixer.SetFloat("masterVolume", masterVolumeSlider.value);
        Debug.Log("masterVolume " + masterVolumeSlider.value);
    }

    public void ChangeMusicVolume(float value)
    {
        audioMixer.SetFloat("musicVolume", musicVolumeSlider.value);
        Debug.Log("musicVolume " + musicVolumeSlider.value);
    }

    public void ChangeSfxVolume(float value)
    {
        audioMixer.SetFloat("sfxVolume", sfxVolumeSlider.value);
        Debug.Log("sfxVolume " + sfxVolumeSlider.value);
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
