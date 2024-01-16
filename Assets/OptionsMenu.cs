using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private Slider MasterVolumeSlider;
    private Slider MusicVolumeSlider;
    private Slider SfxVolumeSlider;


    public bool optionsNeedsAttention; // to trigger some options menu methods
    private void Awake()
    {
        MasterVolumeSlider = transform.Find("MasterVolumeSlider").GetComponent<Slider>();
        MusicVolumeSlider = transform.Find("MusicVolumeSlider").GetComponent<Slider>();
        SfxVolumeSlider = transform.Find("SfxVolumeSlider").GetComponent<Slider>();
        Debug.Log("called");
    }
    
    private void OnEnable()
    {
        AudioManager.Instance.SetMasterSlider(MasterVolumeSlider);
        AudioManager.Instance.SetMusicSlider(MusicVolumeSlider);
        AudioManager.Instance.SetSfxSlider(SfxVolumeSlider);
        //AudioManager.Instance.SetSliderValues();
    }
    /*
    private void OnDisable()
    {
        AudioManager.Instance.RemoveSliderListeners();
    }
    */
    
}
