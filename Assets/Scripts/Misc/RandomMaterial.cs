using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField] Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        int randomInt = Random.Range(0,materials.Length);

        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material = materials[randomInt];
        }
    }

}
