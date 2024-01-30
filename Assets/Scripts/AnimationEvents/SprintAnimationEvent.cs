using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAnimationEvent : MonoBehaviour
{
    [SerializeField] ParticleSystem leftFootParticleSystem;
    [SerializeField] ParticleSystem rightFootParticleSystem;

    [SerializeField] int emitAmount = 5;
    public void EmitParticlesLeft()
    {
        leftFootParticleSystem.Emit(emitAmount);
    }

    public void EmitParticlesRight()
    {
        rightFootParticleSystem.Emit(emitAmount);
    }
}
