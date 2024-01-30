using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathState : State
{
    EnemyEntity enemyEntity;
    bool disabledColliders;
    public EnemyDeathState(Entity entity) : base(entity)
    {
        enemyEntity = entity as EnemyEntity;
    }

    public override void EnterState()
    {
        InitializeValues();
        //base.EnterState();
    }

    public override void UpdateState()
    {
        //base.UpdateState();
        if(enemyEntity.Rigidbody.velocity == Vector3.zero && !disabledColliders)
        {
            disabledColliders = true;
            foreach (Collider collider in enemyEntity.CollidersToDisableOnDeath)
            {
                collider.enabled = false;
            }
        }
    }
    void InitializeValues()
    {
        if(enemyEntity.Agent.isOnNavMesh)
        {
            enemyEntity.Agent.isStopped = true;
        }

        //enemyEntity.SetRigidbodyVelocityToZero();
        enemyEntity.PerformStateEffect(enemyEntity.Values.DeathAnimationName, enemyEntity.Values.DeathClip);
    }
}
