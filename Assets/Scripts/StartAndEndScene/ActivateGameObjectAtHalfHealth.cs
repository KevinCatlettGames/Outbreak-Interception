using UnityEngine;
using UnityEngine.Playables;

public class ActivateGameObjectAtHalfHealth : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;

    [SerializeField] GameObject objectToActivate;

    float maxHealth;

    private void Start()
    {
        if (GetComponent<HealthSystem>())
        {
            healthSystem = GetComponent<HealthSystem>();
            maxHealth = healthSystem.GetCurrentHealth();
            healthSystem.OnDamageEvent.AddListener(ActivateAtHalfHealth);
        }
    }

    void ActivateAtHalfHealth(Vector3 direction, float knockBack)
    {
        if(objectToActivate.activeSelf)
        {
            healthSystem.OnDamageEvent.RemoveListener(ActivateAtHalfHealth);
        }

        if(healthSystem.GetCurrentHealth() <= maxHealth * .5f)
        {
            objectToActivate.SetActive(true);
            healthSystem.OnDamageEvent.RemoveListener(ActivateAtHalfHealth);
        }
    }
}
