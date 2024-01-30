using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The EnemyManager is a singleton that handles herd movement of the enemies and keeps track of the amoung of enemies in the scene.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    #region Variables
    // Singleton of this class.
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }


    [Tooltip("The radius around the target that the enemies will circle around.")]
    [SerializeField] float radiusAroundTarget = .5f;
    public float RadiusAroundTarget { get { return radiusAroundTarget; } }

    [Tooltip("The time until the enemies target positions will be updated.")]
    [SerializeField] float waitDuration = .2f;
    public float WaitDuration { get { return waitDuration; } }


    [SerializeField] bool UpdateUI = true;

    // List of all enemies in range of the target.
    List<EnemyEntity> enemiesChasingTarget = new List<EnemyEntity>();
    public List<EnemyEntity> EnemiesInTargetRange { get { return enemiesChasingTarget; } }

    // List of all enemies in the scene.
    List<EnemyEntity> enemiesInScene = new List<EnemyEntity>();
    public List<EnemyEntity> EnemiesInScene { get { return enemiesInScene; } }


    Transform enemyTarget;
    public Transform EnemyTarget { get { return enemyTarget; } }

    float currentWaitDuration;

    bool allEnemiesDefeatedEventInvoked;

    public Action<int> enemyAmountUpdated;
    bool enemyAmountUpdateInitialized;

    public Action OnAllEnemiesDefeated;
    #endregion

    #region Unity Methods
    void Awake()
    {
        if(instance == null) 
            instance = this;
        else
            Destroy(gameObject);

        if (GameObject.FindGameObjectWithTag("EnemyFocusPoint") != null)
            enemyTarget = GameObject.FindGameObjectWithTag("EnemyFocusPoint").transform;

        currentWaitDuration = waitDuration;       

        Invoke("CheckSceneClearCondition", 5f);
    }

    void Update()
    {
        if(!enemyAmountUpdateInitialized)
        {
            enemyAmountUpdateInitialized = true;
            enemyAmountUpdated?.Invoke(enemiesInScene.Count);
        }
        SetTargetTimer();       
    }
    #endregion

    #region Methods
    /// <summary>
    /// Adds the enemy to the enemy list.
    /// </summary>
    /// <param name="enemy"></param> Thw enemy to add.
    public void AddToEnemiesInScene(EnemyEntity enemy)
    {
        enemiesInScene.Add(enemy);
        if (UpdateUI)
        {
            enemyAmountUpdated?.Invoke(enemiesInScene.Count);
        }
    }

    /// <summary>
    /// Removes the enemy from the enemy list. 
    /// </summary>
    /// <param name="enemy"></param> The enemy to remove.
    public void RemoveFromEnemiesInScene(EnemyEntity enemy)
    {
        enemiesInScene.Remove(enemy);
        if (UpdateUI)
        {
            enemyAmountUpdated?.Invoke(enemiesInScene.Count);
        }
        CheckSceneClearCondition();
    }

    /// <summary>
    /// Sets the enemies target positions once the timer is at or below 0. 
    /// </summary>
    void SetTargetTimer()
    {
        currentWaitDuration -= Time.deltaTime;
        if (currentWaitDuration <= 0)
        {
            currentWaitDuration = waitDuration;
            PositionEnemiesAroundTarget();
        }
    }

    /// <summary>
    /// Sets the target positions of all enemies that are currently chasing the player.
    /// </summary>
    void PositionEnemiesAroundTarget()
    {
        for (int i = 0; i < enemiesChasingTarget.Count; i++)
        {
            if (i <= 0)
            {
                enemiesChasingTarget[i].SetChaseTargetPosition(new Vector3(
                enemyTarget.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / enemiesChasingTarget.Count),
                enemyTarget.position.y,
                enemyTarget.position.z + RadiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / enemiesChasingTarget.Count)));          
            }

            else
            {
                enemiesChasingTarget[i].SetChaseTargetPosition(enemyTarget.position);
            }
        }
    }

    /// <summary>
    /// Invokes the OnAllEnemiesDefeated event if all enemies have been defeated.
    /// </summary>
    void CheckSceneClearCondition()
    {
        if (enemiesInScene.Count <= 0 && !allEnemiesDefeatedEventInvoked)
        {
            OnAllEnemiesDefeated?.Invoke();
            allEnemiesDefeatedEventInvoked = true;
        }
    }

    /// <summary>
    /// Finds a random position that is on the NavMesh within a certain distance from the origin.
    /// </summary>
    /// <param name="origin"></param> The origin position.
    /// <param name="distance"></param> The distance from the origin position.
    /// <param name="layermask"></param> The layermask to use when sampling the NavMesh.
    /// <returns></returns>
    public Vector3 RandomNavSpere(Vector3 origin, float distance, int layermask)
    {
        bool navPosFound = false;
        while (!navPosFound)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
            randomDirection += origin;

            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

            if (navHit.position != Vector3.zero)
            {
                navPosFound = true;
                return navHit.position;
            }
        }

        return Vector3.zero;
    }

    #endregion
}
