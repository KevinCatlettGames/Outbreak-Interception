using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class enables the player to launch grenades,
/// </summary>
public class PlayerGernadeLaucher : MonoBehaviour
{
    #region Variables

    [Tooltip("The Stats of the Player are needed to see if they player has any grenades left.")]
    [SerializeField] PlayerStats stats;

    [Tooltip("The grenade projectile itself.")]
    [SerializeField] GameObject grenade;

    [Tooltip("The position the grenade is launched from.")]
    [SerializeField] Transform launchPosition;

    [Tooltip("The time required to wait between throwing grenades.")]
    [SerializeField] float downtime;

    // A timer to check if the grenade is ready.
    float timer;

    #endregion

    #region Unity Methods

    /// <summary>
    /// The update is only used to count down the timer.
    /// </summary>
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    #endregion

    #region Input

    /// <summary>
    /// If the input is pressed and the player has grenades left a grenade is instantiated and its Launch() method is called.
    /// The Launch() method takes in a vector from the player towards the mouse position. 
    /// </summary>
    /// <param name="input"> Grenade input. </param>
    public void GrenadeInput(InputAction.CallbackContext input)
    {
        if (timer <= 0 && input.started && stats.Grenades > 0)
        {
            stats.Grenades--;
            GameObject newGrenade = Instantiate(grenade, launchPosition.position, Quaternion.identity);
            Grenade grenadeScript = newGrenade.GetComponent<Grenade>();
            grenadeScript.Launch(transform.position + PlayerMovement.AimVector);
            timer = downtime;
        }
    }

    #endregion
}
