using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticlesOnDamaged : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;

    [SerializeField] ParticleSystem[] particlesToEmit;

    private void Start()
    {
        if (particlesToEmit.Length > 0)
        {
            if (GetComponent<HealthSystem>())
            {
                healthSystem = GetComponent<HealthSystem>();
                healthSystem.OnDamageEvent.AddListener(ActivateAtHalfHealth);
            }
        }
    }

    void ActivateAtHalfHealth(Vector3 direction, float knockBack)
    {
        foreach (ParticleSystem p in particlesToEmit)
        {
            if(p)
            p.Emit(10);
        }      
    }
}
