using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the class for the Weapon Drop prefab.
/// </summary>
public class WeaponDrop : Drop 
{
    #region Variables

    [Tooltip("The stats of the weapon contained in this drop.")]
    [SerializeField] WeaponStats weaponStats;

    [Tooltip("The speed of the rotation when the drop is on the floor.")]
    [SerializeField] float rotationAngle;

    // The pivot the weapon model rotates around.
    private Transform weaponPivot;

    #endregion

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
        dropVector3D = new Vector3(0, 1, 0);
        dropVector3D *= lauchStrengh;
        rb.AddForce(dropVector3D, ForceMode.Impulse);
        weaponPivot = transform.GetChild(0).transform;
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

    /// <summary>
    /// The rotate method is called in the fixed update for a constant rotation speed.
    /// </summary>
    private void FixedUpdate()
    {
        if (isPickedUp == false && rb.isKinematic == true)
        {
            RotateWeapon();
        }    
    }

    /// <summary>
    /// Rotates the weapon around the pivot by the rotation angle every time the method is called.
    /// </summary>
    private void RotateWeapon()
    {
        weaponPivot.Rotate(Vector3.up, rotationAngle);
    }

    #endregion

    #region Collision Interactions

    /// <summary>
    /// Once the drop has hit the floor it should stop moving and no longer collide with anything.
    /// </summary>
    /// <param name="collision"> Object which has collided with the box collider of the drop. </param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.isKinematic = true;
            bodyCollider.enabled = false;
        }
    }

    /// <summary>
    /// When the player enters the circle collider (trigger) the drop is picked up if currently has a different weapon.
    /// Additionally the pickup sound is played and all colliders are deactivated.
    /// </summary>
    /// <param name="other"> Any object in the circle collider. </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerStats.CurrentWeaponStats.WeaponID != weaponStats.WeaponID)
        {
            PlayerWeaponHandler playerWeaponHandler = other.GetComponent<PlayerWeaponHandler>();
            playerWeaponHandler.EquipWeapon(weaponStats);
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound);
            pickupTrigger.enabled = false;
            bodyCollider.enabled = false;
            isPickedUp = true;
        }
    }

    #endregion
}

