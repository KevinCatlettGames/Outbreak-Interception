using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the LootBox interactions.
/// </summary>
public class LootBox : MonoBehaviour
{
    #region Variables

    [Header("Audio")]
    [Tooltip("AudioSource connected with the 'Drops' output.")]
    [SerializeField] AudioSource audioSource;

    [Tooltip("Sound which plays when the box is damaged.")]
    [SerializeField] AudioClip damageSound;

    [Tooltip("Sound which plays when the box is opened.")]
    [SerializeField] AudioClip openSound;

    // The Drop handler has the list of all drops inside.
    private DropHandler dropHandler;

    // The BoxCollider is neccecary to give the box a hitbox for the player to shoot.
    BoxCollider boxCollider;

    // This bool ensures that the box is only opened once.
    bool isOpened = false;

    #endregion

    #region Unity Methods

    private void Start()
    {
        dropHandler = GetComponent<DropHandler>();
        boxCollider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Once the loot box is opened and the open sound has finished playing the gameObject destroys itself.
    /// </summary>
    private void Update()
    {
        if (isOpened == true && audioSource.isPlaying == false)
            Destroy(gameObject);
    }

    #endregion

    #region Damage Interactions

    /// <summary>
    /// Plays the sound related to the damage event.
    /// </summary>
    public void Open()
    {
        boxCollider.enabled = false;
        isOpened = true;
        dropHandler.Drop();
        if (openSound != null )
            audioSource.PlayOneShot(openSound);
    }
    public void Damage()
    {
        if(damageSound != null)
            audioSource.PlayOneShot(damageSound);
        
    }

    #endregion
}
