using UnityEngine;
using UnityEngine.UI;

public class ChangeSliderValueOnDamaged : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] Slider healthSlider;
    [SerializeField] bool disableSliderOnEmpty; 
    private void Start()
    {
        if (GetComponent<HealthSystem>())
        {
            healthSystem = GetComponent<HealthSystem>();
            healthSystem.OnDamageEvent.AddListener(ChangeSliderValue);
            healthSystem.OnDeathEvent.AddListener(DisableSlider);
            healthSlider.maxValue = healthSystem.GetCurrentHealth();
            ChangeSliderValue(Vector3.zero, 0);
        }    
    }

    void ChangeSliderValue(Vector3 direction, float knockBack)
    {
        healthSlider.value = healthSystem.GetCurrentHealth(); 
    }

    void DisableSlider(Vector3 direction, float knockBack)
    {
        if(disableSliderOnEmpty)
        healthSlider.gameObject.SetActive(false);
    }
}
