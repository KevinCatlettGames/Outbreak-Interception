using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// This class connects the health system and the exploion effect of the exploding barrel.
/// </summary>
public class ExplodingBarrel : MonoBehaviour
{
    [Tooltip("The explosion exxect prefab that spawns when the barrel is destroyed.")]
    [SerializeField] GameObject ExplosionEffect;

    [Tooltip("Damage inflicted upon explosion.")]
    [SerializeField] float damage;

    [Tooltip("Knockback inflicted upon explosion.")]
    [SerializeField] float knockback;

    // The mesh which is not broken.
    private GameObject barrelMesh;

    // The mesh which is broken
    private GameObject brokenBarrelMesh;

    // Collider to deactivate collision when destroyed.
    private new CapsuleCollider collider;
    
    /// <summary>
    /// In the start method the childeren of this game object are searched for the meshs.
    /// </summary>
    private void Start()
    {
        GameObject meshs = transform.GetChild(0).gameObject;
        barrelMesh = meshs.transform.GetChild(0).gameObject;
        brokenBarrelMesh = meshs.transform.GetChild(1).gameObject;
        barrelMesh?.SetActive(true);
        brokenBarrelMesh?.SetActive(false);
        collider = GetComponent<CapsuleCollider>();
    }

    /// <summary>
    /// This mehtod is called by the health system when the barrel reaches 0 hp and triggers the explosion.
    /// </summary>
    public void OnDeath()
    {
        collider.enabled = false;
        barrelMesh?.SetActive(false);
        brokenBarrelMesh?.SetActive(true);
        GameObject explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Explosion explosionScript = explosion.GetComponent<Explosion>();
        if (damage == 0 || knockback == 0)
        {
            explosionScript.Explode();
        }
        else
        {
            explosionScript.Explode(damage,knockback);
        }
    }
}
