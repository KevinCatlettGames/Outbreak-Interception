using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEntity : MonoBehaviour
{
    public GameObject[] possibleEntities;

    private void Awake()
    {
        int randomInt = Random.Range(0,possibleEntities.Length);
        possibleEntities[randomInt].SetActive(true);
    }

}
