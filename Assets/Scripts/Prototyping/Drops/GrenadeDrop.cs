using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the class for the Grenade Drop prefab.
/// </summary>
public class GrenadeDrop : Drop
{
    [Tooltip("The amount of grenades in this drop.")]
    [SerializeField] int grenadeAmount;

    #region Unity Methods

    /// <summary>
    /// On spawn (Start) the drop sound is played and the drop is launched in a random direction to spread out multiple drops.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pickupTrigger = GetComponent<SphereCollider>();
        bodyCollider = GetComponent<BoxCollider>();
        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);
        dropVector3D = Random.insideUnitSphere;
        dropVector3D *= lauchStrengh;
        Debug.Log(dropVector3D);
        rb.AddForce(dropVector3D, ForceMode.Impulse);
    }

    /// <summary>
    /// Once the drop is picked up and the pickup sound has finished playing the gameObject destroys itself.
    /// </summary>
    private void Update()
    {
        if (isPickedUp == true && audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Collision Interactions

    /// <summary>
    /// Once the drop has hit the floor it should stop moving and no longer collide with anything.
    /// </summary>
    /// <param name="collision"> Object which has collided with the box collider of the drop. </param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.isKinematic = true;
            bodyCollider.enabled = false;
        }
    }

    /// <summary>
    /// When the player enters the circle collider (trigger) the drop is picked up and the player gains the grenades.
    /// Additionally the pickup sound is played and all colliders are deactivated.
    /// </summary>
    /// <param name="other"> Any object in the circle collider. </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats.Grenades += grenadeAmount;
            isPickedUp = true;
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound);
        }
    }

    #endregion
}
