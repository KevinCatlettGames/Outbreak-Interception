using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour 
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    public bool invounerable = false;
    public bool dashInounerable = false;
    bool isDead = false;


    public UnityEvent<Vector3, float> OnDamageEvent, OnDeathEvent;
    public UnityEvent OnHealEvent;

    public void Initialize(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void HealthChange(float change, float knockback, Vector3 hitDirection)
    {
        if (isDead == false && invounerable == false)
        {
            if (dashInounerable && knockback == 0 && change < 0)
            {
                return;
            }
            currentHealth += change;
            if (change >= 0)
            {
                if (currentHealth > maxHealth) currentHealth = maxHealth;
                OnHealEvent?.Invoke();
            }
            else
            {
                if (currentHealth <= 0)
                {
                    isDead = true;
                    OnDeathEvent?.Invoke(hitDirection,knockback);
                } 
                else OnDamageEvent?.Invoke(hitDirection,knockback);
            }
        }
        
    }
}
