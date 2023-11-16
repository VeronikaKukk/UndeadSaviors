using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public ParticleSystem ParticlesPrefab;

    public void PlayParticles(Vector3 position)
    {
        if (ParticlesPrefab != null)
        {
            ParticleSystem particles = Instantiate(ParticlesPrefab, position, Quaternion.identity);
            particles.Play();

            float duration = particles.main.duration;
            Destroy(particles.gameObject, duration);
        }
    }

}
