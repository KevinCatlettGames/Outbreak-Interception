using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// This is the class for the basic pistol.
/// </summary>
public class BasicPistol : Weapon
{
    public BasicPistol(GameObject hitEffect) : base(hitEffect)
    {
    }
    #region Equipment

    /// <summary>
    /// When the pistol is equipped the player does not gain any reserve ammo since the pistol uses none.
    /// </summary>
    /// <param name="playerStats"> Player Stats SO. </param>
    /// <param name="weaponStats"> Weapon Stats SO. </param>
    /// <param name="isStarting"> True if the player is entering a new scene. </param>
    public override void Equip(PlayerStats playerStats, WeaponStats weaponStats, bool isStarting)
    {
        this.playerStats = playerStats;
        this.weaponStats = weaponStats;
        playerStats.CurrentWeaponStats = weaponStats;
        if (isStarting == false)
        {
            playerStats.ReserveAmmo = weaponStats.AmmoPerDrop;
            playerStats.CurrentMagAmmo = weaponStats.MagSize;
        }
    }

    /// <summary>
    /// The pistol doesnt return any reserve ammo when droped since the pistol uses none. 
    /// </summary>
    /// <returns> Returns 0 since ther is no reserve ammo if you have the pistol. </returns>
    public override int Drop()
    {
        return 0;
    }

    #endregion

    #region Shooting

    /// <summary>
    /// The pistol shoots one raycast, deals damage and knockback to any enemy hit, directly to where the player is aiming.
    /// Additionally the camera shakes and the amunition in the magazine is reduced.
    /// </summary>
    /// <param name="playerPosition"> Starting point of the raycast. </param>
    /// <param name="shotDirection"> Direction the player is aiming. </param>
    /// <returns> The amount of time the player has to wait before shooting again. </returns>
    public override float Shoot(Vector3 playerPosition, Vector3 shotDirection)
    {
        CameraShaker.instance.ShakeCamera(weaponStats.CameraShakeTime, weaponStats.CameraShakeIntensity);       
        RaycastHit hit;
        Physics.Raycast(playerPosition, shotDirection, out hit, 500, weaponStats.HittableObjects);
        Debug.DrawRay(playerPosition, shotDirection * 500, Color.red, 5);
        if (hit.collider != null)
        {
            GameObject.Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log(hit.transform.name);
            HealthSystem healthTarget;
            if (hit.collider.GetComponent<HealthSystem>() != null)
            {
                healthTarget = hit.collider.GetComponent<HealthSystem>();
                healthTarget.HealthChange(-playerStats.Damage * weaponStats.Damage, playerStats.Knockback * weaponStats.Knockback, shotDirection);
            }
        }
        playerStats.CurrentMagAmmo--;
        return weaponStats.DownTime * playerStats.FiringDelay;
    }

    /// <summary>
    /// Since the pistol doesnt use reserve ammo the player can alway reload it.
    /// </summary>
    /// <returns> The time it takes to reload. </returns>
    public override float Reload()
    {
        playerStats.CurrentMagAmmo = weaponStats.MagSize;
        Debug.Log("reloading");
        return weaponStats.ReloadTime * playerStats.ReloadDelay;
    }

    #endregion
}
