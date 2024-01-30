using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatsSO", menuName = "ScriptableObjects/NewWeapon")]
public class WeaponStats : ScriptableObject
{
    [Header("Weapon Identifier")]
    [Tooltip("This Number must be the same as the index on the weapons List.")]
    [SerializeField] int weaponID;
    public int WeaponID { get { return weaponID; } }

    [Header("Layermask")]
    [Tooltip("Layers which are hit by the weapon.")]
    [SerializeField] LayerMask hittableObjects;
    public LayerMask HittableObjects { get { return hittableObjects; } }

    [Header("Weapon Stats")]
    [Tooltip("The amount of time the player cannot shoot between shoots.")]
    [SerializeField] float downTime;
    public float DownTime { get { return downTime; } }

    [Tooltip("The amount of time the player has to reload.")]
    [SerializeField] float reloadTime;
    public float ReloadTime { get { return reloadTime; } }

    [Tooltip("The amount of damage the weapon deals per bullet.")]
    [SerializeField] float damage;
    public float Damage { get { return damage; } }

    [Tooltip("The amount of yeet the weapon inflicts on the enemy.")]
    [SerializeField] float knockback;
    public float Knockback { get { return knockback; } }

    [Tooltip("The amount of random bullet spread the weapon has.")]
    [SerializeField] float spread;
    public float Spread { get { return spread; } }

    [Header("Ammo")]
    [Tooltip("The amount of bullets in one magazine.")]
    [SerializeField] int magSize;
    public int MagSize { get { return magSize; } }

    [Tooltip("The amount of bullets the player get when they pick up a new weapon.")]
    [SerializeField] int ammoPerDrop;
    public int AmmoPerDrop { get { return ammoPerDrop; } }

    [Header("Camera Shake")]
    [Tooltip("The amount of time the camera shakes after every shot.")]
    [SerializeField] float cameraShakeTime;
    public float CameraShakeTime { get { return cameraShakeTime; } }

    [Tooltip("The intesity of the camera shake after every shot.")]
    [SerializeField] float cameraShakeIntensity;
    public float CameraShakeIntensity { get { return cameraShakeIntensity; } }

    [Header("Sound")]
    // Sound
    [Tooltip("The shoot sound.")]
    [SerializeField] AudioClip shootSound;
    public AudioClip ShootSound { get { return shootSound; } }

    [Tooltip("The ready to shoot sound.")]
    [SerializeField] AudioClip readyToShootSound;
    public AudioClip ReadyToShootSound { get { return readyToShootSound; } }

    [Tooltip("The reload sound.")]
    [SerializeField] AudioClip reloadSound;
    public AudioClip ReloadSound { get { return reloadSound; } }


}
