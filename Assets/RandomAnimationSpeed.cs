using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationSpeed : MonoBehaviour
{
    private void Awake()
    {
        Transform[] objChild = gameObject.transform.GetComponentsInChildren<Transform>();
        for (var i = 0; i < objChild.Length; i++)
        {
            if (objChild[i].GetComponent<Animator>())
            {
                objChild[i].GetComponent<Animator>().speed = Random.Range(.5f, 2);
            }
        }
    }
}
