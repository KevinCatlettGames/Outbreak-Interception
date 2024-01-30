using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InvokeUnityEventOnEnemyAmount : MonoBehaviour
{
    [Tooltip("At what enemy amount should the event be invoked?")]
    [SerializeField] int invokationAmount;
    public UnityEvent eventToInvoke;
    EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = EnemyManager.Instance;
        Invoke("SubscribeToEvent", 10f);
    }

    void SubscribeToEvent()
    {
        enemyManager.enemyAmountUpdated += CheckIfInvoke;
    }

    void CheckIfInvoke(int currentAmount)
    {
        Debug.Log("Checking for invoke");
        if(currentAmount <= invokationAmount) 
        {
            eventToInvoke.Invoke();
        }
    }
}
