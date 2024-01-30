using UnityEngine;
using TMPro;

public class ChangeTMProTextCountingUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int valueToChangeTo = 10;
    [SerializeField] float countWaitDuration = .05f;

    int currentValue = 0;
    bool activated;
    bool finished;
    float currentDuration;

    private void Update()
    {
        if(activated && !finished)
        {
            currentDuration += Time.deltaTime;
            if(currentDuration > countWaitDuration)
            {
                currentDuration = 0;
                currentValue++;
                text.text = currentValue.ToString() + " Left";
                if(currentValue >= valueToChangeTo)
                {
                    finished = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !activated)
        {
            activated = true;
        }
    }
}
