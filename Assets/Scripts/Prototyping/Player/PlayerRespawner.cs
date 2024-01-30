using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    WeaponStats weapon;
    private float health;
    private float speed;
    private float damage;
    private float knockback;
    private float reloadDelay;
    private float firingDelay;
    private int magAmmo;
    private int reserveAmmo;
    private int grenades;
    private int medKits;
    void Start()
    {
        weapon = playerStats.CurrentWeaponStats;
        health = playerStats.CurrentHealth;
        speed = playerStats.Speed;
        damage = playerStats.Damage;
        knockback = playerStats.Knockback;
        reloadDelay = playerStats.ReloadDelay;
        firingDelay = playerStats.FiringDelay;
        magAmmo = playerStats.CurrentMagAmmo;
        reserveAmmo = playerStats.ReserveAmmo;
        grenades = playerStats.Grenades;
        medKits = playerStats.MedKits;
    }

   public void Respawn()
    {
        playerStats.CurrentWeaponStats = weapon;

        if(health < playerStats.MaxHealth * .5f)
            playerStats.CurrentHealth = playerStats.MaxHealth * .5f;
        else
            playerStats.CurrentHealth = health;

        playerStats.Speed = speed;
        playerStats.Damage = damage;
        playerStats.Knockback = knockback;
        playerStats.ReloadDelay = reloadDelay;
        playerStats.FiringDelay = firingDelay;
        playerStats.CurrentMagAmmo = magAmmo;
        playerStats.ReserveAmmo = reserveAmmo;
        playerStats.Grenades = grenades;
        playerStats.MedKits = medKits;
    }
}
