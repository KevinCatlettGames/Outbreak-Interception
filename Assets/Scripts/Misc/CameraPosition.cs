using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float zOffset;
    Vector3 position;

    private void Update()
    {
        position = playerTransform.position;
        position.z += zOffset;
        transform.position = position;
    }
}
