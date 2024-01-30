using UnityEngine;

/// <summary>
/// Makes the enemy chase the player.
/// </summary>
public class EnemyChaseState : State
{
    // A random value deciding which state to switch to.
    float transitionValue;

    // The duration between the destination setting.
    const float destinationSettingDuration = .2f;
    float currentDestinationSettingDuration;
    bool animationPlayed;

    // The entity this state is attached to.
    EnemyEntity enemyEntity;

    /// <summary>
    /// Creates a new instance of the EnemyChaseState class.
    /// </summary>
    /// <param name="stateColor"></param> The color of the objects material while in this state.
    /// <param name="entity"></param> The entity this state is attached to.
    public EnemyChaseState(Entity entity) : base(entity)
    {
        this.enemyEntity = entity as EnemyEntity;
    }

    public override void EnterState()
    {
        InitializeValues();
        enemyEntity.transform.LookAt(enemyEntity.Target.position);
        // base.EnterState();
    }

    public override void UpdateState()
    {
        if(!animationPlayed)
        {
            enemyEntity.PerformStateEffect(enemyEntity.Values.RunAnimationName, null);
            animationPlayed = true;
        }
        CheckSwitchState();
        PerformDestinationSetting();
        Debug.Log("In chase state");
        // base.UpdateState();
    }

    public override void CheckSwitchState()
    {
        if(!enemyEntity.CanChase)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.IdleState);
        }

        else if (enemyEntity.Dead)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.DeathState); // Death State
        }

        else if (Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.transform.position) < enemyEntity.Values.AttackDistance && !enemyEntity.Values.DoesChargeAttack)
            enemyEntity.StateMachine.SetState(enemyEntity.AttackState);

        else if(enemyEntity.Values.DoesChargeAttack)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.ChargeAttackState);
        }

        else if (Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.transform.position) > enemyEntity.Values.ChaseDistance)
        {
            enemyEntity.LastPlayerLocation = enemyEntity.Target.transform.position;
            enemyEntity.StateMachine.SetState(enemyEntity.RoamingState);
        }

        // base.CheckSwitchState();
    }

    public override void ExitState()
    {
        EnemyManager.Instance.EnemiesInTargetRange.Remove(enemyEntity);
        animationPlayed = false;
        //base.ExitState();
    }
    #region Methods
    /// <summary>
    /// Resets the currentDestinationSettingDuration value and enables agent movement.
    /// Changes the color of the gameObject to the color of this state.
    /// </summary>
    void InitializeValues()
    {
        EnemyManager.Instance.EnemiesInTargetRange.Add(enemyEntity);
        enemyEntity.Agent.speed = enemyEntity.Values.ChaseSpeed;
        enemyEntity.transform.LookAt(enemyEntity.Target);
        if (enemyEntity.Agent.isOnNavMesh)
        {
            enemyEntity.Agent.isStopped = false;
        }

        currentDestinationSettingDuration = destinationSettingDuration;
    }

    /// <summary>
    /// Set the destination of the agent to the targets position once the currentDestinationSettingDuration is at zero.
    /// </summary>
    void PerformDestinationSetting()
    {
        currentDestinationSettingDuration -= Time.deltaTime;
        if(currentDestinationSettingDuration <= 0 && enemyEntity.chaseTargetPosition != Vector3.zero)
        {
            currentDestinationSettingDuration = destinationSettingDuration;
            if (enemyEntity.Agent.isOnNavMesh)
                enemyEntity.Agent.SetDestination(enemyEntity.chaseTargetPosition);
        }     
    }
    #endregion
}
