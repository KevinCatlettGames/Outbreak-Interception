using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyOnEmptyList : MonoBehaviour
{
    [ReadOnly] public List<GameObject> list;

    bool initialCheck;

    [SerializeField] AudioSource audioS;
    [SerializeField] AudioClip audioC;
    [SerializeField] GameObject objectToDestroy;
    private void Awake()
    {
        list = new List<GameObject>();
    }
    private void Update()
    {
    }


    public void AddToList(GameObject objectToAdd)
    {
        list.Add(objectToAdd);
    }

    public void RemoveFromList(GameObject objectToRemove)
    {

        foreach(GameObject g in list)
        {
            g.GetComponent<ExplodingBarrel>().OnDeath();
        }
        Destroy();
    }

    void Destroy()
    {
        if (list.Count < 1)
        {
            if (audioS && audioC)
            {
                audioS.PlayOneShot(audioC);
            }
        }
        Destroy(objectToDestroy);
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject, 4f);
    }
}
