using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Game/AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    public float VolumeMin = 1.0f;
    public float VolumeMax = 1.0f;
    public float PitchMin = 1.0f;
    public float PicthMax = 1.0f;

    public float Cooldown = 0.1f;

    private static float nextPlayTime = 0;

    public List<AudioClip> AudioClips;


    public void Play()
    {
        Play(AudioSourcePool.Instance.GetSource());
    }

    public void Play(AudioSource source) 
    {
        if (Time.unscaledTime < nextPlayTime) return;

        nextPlayTime = Time.unscaledTime + Cooldown;

        source.clip = AudioClips[Random.Range(0, AudioClips.Count)];
        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PicthMax);

        source.Play();
    }
}