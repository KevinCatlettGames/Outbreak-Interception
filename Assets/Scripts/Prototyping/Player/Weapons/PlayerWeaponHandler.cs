using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// This class connects the weapons to the given inputs and calls on the related methods.
/// </summary>
public class PlayerWeaponHandler : MonoBehaviour
{
    #region Variables

    [Tooltip("The default weapon is the Pistol.")]
    [SerializeField] WeaponStats defaultWeaponStats;

    [Tooltip("The Player Stats Scriptable Object.")]
    [SerializeField] PlayerStats playerStats;

    [Tooltip("The UI element which shows the ammo amounts.")]
    [SerializeField] TextMeshProUGUI ammoCounter;

    [Tooltip("The AudioSource connected to the 'Weapons' output.")]
    [SerializeField] AudioSource weaponSound;

    [Tooltip("The animator connected to the playermodel.")]
    [SerializeField] Animator animator;

    [Tooltip("The muzzle flash particle prefab.")]
    [SerializeField] GameObject muzzleFlash;

    [Tooltip("The muzzle flash particle prefab for the shotgun.")]
    [SerializeField] GameObject muzzleFlashShotgun;

    [Tooltip("The hit Effect particle prefab.")]
    [SerializeField] GameObject hitEffet;

    [Tooltip("Target the torso will be facing.")]
    [SerializeField] Transform aimTarget;

    // The position where the muzzleflash occurs.
    public Transform MuzzleflashPosition;

    // The transform y coordinate has to be incresed to 1 so the player doesnt shoot inside the floor.
    private Vector3 transformAdjusted;

    // The aimvector has to be normalised to make spred consistent.
    private Vector3 aimAdjusted;

    // All weapons available to the player.
    private List<Weapon> weapons;

    // The weapon the player currently has equipped in order to call the correct methodes.
    private Weapon currentWeapon;

    // The stats connected to the currently equipped weapon.
    private WeaponStats currentWeaponStats;

    // A timer used for the delay between shots.
    private float downTimer = 0;

    // A time used to prevent the player from shooting while reloading.
    private float reloadTimer = 0;

    // This bool is true while the shoot input is pressed to allow continuous shooting.
    private bool isShooting;

    // This bool prevents the player from gaining equip bonus ammo when entering a new scene.
    private bool isStarting;

    #endregion

    #region Unity Methods

    /// <summary>
    /// The weapons list is filled with the weapons and the current weapon from the player SO is equipped.
    /// </summary>
    private void Start()
    {
        weapons = new List<Weapon>();
        Weapon basicPistol = new BasicPistol(hitEffet);
        weapons.Add(basicPistol);
        Weapon shotgun = new Shotgun(hitEffet);
        weapons.Add(shotgun);
        Weapon smg = new SMG(hitEffet);
        weapons.Add(smg);

        isStarting = true;
        EquipWeapon(playerStats.CurrentWeaponStats);
        isStarting = false;
    }

    /// <summary>
    /// Update counts down all current timers and executes the reload and shoot methods if the input is given.
    /// Additionally if the player runs out of ammo for any weapon exept the pistol they reequip the pistol.
    /// </summary>
    private void Update()
    {
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                UpdateAmmoCounter();
                if (currentWeaponStats.ReadyToShootSound != null)
                    weaponSound.PlayOneShot(currentWeaponStats.ReadyToShootSound);
            }
        }
        if (downTimer > 0)
        {
            downTimer -= Time.deltaTime;
            if (downTimer <= 0 && reloadTimer <= 0)
            {
                if (currentWeaponStats.ReadyToShootSound != null)
                    weaponSound.PlayOneShot(currentWeaponStats.ReadyToShootSound);
            }
        }
        if (isShooting && reloadTimer <= 0 && downTimer <= 0) 
        {
            transformAdjusted = transform.position;
            transformAdjusted.y = 1.2f;
            if (MuzzleflashPosition != null && muzzleFlash != null)
            {           
                if (currentWeaponStats.WeaponID == 1)
                {
                    Instantiate(muzzleFlashShotgun, MuzzleflashPosition.position, Quaternion.LookRotation(aimTarget.position - MuzzleflashPosition.position), MuzzleflashPosition);
                }
                else
                {
                    Instantiate(muzzleFlash, MuzzleflashPosition.position, Quaternion.LookRotation(aimTarget.position - MuzzleflashPosition.position), MuzzleflashPosition);
                }
            }
            aimAdjusted = PlayerMovement.AimVector.normalized;
            downTimer = currentWeapon.Shoot(transformAdjusted, aimAdjusted);
            if (currentWeaponStats.ShootSound != null)
                weaponSound.PlayOneShot(currentWeaponStats.ShootSound);
            UpdateAmmoCounter();
            if (currentWeaponStats.WeaponID != 0 && playerStats.ReserveAmmo == 0 && playerStats.CurrentMagAmmo == 0)
            {
                DefaultWeapon();
            }
            if (playerStats.CurrentMagAmmo == 0)
            {
                reloadTimer = currentWeapon.Reload();
                animator.SetTrigger("Reload");
                if (currentWeaponStats.ReloadSound != null)
                    weaponSound.PlayOneShot(currentWeaponStats.ReloadSound);
            }
        }
    }

    #endregion

    #region ShootingInputs

    /// <summary>
    /// Sets the isShooting bool accoring to the given input.
    /// </summary>
    /// <param name="input"></param>
    public void ShootInput(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            isShooting = true;
        }
        else if (input.canceled)
        {
            isShooting = false;
        }
    }

    /// <summary>
    /// If the input is pressed, the player isnt already reloading 
    /// and the current magazine isnt full this method calls the reload function of the current weapon.
    /// </summary>
    /// <param name="input"></param>
    public void ReloadInput(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            if (reloadTimer <= 0 && playerStats.CurrentMagAmmo < currentWeaponStats.MagSize)
            {
                animator.SetTrigger("Reload");
                reloadTimer = currentWeapon.Reload();
                if (currentWeaponStats.ReloadSound != null)
                    weaponSound.PlayOneShot(currentWeaponStats.ReloadSound);
            }
        }    
    }

    #endregion

    #region WeaponManagement

    /// <summary>
    /// The current weapon is droped, any exess ammo converted and then the new weapon is equipped.
    /// The isStarting bool prevents the player from gaining ammo when entering a new scene.
    /// </summary>
    /// <param name="weaponToEquip"> The stats of the weapon to be equipped. </param>
    public void EquipWeapon(WeaponStats weaponToEquip)
    {
        int bonusAmmo = 0;
        if (isStarting == false) 
        {
            bonusAmmo = currentWeapon.Drop();          
            if (currentWeaponStats.ReadyToShootSound != null)
                weaponSound.PlayOneShot(currentWeaponStats.ReadyToShootSound);
        }
        currentWeapon = weapons[weaponToEquip.WeaponID];
        currentWeaponStats = weaponToEquip;
        currentWeapon.Equip(playerStats,weaponToEquip,isStarting);
        if (bonusAmmo > 0)
        {
            for (int i = 0; i < bonusAmmo; i++)
            {
                PickUpAmmo();
            }
        }
        UpdateAmmoCounter();
    }

    /// <summary>
    /// This method is called whenever the player is set back to the basic pistol.
    /// </summary>
    private void DefaultWeapon()
    {
        EquipWeapon(defaultWeaponStats);
    }

    /// <summary>
    /// This method is called when the player picks up an ammo drop and adds the ammo to the reserve.
    /// </summary>
    public void PickUpAmmo()
    {
        playerStats.ReserveAmmo += currentWeaponStats.AmmoPerDrop;
        UpdateAmmoCounter();
    }

    /// <summary>
    /// This methos updates the UI to always refect the current weapon and ammo of the palyer.
    /// </summary>
    private void UpdateAmmoCounter()
    {
        if (ammoCounter != null)
        {
            if (currentWeaponStats.WeaponID == 0)
            {
                ammoCounter.SetText($"Ammo: {playerStats.CurrentMagAmmo}/{currentWeaponStats.MagSize} <br>Reserve: --");
            }
            else
            {
                ammoCounter.SetText($"Ammo: {playerStats.CurrentMagAmmo}/{currentWeaponStats.MagSize} <br>Reserve: {playerStats.ReserveAmmo}");
            }
        }
    }

    #endregion
}
