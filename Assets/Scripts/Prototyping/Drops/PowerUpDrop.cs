using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDrop : Drop
{
    [SerializeField] float hpUp;
    [SerializeField] float spdUp;
    [SerializeField] float dmgUp;
    [SerializeField] float knkUp;
    [SerializeField] float rldDwn;
    [SerializeField] float frtDwn;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pickupTrigger = GetComponent<SphereCollider>();
        bodyCollider = GetComponent<BoxCollider>();
        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);
        dropVector3D = Random.insideUnitSphere;
        dropVector3D *= lauchStrengh;
        Debug.Log(dropVector3D);
        rb.AddForce(dropVector3D, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int rnd = Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    playerStats.Speed += spdUp;
                    //speed up
                    break;
                case 1:
                    playerStats.Damage += dmgUp;
                    playerStats.Knockback += knkUp;
                    break;
                case 2:
                    playerStats.MaxHealth += hpUp;
                    playerStats.CurrentHealth = playerStats.MaxHealth;
                    PlayerHealthManager playerHealthManager= other.GetComponent<PlayerHealthManager>();
                    playerHealthManager.Initalise();
                    break;
                case 3:
                    playerStats.ReloadDelay -= rldDwn;
                    playerStats.FiringDelay -= frtDwn;
                    break;

            }
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound);
            isPickedUp = true;
        }
    }

    private void Update()
    {
        if (isPickedUp == true && audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.isKinematic = true;
            bodyCollider.enabled = false;
        }
    }

    
}
