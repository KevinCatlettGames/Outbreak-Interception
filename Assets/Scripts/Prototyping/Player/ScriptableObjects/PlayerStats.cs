using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStatsSO")]
public class PlayerStats : ScriptableObject
{
    public float Speed;

    public float MaxHealth;
    public float CurrentHealth;

    public WeaponStats CurrentWeaponStats;

    public float Damage;
    public float Knockback;

    public float FiringDelay;
    public float ReloadDelay;

    public int ReserveAmmo;
    public int CurrentMagAmmo;

    public int Grenades;
    public int MedKits;
}
