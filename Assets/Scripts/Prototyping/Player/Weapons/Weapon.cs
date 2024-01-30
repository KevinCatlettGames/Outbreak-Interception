using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the class which every weapon inherits from.
/// </summary>
public abstract class Weapon
{
    #region Variables

    // The Weapon nned the stats of the player for damage calculations and universal adjustments.
    protected PlayerStats playerStats;

    // The stats the weapon iself needs to function.
    protected WeaponStats weaponStats;

    // A vector which is used to calculate the spread if the weapon has some.
    protected Vector2 spreadVector;

    // The vector the bullet takes once it leaves the weapon.
    protected Vector3 shotDirectionMod;

    protected GameObject hitEffect;

    public Weapon(GameObject hitEffect)
    {
        this.hitEffect = hitEffect;
    }

    #endregion

    #region Equipment

    /// <summary>
    /// Method for equiping a new weapon.
    /// </summary>
    /// <param name="playerStats"> The player scriptable object needs the stats of the new weapon. </param>
    /// <param name="weaponStats"> The stats of the weapon itself. </param>
    /// <param name="isStaring"> When the player is enering (isStarting == true) a new scene they dont gain the equip ammo for the weapon. </param>
    public abstract void Equip(PlayerStats playerStats, WeaponStats weaponStats, bool isStaring);

    /// <summary>
    /// When a new weapon is equipped the remaining reserve ammo is converted for the new weapon.
    /// </summary>
    /// <returns> Transfered reserve ammo. </returns>
    public abstract int Drop();

    #endregion

    #region Shooting

    /// <summary>
    /// This method is called when the player pulls the trigger, then raycasts are used for the bullets.
    /// </summary>
    /// <param name="playerPosition"> The position of the player => the position the bullet is coming from. </param>
    /// <param name="shotDirection"> The direction the player is aiming to. </param>
    /// <returns> The amount of time the player has to wait before shooting again. </returns>
    public abstract float Shoot(Vector3 playerPosition, Vector3 shotDirection);

    /// <summary>
    /// This method is called when the player reloads manually or runs out of ammo. 
    /// If the weapon uses reserve ammo the player can only gain the amount they have left.
    /// </summary>
    /// <returns> The amount of time the weapon need to reload is returned. </returns>
    public abstract float Reload();

    #endregion
}
