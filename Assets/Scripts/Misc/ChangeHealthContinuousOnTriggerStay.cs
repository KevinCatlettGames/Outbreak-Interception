using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Applies damage to objects with a health system component after a duration if a trigger is still occurring. 
/// The collided objects with a health system component are saved in a dictionary and a timer is counted down for each to apply damage after a duration.
/// </summary>
public class ChangeHealthContinuousOnTriggerStay : MonoBehaviour
{
    #region Variables
    [Tooltip("The objects with a layer inside of this layermask can be damaged by this script.")]
    [SerializeField] LayerMask healthChangeableLayerMask;

    [Tooltip("The amount of health that will be changed when a trigger occurs.")]
    [SerializeField] float healthChangeAmount = -1;

    [Tooltip("The amount of time to wait before applying damage for the first time.")]
    [SerializeField] float initialWaitDuration = 1f;

    [Tooltip("The amount of time to wait before applying damage again.")]
    [SerializeField] float waitDuration = 2f;

    // A dictionary that contains the objects that are currently being hit by the laser and the time that has passed since the last hit.
    Dictionary<GameObject, float> hitObjects;

    // Gets invoked when a object with a health system component gets hurt, can be used to apply effects.
    public Action<Vector3> OnHit;
    #endregion
    #region Unity Methods
    /// <summary>
    /// Initializes the dictionary.
    /// </summary>
    void Awake()
    {
        hitObjects = new Dictionary<GameObject, float>();
    }

    /// <summary>
    /// For every object in the hitObjects dictionary, decrease the timer by the time that has passed since the last frame.
    /// </summary>
    void Update()
    {
        DamageCollidedObjects();
    }
    #endregion
    #region Methods
    /// <summary>
    /// For every object in the hitObjects dictionary, decrease the timer by the time that has passed since the last frame.
    /// </summary>
    void DamageCollidedObjects()
    {
        if (hitObjects.Count > 0)
        {
            foreach (GameObject hitObject in hitObjects.Keys.ToList())
            {
                hitObjects[hitObject] -= Time.deltaTime;
            }
        }
    }
    #endregion
    #region Triggers
    /// <summary>
    ///  If the object that entered the trigger is in the healthChangeableLayerMask, add it to the hitObjects dictionary
    ///  and set the value to the waitDuration.
    /// </summary> 
    /// <param name="other"></param> The collider that entered the trigger.
    void OnTriggerEnter(Collider other)
    {
        if ((healthChangeableLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (!hitObjects.ContainsKey(other.gameObject))
            {
                hitObjects.Add(other.gameObject, initialWaitDuration);
            }
        }
    }

    /// <summary>
    /// If the object that is still in the trigger is in the hitObjects dictionary and the timer has reached 0 for that object, apply damage.
    /// </summary>
    /// <param name="other"></param> The collider that is still in the trigger.
    void OnTriggerStay(Collider other)
    {
        if ((healthChangeableLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (hitObjects.ContainsKey(other.gameObject))
            {
                if (hitObjects[other.gameObject] <= 0)
                {
                    other.GetComponent<HealthSystem>().HealthChange(healthChangeAmount, 0, Vector3.zero);
                    hitObjects[other.gameObject] = waitDuration;
                    OnHit?.Invoke(other.transform.position);
                }
            }
        }
    }

    /// <summary>
    /// If the object that exited the trigger is in the hitObjects dictionary, remove it. 
    /// </summary>
    /// <param name="other"></param> The collider that exited the trigger.
    void OnTriggerExit(Collider other)
    {
        if (hitObjects.ContainsKey(other.gameObject))
        {
            hitObjects.Remove(other.gameObject);
        }
    }
    #endregion
}

