using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class initalises the player stats scriptable object with all the base stats.
/// This resets any weapon and stat changes and should be used at the beginning of the game.
/// </summary>
public class PlayerInitialiser : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] WeaponStats baseWeapon;
    [SerializeField] float baseSpeed;
    [SerializeField] float baseKnockback;
    [SerializeField] float baseReloadDelay;
    [SerializeField] float baseFiringDelay;
    [SerializeField] int baseGrenades;
    [SerializeField] int baseMedKits;
    private void Awake()
    {
        playerStats.CurrentHealth = playerStats.MaxHealth;
        playerStats.Speed = baseSpeed;
        playerStats.Knockback = baseKnockback;
        playerStats.ReloadDelay = baseReloadDelay;
        playerStats.FiringDelay = baseFiringDelay;
        playerStats.CurrentWeaponStats = baseWeapon;
        playerStats.CurrentMagAmmo = baseWeapon.MagSize;
        playerStats.ReserveAmmo = 0;
        playerStats.Grenades = baseGrenades;
        playerStats.MedKits = baseMedKits;
    }
}
