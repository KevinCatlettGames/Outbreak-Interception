using UnityEngine;

/// <summary>
/// Checks if a transform with the specificed layer is in range.
/// </summary>
public class Observer : MonoBehaviour
{
    bool observingObject;
    public bool ObservingObject { get { return observingObject;} }

    [SerializeField] LayerMask observerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((observerLayer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            observingObject = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((observerLayer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            observingObject = false;
        }
    }
}
