using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider, sfxVolumeSlider;
    [SerializeField] private bool toggleMusic, toggleSfx; // for the mute button(s) 

    private void Start()
    {
        AudioManager.Instance.ChangeMasterVolume(0.5f); // starting with default value
        musicVolumeSlider.onValueChanged.AddListener(value => AudioManager.Instance.ChangeMusicVolume(value));
        sfxVolumeSlider.onValueChanged.AddListener(value => AudioManager.Instance.ChangeSfxVolume(value));
    }

    public void Toggle()
    {
        if (toggleMusic) AudioManager.Instance.ToggleMusic();
        if (toggleSfx) AudioManager.Instance.ToggleSfx();
    }

}
