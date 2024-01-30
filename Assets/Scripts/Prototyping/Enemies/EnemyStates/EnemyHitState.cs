
using UnityEngine;

public class EnemyHitState : State
{
    EnemyEntity enemyEntity;
    float hitwaitTime;
    public EnemyHitState(Entity entity) : base(entity)
    {
        enemyEntity = entity as EnemyEntity;    
    }

    public override void CheckSwitchState()
    {
        if (enemyEntity.Dead)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.DeathState);
        }
        else if (hitwaitTime <= 0 && enemyEntity.CanRoam)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.RoamingState);
        }
        else if (hitwaitTime <= 0 && !enemyEntity.CanRoam)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.IdleState);
        }
        // base.CheckSwitchState();
    }

    public override void EnterState()
    {
        Initialize();
        //base.EnterState();
    }

    public override void ExitState()
    {
        //base.ExitState();
        enemyEntity.Agent.enabled = true;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        PerformHitSequence();

        //base.UpdateState();
    }

    #region Methods

    void PerformHitSequence()
    {
        hitwaitTime -= Time.deltaTime;
    }
    void Initialize()
    {
        hitwaitTime = .2f;
    }
    #endregion
}
