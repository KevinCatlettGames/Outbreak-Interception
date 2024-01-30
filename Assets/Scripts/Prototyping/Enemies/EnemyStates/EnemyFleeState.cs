using UnityEngine;

/// <summary>
/// Makes the enemy flee from the player in the opposite direction.
/// </summary>
public class EnemyFleeState : State
{
    // A random value deciding which state to switch to.
    float transitionValue;
    // The duration between the destination setting.
    const float destinationSettingDuration = .2f;
    float currentDestinationSettingDuration;

    // The entity this state is attached to.
    EnemyEntity enemyEntity;

    /// <summary>
    /// Creates a new instance of the EnemyFleeState class.
    /// </summary>
    /// <param name="stateColor"></param> The color of the objects material while in this state.
    /// <param name="entity"></param> The entity this state is attached to.
    public EnemyFleeState(Entity entity) : base(entity)
    {
        this.enemyEntity = entity as EnemyEntity;
    }

    public override void EnterState()
    {
        InitializeValues();

        // base.EnterState();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        PerformDestinationSetting();

        // base.UpdateState();
    }

    public override void CheckSwitchState()
    {
        if(enemyEntity.Dead)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.DeathState); // Death State
        }

        else if (Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.transform.position) > enemyEntity.Values.FleeDistance)
        {
            transitionValue = Random.Range(0, 1);

            if (transitionValue <= .5f || !enemyEntity.CanRoam)
                enemyEntity.StateMachine.SetState(enemyEntity.IdleState); // Idle State
            else if(transitionValue > .5f || enemyEntity.CanRoam)
                enemyEntity.StateMachine.SetState(enemyEntity.RoamingState); // Roaming State
        }

        else if (Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.transform.position) < enemyEntity.Values.FleeToAttackDistance && !enemyEntity.Values.DoesChargeAttack)
           enemyEntity.StateMachine.SetState(enemyEntity.AttackState); // Attack State

        else if(enemyEntity.Values.DoesChargeAttack)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.ChargeAttackState);
        }

    


        // base.CheckSwitchState();
    }

    #region Methods
    /// <summary>
    /// Enables agent movement and sets the color of the object.
    /// </summary>
    void InitializeValues()
    {
        enemyEntity.Agent.speed = enemyEntity.Values.ChaseSpeed;
        enemyEntity.Agent.isStopped = false;

        enemyEntity.PerformStateEffect(enemyEntity.Values.RunAnimationName, null);
    }

    /// <summary>
    /// Set the destination of the agent to the opposite of the targets position once the currentDestinationSettingDuration is at zero.
    /// </summary>
    void PerformDestinationSetting()
    {
        currentDestinationSettingDuration -= Time.deltaTime;
        if (currentDestinationSettingDuration <= 0)
        {
            currentDestinationSettingDuration = destinationSettingDuration;

            Vector3 runTo = enemyEntity.transform.position + ((enemyEntity.transform.position - enemyEntity.Target.position));
            enemyEntity.Agent.SetDestination(runTo);
        }
    }
    #endregion
}
