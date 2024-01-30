using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] MeshRenderer Pistol, Shotgun, SMG;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerWeaponHandler playerWeaponHandler;
    private Transform pistolMuzzle, shotgunMuzzle, smgMuzzle;
    Vector3 aimDirection;
    Vector3 movementDirection;

    float angle;
    int currentWeapon;
    void Awake()
    {   
        pistolMuzzle = Pistol.transform.GetChild(0).transform;
        shotgunMuzzle = Shotgun.transform.GetChild(0).transform;
        smgMuzzle = SMG.transform.GetChild(0).transform;
        EquipWeapon();
    }

    void Update()
    {
        aimDirection = PlayerMovement.AimVector;
        movementDirection = PlayerMovement.MoveVector;
        if (currentWeapon != playerStats.CurrentWeaponStats.WeaponID)
        {
            EquipWeapon();
        }
        if (movementDirection != Vector3.zero)
        {
            angle = Vector3.SignedAngle(aimDirection, movementDirection, Vector3.up);
            animator.SetBool("IsMoving",true);
            if (angle < -95 || angle > 95)
            {
                transform.rotation = Quaternion.LookRotation(-movementDirection, Vector3.up);
                animator.SetBool("isForwards", false);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                animator.SetBool("isForwards", true);
            }
        }
        else
        {
            angle = Vector3.SignedAngle(transform.forward, aimDirection, Vector3.up);
            if (angle > 45f)
            {

                transform.Rotate(Vector3.up, 45f);

            }
            else if (angle < -45f)
            {
                transform.Rotate(Vector3.up, -45f);
            }
            animator.SetBool("IsMoving", false);
        }
    }

    private void EquipWeapon()
    {
        switch (playerStats.CurrentWeaponStats.WeaponID)
        {
            case 0:
                Pistol.enabled = true;
                Shotgun.enabled = false;
                SMG.enabled = false;
                playerWeaponHandler.MuzzleflashPosition = pistolMuzzle;
                animator.SetBool("hasPistol", true);
                break; 
            case 1:
                Pistol.enabled = false;
                Shotgun.enabled = true;
                SMG.enabled = false;
                animator.SetBool("hasPistol", false);
                playerWeaponHandler.MuzzleflashPosition = shotgunMuzzle;
                break; 
            case 2:
                Pistol.enabled = false;
                Shotgun.enabled = false;
                SMG.enabled = true;
                animator.SetBool("hasPistol", false);
                playerWeaponHandler.MuzzleflashPosition = smgMuzzle;
                break;
        }
        currentWeapon = playerStats.CurrentWeaponStats.WeaponID;
    }
}
