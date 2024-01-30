using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This is the class for the shotgun.
/// </summary>
public class Shotgun : Weapon
{
    // The amount of raycasts per shot input
    private int pellets = 6;



    public Shotgun(GameObject hitEffect) : base(hitEffect)
    {

    }

    #region Equipment

    /// <summary>
    /// The Shotgun starts with the equivalent of 3 ammodrops of ammo when equiped.
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
    }

    /// <summary>
    /// Any additional ammo over the staring ammo is devided into ammodrops and then returned for the new weapon to gain. 
    /// </summary>
    /// <returns> Returns the amount of ammodrops over the starting ammo. </returns>
    public override int Drop()
    {
        int bonusAmmo = playerStats.ReserveAmmo - (weaponStats.AmmoPerDrop * 3);
        if (bonusAmmo > 0)
        bonusAmmo /= weaponStats.AmmoPerDrop;
        else if (bonusAmmo < 0)
            bonusAmmo = 0;
        return bonusAmmo;
    }

    #endregion

    #region Shooting

    /// <summary>
    /// The Shoot() method of the shotgun calls the ShootPellet() method for each pellet. 
    /// This results in multiple raycast each with slightly adjusted direction.
    /// </summary>
    /// <param name="playerPosition"> Starting point of the raycast. </param>
    /// <param name="shotDirection"> Direction the player is aiming. </param>
    /// <returns> The amount of time the player has to wait before shooting again. </returns>
    public override float Shoot(Vector3 playerPosition, Vector3 shotDirection)
    {
        CameraShaker.instance.ShakeCamera(weaponStats.CameraShakeTime, weaponStats.CameraShakeIntensity);
        spreadVector = Vector2.zero;
        for (int i = 0; i <= pellets; i++)
        {
            ShootPellet(playerPosition, shotDirection);
        }
        playerStats.CurrentMagAmmo--;
        return weaponStats.DownTime * playerStats.FiringDelay;
    }

    /// <summary>
    /// One raycast is shot then the vector is adjusted to include the spread.
    /// This makes the first pellet always accurate.
    /// </summary>
    /// <param name="playerPosition"> Starting point of the raycast. </param>
    /// <param name="shotDirection"> Direction the player is aiming. </param>
    private void ShootPellet(Vector3 playerPosition, Vector3 shotDirection)
    {
        shotDirectionMod = shotDirection;
        shotDirectionMod.x += spreadVector.x;
        shotDirectionMod.z += spreadVector.y;
        RaycastHit hit;
        Physics.Raycast(playerPosition, shotDirectionMod, out hit, 500, weaponStats.HittableObjects);
        Debug.DrawRay(playerPosition, shotDirectionMod * 500,Color.red,5);
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
        // The spread only kick in after the first pellet is shot, that way 1 pellet will always be perfectly accurate.
        spreadVector = Random.insideUnitCircle;
        spreadVector *= weaponStats.Spread;
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
        return weaponStats.ReloadTime * playerStats.ReloadDelay;
    }

    #endregion
}



