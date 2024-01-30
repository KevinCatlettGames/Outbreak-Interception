using UnityEngine;

/// <summary>
/// Makes the enemy roam around the map.
/// </summary>
public class EnemyRoamState : State
{
    // The amount of time the entity will roam before switching to another state.
    float roamTime;

    // The position the entity will roam to.
    Vector3 roamPosition;
    Vector3 startPosition;
    // The entity that is using this state.
    EnemyEntity enemyEntity;

    /// <summary>
    /// Creates a new instance of the EnemyRoamState class.
    /// </summary>
    /// <param name="stateColor"></param> The color of the objects material while in this state.
    /// <param name="entity"></param> The entity this state is attached to.
    public EnemyRoamState(Entity entity) : base(entity)
    {
        this.enemyEntity = entity as EnemyEntity;
        roamTime = Random.Range(enemyEntity.Values.MinRoamTime, enemyEntity.Values.MaxRoamTime);
        startPosition = enemyEntity.transform.position;
    }

    public override void EnterState()
    {
        InitializeValues();

        // base.EnterState();       
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        roamTime -= Time.deltaTime;

        // base.UpdateState();
    }

    public override void CheckSwitchState()
    {
        if (enemyEntity.Dead)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.DeathState); // Death State
        }
        else if (enemyEntity.Target && Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.position) < enemyEntity.Values.AttackDistance && enemyEntity.IsPlayerInSight())
            enemyEntity.StateMachine.SetState(enemyEntity.AttackState);

        else if (enemyEntity.CanChase && enemyEntity.Target && Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.transform.position) < enemyEntity.Values.ChaseDistance && enemyEntity.IsPlayerInSight())
        {
            enemyEntity.PerformStateEffect(null, enemyEntity.Values.AggroClip[Random.Range(0, enemyEntity.Values.AggroClip.Length -1)]);
            enemyEntity.StateMachine.SetState(enemyEntity.ChaseState);
        }

        else if (roamTime < 0 || Vector3.Distance(enemyEntity.SightStartPosition.position, enemyEntity.Agent.destination) < 1)
            enemyEntity.StateMachine.SetState(enemyEntity.IdleState);

        // base.CheckSwitchState();
    }

    #region Methods
    /// <summary>
    /// Sets a roamTime and finds a roamPosition. 
    /// Enables agent movement and changes the materials color on the entity gameObject. 
    /// </summary>
    void InitializeValues()
    {
        roamTime = Random.Range(enemyEntity.Values.MinRoamTime, enemyEntity.Values.MaxRoamTime);

        roamPosition = enemyEntity.transform.position;

        if (enemyEntity.LastPlayerLocation == Vector3.zero)
        {
            if (EnemyManager.Instance != null)
            {
                while (Vector3.Distance(enemyEntity.transform.position, roamPosition) < enemyEntity.Values.MinRoamDistance)
                    roamPosition = EnemyManager.Instance.RandomNavSpere(startPosition, enemyEntity.Values.MaxRoamDistance, enemyEntity.Values.MoveSurfacesLayerMask);
            }
        }
        else
        {
            roamPosition = enemyEntity.LastPlayerLocation;
            enemyEntity.LastPlayerLocation = Vector3.zero;
        }

        enemyEntity.Agent.speed = enemyEntity.Values.RoamSpeed;
        if (enemyEntity.Agent.isOnNavMesh)
        {
            enemyEntity.Agent.isStopped = false;
            enemyEntity.Agent.SetDestination(roamPosition);
        }

        enemyEntity.PerformStateEffect(enemyEntity.Values.WalkAnimationName, enemyEntity.Values.IdleClip);
    }
    #endregion
}
