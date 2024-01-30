using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyChargeAttackState : State
{
    bool chargeEnded;
    bool doCharge;
    float chargeWaitTime = 1.3f;
    float maxChargeTime = 5;
    EnemyEntity enemyEntity;
    Vector3 chargePosition;
    float waitBeforeEnding;
    bool attackAnimationPlayed; 
    public EnemyChargeAttackState(Entity entity) : base(entity)
    {
        this.enemyEntity = entity as EnemyEntity;
    }

    public override void EnterState()
    {
        Debug.Log("In charge attack");
        InitializeValues();
        maxChargeTime = 5;
        chargeEnded = false;
        doCharge = false;
        chargeWaitTime = 5;
        waitBeforeEnding = 1.3f;
        attackAnimationPlayed = false;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    
        chargeWaitTime -= Time.deltaTime;
        if(chargeWaitTime <= 0 && !doCharge)
        {
            doCharge = true;
            PerformAttack();
        }
  
        if (doCharge)
        {
            chargePosition = enemyEntity.Target.position;
            Vector3 heading = enemyEntity.Target.position - enemyEntity.transform.position;
            float dot = Vector3.Dot(heading, enemyEntity.transform.forward);

            if (!chargeEnded || chargeEnded && dot > .4f)
                 enemyEntity.Agent.SetDestination(chargePosition);

            maxChargeTime -= Time.deltaTime;
            if (Vector3.Distance(enemyEntity.transform.position, chargePosition) <= 1.5f)
            {
                if (!attackAnimationPlayed)
                {
                    attackAnimationPlayed = true;
                    enemyEntity.PerformStateEffect(enemyEntity.Values.AttackAnimationName, enemyEntity.Values.AggroClip[2]);
                }
                Debug.Log("Charge ended");
                chargeEnded = true;
            }
        } 
        
        if(chargeEnded && waitBeforeEnding > 0)
        {
            
            waitBeforeEnding -= Time.deltaTime;
        }
    }

 

    public override void CheckSwitchState()
    {
        if (waitBeforeEnding <= 0 || maxChargeTime <= 0)
        {
            enemyEntity.Agent.isStopped = true;
            enemyEntity.StateMachine.SetState(enemyEntity.ChaseState);
        }
        else if (enemyEntity.Dead)
        {
            enemyEntity.StateMachine.SetState(enemyEntity.DeathState);
        }
        
    }

    #region Methods
    /// <summary>
    /// Resets the attack wait time and stops the enemy from moving. 
    /// Changes the color of the gameObject to the color of this state.
    /// </summary>
    void InitializeValues()
    {       
        enemyEntity.PerformStateEffect(enemyEntity.Values.ChargingAnimationName, null);
        enemyEntity.Agent.isStopped = true;
        enemyEntity.transform.LookAt(enemyEntity.Target.position);
        enemyEntity.Agent.speed = enemyEntity.Values.ChargeSpeed;
    }

    void PerformAttack()
    {    
        enemyEntity.Agent.isStopped = false;      
        enemyEntity.PerformStateEffect(enemyEntity.Values.ChargeAnimationName, null);
    }
    #endregion
}
