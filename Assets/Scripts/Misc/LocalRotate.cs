using UnityEngine;

public class LocalRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;

    void Update()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + rotation.x, transform.localEulerAngles.y + rotation.y, transform.localEulerAngles.z + rotation.z);
    }
}
