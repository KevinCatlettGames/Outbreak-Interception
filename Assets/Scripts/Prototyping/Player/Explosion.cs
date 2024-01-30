using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class deals the damage an knockback of an explosion and spawns an effect.
/// </summary>
public class Explosion : MonoBehaviour
{
    #region Variables

    // Healthsystem of anyone caught in the explosion.
    HealthSystem healthSystem;
    // The direction in which they are launched.
    Vector3 lauchDirection;

    [Tooltip("This is the affected area of the explosion (starts inactive).")]
    [SerializeField] SphereCollider explosionCollider;

    [Tooltip("The trigger needs a little time to procces all the collisions.")]
    [SerializeField] float explosionTime;

    [Tooltip("AudioSource connected to the 'Weapons' output.")]
    [SerializeField] AudioSource audioSource;

    [Tooltip("The damage dealt by the grenade.")]
    [SerializeField] float damage;

    [Tooltip("The knockback dealt by the grenade.")]
    [SerializeField] float knockback;

    [Tooltip("A bool to determine weather the player is effected by this explosion.")]
    [SerializeField] bool affectsPlayer;

    [Tooltip("The particle effect spawned by the explosion.")]
    [SerializeField] GameObject explosionEffect;

    // Timer for the explosiontime.
    float timer;
    // Bool prevent the explosion from happening twice.
    bool isExploded;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Once the grenade has exploded and the audiosource has finished playing the object will destroy itself.
    /// </summary>
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                explosionCollider.enabled = false;
            }    
        }
        if (isExploded && audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This trigger will, once activated, apply the damage and knockback to any eneties in the range of the collider.
    /// Including the player is the affects player bool is true.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name + " boom");
        if (affectsPlayer == false)
        {
            if (!other.isTrigger && other.gameObject.CompareTag("Enemy"))
            {
                healthSystem = other.GetComponent<HealthSystem>();
                lauchDirection = (other.transform.position - transform.position).normalized;
                lauchDirection.y = 0;
                healthSystem.HealthChange(-damage,knockback,lauchDirection);
            }
        }
        else
        {
            if (!other.isTrigger && (other.gameObject.CompareTag("Enemy")|| other.gameObject.CompareTag("Player")))
            {
                healthSystem = other.GetComponent<HealthSystem>();
                lauchDirection = (other.transform.position - transform.position).normalized;
                lauchDirection.y = 0;
                healthSystem.HealthChange(-damage, knockback, lauchDirection);
            }
        }
    }

    #endregion

    #region Explosion

    /// <summary>
    /// This method is called from outside this script to trigger the explosion.
    /// </summary>
    public void Explode()
    {
        BlowUp();
    }

    /// <summary>
    /// A version of the Explosion() method with the option to overwrite the damage and knockback. 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="knockback"></param>
    public void Explode(float damage,float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
        BlowUp();
    }

    /// <summary>
    /// This function activates the explosion collider, instantiates the explosion effect and plays the sound.
    /// The isExploded bool is set to true alloing the update to destroy the object one the sound has finished playing.
    /// </summary>
    private void BlowUp()
    {
        explosionCollider.enabled = true;
        timer = explosionTime;
        isExploded = true;
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        audioSource.PlayOneShot(audioSource.clip);
    }

    #endregion
}
