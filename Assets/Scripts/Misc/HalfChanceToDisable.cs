using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script destroys the gameobject it is attached to by a fifty percent chance.
/// </summary>
public class HalfChanceToDisable : MonoBehaviour
{
    void Awake()
    {
        int randomInt = (int)Random.Range(0, 2);
        if(randomInt == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
