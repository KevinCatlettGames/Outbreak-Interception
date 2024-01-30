using Unity.VisualScripting;
using UnityEngine;

public class PlayAnimationIfHealthPackIsAvailable : MonoBehaviour
{
    PlayerStats playerStats;
    Animator anim; 
    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().Stats;
        }

        anim = GetComponent<Animator>();    
    }

    private void Update()
    {
        if(playerStats != null)
        {
            if(playerStats.MedKits > 0)
                anim.SetBool("PlayAnimation", true);
            else
                anim.SetBool("PlayAnimation", false); 
        }

    }
}
