using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObjectOnTrigger : MonoBehaviour
{

    [Tooltip("The layer mask that will be used to activate the laser.")]
    [SerializeField] LayerMask collisionLayerMask;

    [SerializeField] GameObject objectToActivate;
    void OnTriggerEnter(Collider other)
    {
        if ((collisionLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (!objectToActivate.activeSelf)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
