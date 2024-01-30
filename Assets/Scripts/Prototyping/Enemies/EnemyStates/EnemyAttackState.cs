using UnityEngine;

/// <summary>
/// Makes the enemy attack the player.
/// </summary>
public class EnemyAttackState : State
{
    // The time until the enemy attacks again.
    float currentAttackWaitTime;

    // The entity this state is attached to.
    EnemyEntity enemyEntity;

    /// <summary>
    /// Creates a new instance of the EnemyAttackState class.
    /// </summary>
    /// <param name="color"></param> The color of objects material while in this state.
    /// <param name="entity"></param> The entity this state is attached to.
    public EnemyAttackState(Entity entity) : base(entity)
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
        CheckForAttack();
        Debug.Log("In Attack state");
        // base.UpdateState();
    }

    public override void CheckSwitchState()
    {
        if (enemyEntity.Dead)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.DeathState); // Death State
        }

        else if (Vector3.Distance(enemyEntity.transform.position, enemyEntity.Target.transform.position) > enemyEntity.Values.AttackDistance + 1.5f)
            enemyEntity.StateMachine.SetState(enemyEntity.ChaseState); // Chase State
    }

    #region Methods
    /// <summary>
    /// Resets the attack wait time and stops the enemy from moving. 
    /// Changes the color of the gameObject to the color of this state.
    /// </summary>
    void InitializeValues()
    {
        enemyEntity.PerformStateEffect(enemyEntity.Values.IdleAnimationName, null);
        currentAttackWaitTime = enemyEntity.Values.InitialAttackWaitTime;
        enemyEntity.transform.LookAt(enemyEntity.Target);
        if (enemyEntity.Agent.isOnNavMesh)
        {
            enemyEntity.Agent.isStopped = true;
        }
        enemyEntity.Agent.velocity = Vector3.zero;
    }

    /// <summary>
    /// When the currentAttackWaitTime is at zero, perform a attack and reset the waitTime.
    /// </summary>
    void CheckForAttack()
    {
        currentAttackWaitTime -= Time.deltaTime;

        if (currentAttackWaitTime <= 0)
        {       
            currentAttackWaitTime = enemyEntity.Values.AttackWaitTime;
            PerformAttack();
        }
    }

    /// <summary>
    /// Plays the attack animation and plays the attack clip.
    /// </summary>
    void PerformAttack()
    {       
        if (enemyEntity.Values.IsRanged)
        {
            enemyEntity.SpawnProjectile();           
        }
        else 
        {
            enemyEntity.PerformStateEffect(enemyEntity.Values.AttackAnimationName, null);
            enemyEntity.transform.LookAt(enemyEntity.Target);
        }   
    }
    #endregion
}

