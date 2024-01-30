using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the class for the SMG.
/// </summary>
public class SMG : Weapon
{
    #region Variables

    // The current spread applied to each raycast.
    float currentSpread;
    // The rate at which spread increases for each bullet.
    float spreadRate;
    // How much time has passed since the last shot to potentially reset the spread.
    float timeAtLastShot;
    // The time the player has to stop shooting for the spread to go back to 0.
    float spreadResetTime = 1f;

    public SMG(GameObject hitEffect) : base(hitEffect)
    {
    }

    #endregion

    #region Equipment

    /// <summary>
    /// The SMG starts with the equivalent of 3 ammodrops of ammo when equiped.
    /// </summary>
    /// <param name="playerStats"> Player Stats SO. </param>
    /// <param name="weaponStats"> Weapon Stats SO. </param>
    /// <param name="isStarting"> True if the player is entering a new scene, if true the player doesnt gain the equip ammo. </param>
    public override void Equip(PlayerStats playerStats, WeaponStats weaponStats, bool isStarting)
    {
        this.playerStats = playerStats;
        this.weaponStats = weaponStats;
        playerStats.CurrentWeaponStats = weaponStats;
        if (isStarting == false)
        {
            playerStats.ReserveAmmo = weaponStats.AmmoPerDrop * 3;
            playerStats.CurrentMagAmmo = weaponStats.MagSize;
        }
        spreadRate = 0.075f * weaponStats.Spread;
    }

    /// <summary>
    /// Any additional ammo over the staring ammo is devided into ammodrops and then returned for the new weapon to gain. 
    /// </summary>
    /// <returns> Returns the amount of ammodrops over the starting ammo. </returns>
    public override int Drop()
    {
        int bonusAmmo = playerStats.ReserveAmmo - (weaponStats.AmmoPerDrop * 3);
        Debug.Log(bonusAmmo);
        if (bonusAmmo > 0)
            bonusAmmo /= weaponStats.AmmoPerDrop;
        else if (bonusAmmo < 0)
            bonusAmmo = 0;
        Debug.Log(bonusAmmo);
        return bonusAmmo;
    }

    #endregion

    #region Shooting

    /// <summary>
    /// The Shoot() method of the SMG calls the ApplySpread() method if the player has recently shot.
    /// This spred increases the longe the player shoots without taking a break.
    /// </summary>
    /// <param name="playerPosition"> Starting point of the raycast. </param>
    /// <param name="shotDirection"> Direction the player is aiming. </param>
    /// <returns> The amount of time the player has to wait before shooting again. </returns>
    public override float Shoot(Vector3 playerPosition, Vector3 shotDirection)
    {
        if (playerStats.CurrentMagAmmo > 0)
        {
            if (Time.time - timeAtLastShot >= spreadResetTime)
            {
                currentSpread = 0;
            }
            else if(currentSpread < weaponStats.Spread)
            {
                currentSpread += spreadRate;
                if (currentSpread > weaponStats.Spread)
                {
                    currentSpread = weaponStats.Spread;
                }
            }
            if (currentSpread < 0)
            {
                shotDirectionMod = ApplySpread(shotDirection, currentSpread);
            }
            else
            {
                shotDirectionMod = shotDirection;
            }
            RaycastHit hit;
            Physics.Raycast(playerPosition, shotDirectionMod, out hit, 500, weaponStats.HittableObjects);
            Debug.DrawRay(playerPosition, shotDirectionMod * 500, Color.red, 5);
            if (hit.collider != null)
            {
                GameObject.Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                HealthSystem healthTarget;
                if (hit.collider.GetComponent<HealthSystem>() != null)
                {
                    healthTarget = hit.collider.GetComponent<HealthSystem>();
                    healthTarget.HealthChange(-playerStats.Damage * weaponStats.Damage, playerStats.Knockback * weaponStats.Knockback, shotDirectionMod);
                }
            }
            CameraShaker.instance.ShakeCamera(weaponStats.CameraShakeTime, weaponStats.CameraShakeIntensity);
            playerStats.CurrentMagAmmo--;
            timeAtLastShot = Time.time;
        }
        return weaponStats.DownTime * playerStats.FiringDelay;
    }

    /// <summary>
    /// This method applies the current spread to the bullet vector.
    /// </summary>
    /// <param name="direction"> Direction the player is aiming. </param>
    /// <param name="spread"> The current spread for the bullet. </param>
    /// <returns> A vector dajusted with the current spread. </returns>
    private Vector3 ApplySpread(Vector3 direction,float spread)
    {
        spreadVector = Random.insideUnitCircle;
        spreadVector *= spread;
        direction.x += spreadVector.x;
        direction.z += spreadVector.y;
        return direction;
    }

    /// <summary>
    /// This method is called when the player reloads manually or runs out of ammo. 
    /// If the weapon uses reserve ammo the player can only gain the amount they have left.
    /// </summary>
    /// <returns> The amount of time the weapon need to reload is returned. </returns>
    public override float Reload()
    {
        for (int i = playerStats.CurrentMagAmmo - weaponStats.MagSize; i < 0; i++)
        {
            if (playerStats.ReserveAmmo > 0)
            {
                playerStats.ReserveAmmo--;
                playerStats.CurrentMagAmmo++;
            }
            else
            {
                break;
            }
        }
        return weaponStats.ReloadTime* playerStats.ReloadDelay;
    }

    #endregion
}
