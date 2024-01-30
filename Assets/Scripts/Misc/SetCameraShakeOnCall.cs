using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraShakeOnCall : MonoBehaviour
{
    public CameraShaker shaker;
    public void BeginShake()
    {
        shaker.ShakeCamera(10, 3);
    }
}
