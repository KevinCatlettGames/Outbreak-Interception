using UnityEngine;

public class EnemyAttackAnimationEvent : MonoBehaviour
{
    [SerializeField] EnemyEntity enemyEntity;

    public void DoAttack()
    {
        enemyEntity.DamageTargetMelee();
    }
}

