using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticleSystemAnimationEvent : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleSystemsToStop;

    public void StopParticleSystems()
    {
        foreach(ParticleSystem p in particleSystemsToStop)
        {
            p.Stop();
        }
    }
}

