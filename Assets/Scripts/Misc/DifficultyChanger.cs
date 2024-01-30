using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyChanger : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] float easyHealth;
    [SerializeField] float hardHealth;
    [SerializeField] float easyDamage;
    [SerializeField] float hardDamage;
    float currentHealthProportion;
    private void Awake()
    {

    }

    public void ChangeDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                currentHealthProportion = playerStats.CurrentHealth / playerStats.MaxHealth;
                playerStats.MaxHealth = easyHealth;
                playerStats.Damage = easyDamage;
                playerStats.CurrentHealth = currentHealthProportion * easyHealth;
                break;
            case 1:
                currentHealthProportion = playerStats.CurrentHealth / playerStats.MaxHealth;
                playerStats.MaxHealth = easyHealth;
                playerStats.Damage = hardDamage;
                playerStats.CurrentHealth = currentHealthProportion * easyHealth;
                break;
            case 2:
                currentHealthProportion = playerStats.CurrentHealth / playerStats.MaxHealth;
                playerStats.MaxHealth = hardHealth;
                playerStats.Damage = hardDamage;
                playerStats.CurrentHealth = currentHealthProportion * hardHealth;
                break;
        }
    }
}
