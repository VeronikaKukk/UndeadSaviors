using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public AudioSource AudioSourcePrefab = null;
    public static AudioSourcePool Instance;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private void Awake()
    {
        Instance = this;
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                print(source.name);
                return source;
            }
        }
        AudioSource newSource = Instantiate<AudioSource>(AudioSourcePrefab);
        newSource.transform.SetParent(transform, false);
        audioSources.Add(newSource);
        return newSource;
    }
}