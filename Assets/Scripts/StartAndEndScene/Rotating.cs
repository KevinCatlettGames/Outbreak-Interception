using UnityEngine;

public class Rotating : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    void Update ()
    {
        transform.Rotate (rotation);
    }
    
}
