using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private bool toggleMusic, toggleSfx; // for the mute buttons not yet existing/functional

    private void Start()
    {
        AudioManager.Instance.ChangeMasterVolume(volumeSlider.value); // starting with default value
        volumeSlider.onValueChanged.AddListener(value => AudioManager.Instance.ChangeMasterVolume(value));
    }

    public void Toggle()
    {
        if (toggleMusic) AudioManager.Instance.ToggleMusic();
        if (toggleSfx) AudioManager.Instance.ToggleSfx();
    }
}
