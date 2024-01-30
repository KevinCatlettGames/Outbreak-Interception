using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class is responsible for holding all drops which the object is able drop. 
/// </summary>
public class DropHandler : MonoBehaviour
{
    [Header("List of drops")]
    [Tooltip("All drops in this list will be spawned once the Drop() method is called.")]
    [SerializeField] List<Drop> drops = new List<Drop>();

    /// <summary>
    /// When this method is called every drop in the list is spawned.
    /// </summary>
    public void Drop()
    {
        foreach (Drop drop in drops)
        {
            Instantiate(drop,transform.position,Quaternion.identity);
        }
    }
}
