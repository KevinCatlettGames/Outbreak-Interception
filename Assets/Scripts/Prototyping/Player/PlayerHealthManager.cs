using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class deals with the interaction between the healthSystem and the player.
/// </summary>
public class PlayerHealthManager : MonoBehaviour
{
    #region Variables

    [Header("Player Stats Scriptable Object")]
    [Tooltip("The player stats scriptable object that will be used to store all the values that are needed for the enemy to function.")]
    [SerializeField] private PlayerStats stats;

    [Header("Audio")]
    [Tooltip("AudioSource connected with the 'Grunts' output.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Sound which plays when the Player is damaged.")]
    [SerializeField] private AudioClip damageSound;

    [Tooltip("Sound which plays when the Player is killed.")]
    [SerializeField] private AudioClip deathSound;

    private HealthSystem playerHealthSystem;

    private PlayerMovement playerMovement;

    private PlayerWeaponHandler playerWeaponHandler;
    #endregion Variables

    #region Initialise
    private void Start()
    {
        playerHealthSystem = GetComponent<HealthSystem>();
        playerMovement = GetComponent<PlayerMovement>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();  
        Initalise();
    }
    /// <summary>
    /// Updates the stats from the players HealthSystem to reflect whats in the Scriptable object of the player.
    /// </summary>
    public void Initalise()
    {
        playerHealthSystem.Initialize(stats.CurrentHealth, stats.MaxHealth);
    }

    public void HealInput(InputAction.CallbackContext input)
    {
        if (stats.MedKits > 0 && input.started)
        {
            playerHealthSystem.HealthChange(stats.MaxHealth * 0.5f, 0, Vector3.zero);
            stats.MedKits--;
        }
    }

    #endregion

    #region Damage Interactions
    /// <summary>
    /// Plays the sound related to the damage event.
    /// </summary>
    public void OnDamage()
    {
        audioSource.PlayOneShot(damageSound);
    }
    public void OnDeath()
    {
        playerMovement.enabled = false;
        playerWeaponHandler.enabled = false;
        audioSource.PlayOneShot(deathSound);
    }
    #endregion

    #region Update Connected Systems
    /// <summary>
    /// This is called whenever the healthSystem registers a health change and updates the player scriptable object with the new values.
    /// </summary>
    public void UpdateSO()
    {
        stats.CurrentHealth = playerHealthSystem.GetCurrentHealth();
    }
    #endregion
}