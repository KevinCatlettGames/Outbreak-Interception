using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ActivateLaserOnTrigger : MonoBehaviour
{
    [Tooltip("The layer mask that will be used to activate the laser.")]
    [SerializeField] LayerMask collisionLayerMask;

    [SerializeField] List<DynamicLineRendererDrawing> laserList = new List<DynamicLineRendererDrawing>();

    private DynamicLineRendererDrawing dynamicLineRendererDrawing;

    void OnTriggerEnter(Collider other)
    {
        if ((collisionLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            for (int i = 0; i < laserList.Count; i++)
            {
                if (laserList[i] != null)
                {
                    dynamicLineRendererDrawing = laserList[i];
                    if(!dynamicLineRendererDrawing.IsActive)
                    {
                         dynamicLineRendererDrawing.IsActive = true;
                    }

                }
            }
        }
    }
}
