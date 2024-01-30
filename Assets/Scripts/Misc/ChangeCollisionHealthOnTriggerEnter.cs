using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies damage to objects with a health system component once when a trigger occurs.
/// </summary>
public class ChangeCollisionHealthOnTriggerEnter : MonoBehaviour
{
    [Tooltip("The objects with a layer inside of this layermask can be damaged by this script.")]
    [SerializeField] LayerMask healthChangeableLayerMask;

    [Tooltip("The amount of health that will be changed when a trigger occurs.")]
    [SerializeField] float healthChangeAmount = -1;

    // This list is used to make sure that the same object is not damaged multiple times.
    List<GameObject> hitObjects;

    void Awake()
    {
        hitObjects = new List<GameObject>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if(hitObjects.Contains(collision.gameObject)) return;

        if ((healthChangeableLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            collision.GetComponent<HealthSystem>().HealthChange(healthChangeAmount, 0, Vector3.zero);   
            hitObjects.Add(collision.gameObject);
        }
    }
}
