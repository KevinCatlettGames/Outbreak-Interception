using UnityEngine;

/// <summary>
/// Subscribes to the OnHit event of the ChangeHealthContinuousOnTriggerStay script and spawns the laser hit effect when it gets invoked.
/// </summary>
public class SpawnLaserHitEffect : MonoBehaviour
{
    [Tooltip("The script that will be subscribed to.")]
    [SerializeField] ChangeHealthContinuousOnTriggerStay changeHealthContinuousOnTriggerStay;

    [Tooltip("The laser hit effect that will be spawned when the OnHit event gets invoked.")]
    [SerializeField] GameObject hitEffect;

    void Start()
    {
        changeHealthContinuousOnTriggerStay.OnHit += SpawnLaserEffect;
    }

    void SpawnLaserEffect(Vector3 position)
    {
        Instantiate(hitEffect, position, Quaternion.identity);
    }
}
