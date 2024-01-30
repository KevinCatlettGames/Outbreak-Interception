using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Changes the value of a boolean inside of an animator when a trigger is entered or exited.
/// </summary>
public class AnimatorBooleanChangeOnTrigger : MonoBehaviour
{
    [Tooltip("The doors animator to play open or closed animations.")]
    [SerializeField] Animator animator;

    [Tooltip("The layer mask that will be used to check if the trigger is from the correct layer.")]
    [SerializeField] LayerMask triggerLayerMask;

    [Tooltip("The name of the boolean parameter that will be changed.")]
    [SerializeField] string booleanParameterName;

    [Tooltip("The entities that are in the door.")]
    List<GameObject> entitiesInDoor = new List<GameObject>();

    public UnityEvent OnTriggerOccured;

    #region Triggering

    /// <summary>
    /// When triggered by a collider, check if the collider is on the correct layer. If it is, change the value of the boolean parameter to false.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if ((triggerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (!entitiesInDoor.Contains(other.gameObject))
            {
                entitiesInDoor.Add(other.gameObject);
            }

            if(entitiesInDoor.Count == 1)
            {
                animator.SetBool(booleanParameterName, false);
            }

            OnTriggerOccured?.Invoke();
        }
    }

    /// <summary>
    /// When triggered by a collider, check if the collider is on the correct layer. If it is, change the value of the boolean parameter to true.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if ((triggerLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (entitiesInDoor.Contains(other.gameObject))
            {
                entitiesInDoor.Remove(other.gameObject);
            }

            if (entitiesInDoor.Count <= 0)
            {
                    animator.SetBool(booleanParameterName, true);
            }
        }
    }
    #endregion
}
