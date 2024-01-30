using UnityEngine;

/// <summary>
/// Makes the enemy idle for the idleTime duration.
/// </summary>
public class EnemyIdleState : State
{
    // The amount of time the entity will idle before switching to another state.
    float idleTime;

    // The entity that is using this state.
    EnemyEntity enemyEntity;

    /// <summary>
    /// Creates a new instance of the EnemyIdleState class.
    /// </summary>
    /// <param name="stateColor"></param> The color of the objects material while in this state.
    /// <param name="entity"></param> The entity this state is attached to.
    public EnemyIdleState(Entity entity) : base(entity)
    {
        this.enemyEntity = entity as EnemyEntity;
        idleTime = Random.Range(enemyEntity.Values.MinIdleTime, enemyEntity.Values.MaxIdleTime);
    }

    public override void EnterState()
    {
        idleTime = Random.Range(enemyEntity.Values.MinIdleTime, enemyEntity.Values.MaxIdleTime);
        InitializeValues();

        // base.EnterState();
    }

    public override void UpdateState()
    {
      
        CheckSwitchState();
        idleTime -= Time.deltaTime;

        // base.UpdateState();
    }

    public override void CheckSwitchState()
    {
        if (enemyEntity.Dead)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.DeathState);
        }
        else if (enemyEntity.Target && Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.position) < enemyEntity.Values.AttackDistance && enemyEntity.IsPlayerInSight())
            enemyEntity.StateMachine.SetState(enemyEntity.AttackState);

        else if (enemyEntity.CanChase && enemyEntity.Target && Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.position) < enemyEntity.Values.ChaseDistance && enemyEntity.IsPlayerInSight())
        {
            enemyEntity.PerformStateEffect(null, enemyEntity.Values.AggroClip[Random.Range(0, enemyEntity.Values.AggroClip.Length -1)]);
            enemyEntity.StateMachine.SetState(enemyEntity.ChaseState);
        }

        else if (idleTime < 0 && enemyEntity.CanRoam)
            enemyEntity.StateMachine.SetState(enemyEntity.RoamingState);

        // base.CheckSwitchState();
    }

    #region Methods
    void InitializeValues()
    {

        if (enemyEntity.Agent.isOnNavMesh)
        {
            enemyEntity.Agent.isStopped = true;
        }

        enemyEntity.PerformStateEffect(enemyEntity.Values.IdleAnimationName, enemyEntity.Values.IdleClip);
    }
    #endregion
}
