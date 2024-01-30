using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is the class which every drop inherits from.
/// </summary>

#region Required Components
// The sphere collider is the pickup trigger for the drop.
[RequireComponent(typeof(SphereCollider))]
// The box collider is used for collision when the drop is spawned.
[RequireComponent(typeof(BoxCollider))]
// The rigidbody is neccecary for the launch.
[RequireComponent(typeof(Rigidbody))]
#endregion
public abstract class Drop : MonoBehaviour
{
    #region Variables
    [Header("Player Stats Scriptable Object")]
    [Tooltip("The player stats scriptable object that will be used to store all the values that are needed for the enemy to function.")]
    [SerializeField] protected PlayerStats playerStats;

    [Header("Audio")]
    [Tooltip("AudioSource connected with the 'Drops' output.")]
    [SerializeField] protected AudioSource audioSource;

    [Tooltip("Sound which plays when the drop is spawned.")]
    [SerializeField] protected AudioClip dropSound;

    [Tooltip("Sound which plays when the drop is collected.")]
    [SerializeField] protected AudioClip pickupSound;

    [Header("Launch")]
    [Tooltip("The power of the launch when the drop spawns.")]
    [SerializeField] protected float lauchStrengh;

    // Indicator for when the drop is alreday collected to prevent collecting it twice.
    protected bool isPickedUp = false;
    // The launchvector for the drop on spawn.
    protected Vector3 dropVector3D;
    /// <summary>
    /// The following are the variables for the components required above.
    /// </summary>
    protected Rigidbody rb;
    protected SphereCollider pickupTrigger;
    protected BoxCollider bodyCollider;
    #endregion
}
