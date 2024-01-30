using UnityEngine;
using UnityEngine.Playables;

public class ActivateTimelineOnDeath : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;

    [SerializeField] PlayableDirector director;


    private void Start()
    {
        if(GetComponent<HealthSystem>())
        {
            healthSystem = GetComponent<HealthSystem>();
            healthSystem.OnDeathEvent.AddListener(ActivateTimeline);
        }
    }

    void ActivateTimeline(Vector3 direction, float knockBack)
    {
        director.Play();
    }
}
