using UnityEngine;

/// <summary>
/// The enemy values class. This class is used to store all the values that are needed for the enemy to function.
/// </summary>
[CreateAssetMenu(fileName = "EnemyValues", menuName = "ScriptableObjects/EnemyValues", order = 1)]
public class EnemyValues : ScriptableObject
{
    #region Health 
    [Header("Health")]
    // Health
    [Tooltip("The maximum health the enemy can have.")]
    [SerializeField] int maxHealth = 10;
    public int MaxHealth { get { return maxHealth; } }

    [SerializeField] LayerMask damageLayer;
    public LayerMask DamageLayer { get { return damageLayer; } }
    #endregion

    #region Effects
    [Header("Animation")]
    // Animation
    [Tooltip("The name of the idle animation state inside of the animator.")]
    [SerializeField] string idleAnimationName = "Idle";
    public string IdleAnimationName { get { return idleAnimationName; } }

    [Tooltip("The name of the walk animation state inside of the animator.")]
    [SerializeField] string walkAnimationName = "Walk";
    public string WalkAnimationName { get { return walkAnimationName; } }

    [Tooltip("The name of the run animation state inside of the animator.")]
    [SerializeField] string runAnimationName = "Run";
    public string RunAnimationName { get { return runAnimationName; } }

    [Tooltip("The name of the attack animation state inside of the animator.")]
    [SerializeField] string attackAnimationName = "Attack";
    public string AttackAnimationName { get { return attackAnimationName; } }

    [Tooltip("The name of the death animation state inside of the animator")]
    [SerializeField] string deathAnimationName = "Death";
    public string DeathAnimationName { get { return deathAnimationName; } }

    [Tooltip("The name of the hit animation state inside of the animator")]
    [SerializeField] string hitAnimationName = "Death";
    public string HitAnimationName { get { return hitAnimationName; } }

    [Tooltip("The name of the knockback animation state inside of the animator")]
    [SerializeField] string knockBackAnimationName = "KnockBack";
    public string KnockBackAnimationName { get { return knockBackAnimationName; } }

    [Tooltip("The name of the charge animation state inside of the animator")]
    [SerializeField] string chargeAnimationName = "Charge";
    public string ChargeAnimationName { get { return chargeAnimationName; } }

    [Tooltip("The name of the charging animation state inside of the animator")]
    [SerializeField] string chargingAnimationName = "Charging";
    public string ChargingAnimationName { get { return chargingAnimationName; } }

    [Header("Sound")]
    // Sound
    [Tooltip("The idle AudioClip for this enemy.")]
    [SerializeField] AudioClip idleClip;
    public AudioClip IdleClip { get { return idleClip; } }

    [Tooltip("The aggro AudioClip for this enemy.")]
    [SerializeField] AudioClip[] aggroClips;
    public AudioClip[] AggroClip { get { return aggroClips; } }

    [Tooltip("The attack AudioClip for this enemy.")]
    [SerializeField] AudioClip attackClip;
    public AudioClip AttackClip { get { return attackClip; } }

    [Tooltip("The hit AudioClip for this enemy.")]
    [SerializeField] AudioClip hitClip;
    public AudioClip HitClip { get { return hitClip; } }

    [Tooltip("The death AudioClip for this enemy.")]
    [SerializeField] AudioClip deathClip;
    public AudioClip DeathClip { get { return deathClip; } }

    #endregion

    #region State values
    [Header("Idle")]
    // Idle 
    [Tooltip("How long should the enemy idle the shortest for before switching state?")]
    [SerializeField] float minIdleTime = 5;
    public float MinIdleTime { get { return minIdleTime; } }

    [Tooltip("How long should the enemy idle the longest for before switching state?")]
    [SerializeField] float maxIdleTime = 5;
    public float MaxIdleTime { get { return maxIdleTime; } }


    [Header("Roam")]
    // Roam
    [Tooltip("The speed which will be applied onto the agent while walking.")]
    [SerializeField] float roamSpeed = 5;
    public float RoamSpeed { get { return roamSpeed; } }

    [Tooltip("How long should the enemy roam the shortest for before switching state?")]
    [SerializeField] float minRoamTime = 5;
    public float MinRoamTime { get { return minRoamTime; } }

    [Tooltip("How long should the enemy roam the longest for before switching state?")]
    [SerializeField] float maxRoamTime = 5;
    public float MaxRoamTime { get { return maxRoamTime; } }

    [Tooltip("How far away at minimum from the current position should the roam position be?")]
    [SerializeField] float minRoamDistance = 5;
    public float MinRoamDistance { get { return minRoamDistance; } }

    [Tooltip("How far away at maximum from the current position should the roam position be?")]
    [SerializeField] float maxRoamDistance = 20;
    public float MaxRoamDistance { get { return maxRoamDistance; } }

    [Tooltip("Which objects the entity can move onto.")]
    [SerializeField] LayerMask moveSurfacesLayerMask = -1;
    public LayerMask MoveSurfacesLayerMask { get { return moveSurfacesLayerMask; } }


    [Header("Chase")]
    // Chase
    [Tooltip("The speed which will be applied onto the agent while running.")]
    [SerializeField] float chaseSpeed = 7;
    public float ChaseSpeed { get { return chaseSpeed; } }

    [Tooltip("At what distance should the enemy start chasing the target?")]
    [SerializeField] float chaseDistance = 10;
    public float ChaseDistance { get { return chaseDistance; } }

    [Header("Flee")]
    // Flee
    [Tooltip("Should the enemy flee?")]
    [SerializeField] bool canFlee = true;
    public bool CanFlee { get { return canFlee; } }

    [Tooltip("If the enemy should flee, at what distance should the enemy start fleeing?")]
    [SerializeField] float fleeDistance = 5;
    public float FleeDistance { get { return fleeDistance; } }

    [Tooltip("If the enemy should flee, and the player is withing melee distance, should the enemy start attacking?")]
    [SerializeField] bool fleeToAttack = true;
    public bool FleeToAttack { get { return fleeToAttack; } }

    [Tooltip("If the enemy should flee, and the player is within melee distance, at what distance should the enemy start attacking?")]
    [SerializeField] float fleeToAttackDistance = 2;
    public float FleeToAttackDistance { get { return fleeToAttackDistance; } }


    [Header("Attack")]
    // Attack
    [Tooltip("The layers that should be recognized when checking for sight")]
    [SerializeField] LayerMask sightInfluencers;
    public LayerMask SightInfluencers { get { return sightInfluencers; } }

    [Tooltip("How long should the enemy wait before doing the first attack?")]
    [SerializeField] float initialAttackWaitTime = 1;
    public float InitialAttackWaitTime { get { return initialAttackWaitTime; } }

    [Tooltip("How long should the enemy wait before attacking again?")]
    [SerializeField] float attackWaitTime = 1;
    public float AttackWaitTime { get { return attackWaitTime; } }

    [Tooltip("At what distance should the enemy attack the target?")]
    [SerializeField] float attackDistance = 1;
    public float AttackDistance { get { return attackDistance; } }

    [Tooltip("The damage inflicted on the player when attacking.")]
    [SerializeField] float attackDamage = 1;
    public float AttackDamage { get { return attackDamage; } }

    [Tooltip("The amount of knockback that will be applied onto the target.")]
    [SerializeField] float knockBack;
    public float KnockBack { get { return knockBack; } }

    [Tooltip("Should the enemy attack with a ranged weapon?")]
    [SerializeField] bool isRanged;
    public bool IsRanged { get { return isRanged; } }

    [Tooltip("Does the enemy do a charge attack")]
    [SerializeField] bool doesChargeAttack;
    public bool DoesChargeAttack { get { return doesChargeAttack; } }

    [Tooltip("If the enemy charges, how fast should it charge?")]
    [SerializeField] float chargeSpeed = 10;
    public float ChargeSpeed { get { return chargeSpeed; } }

    [Tooltip("The projectile prefab that should be spawned when ranged and attacking.")]
    [SerializeField] GameObject projectilePrefab;
    public GameObject ProjectilePrefab { get { return projectilePrefab;  } }

    [Tooltip("The distance from the player that the projectile should move to.")]
    [SerializeField] float projectileDistance = 1;
    public float ProjectileDistance { get { return projectileDistance; } }

    [Tooltip("The amount of projectiles that should be spawned per shot")]
    [SerializeField] int projectileAmount = 1;
    public int ProjectileAmount { get { return projectileAmount; } }

    [Tooltip("The randomized offset that should be applied to the projectile target position")]
    [SerializeField] Vector2 projectileTargetOffset = Vector2.zero;
    public Vector2 ProjectileTargetOffset { get { return projectileTargetOffset; } }


    [Header("Death")]
    // Death
    [Tooltip("How long should the enemy wait before disappearing?")]
    [SerializeField] float deathWaitTime = 5;
    public float DeathWaitTime { get { return deathWaitTime; } }


    [Header("Hit")]
    // Hit
    [Tooltip("How long should the enemy wait before going out of the hit state?")]
    [SerializeField] float hitWaitTime = 5;
    public float HitWaitTime { get { return hitWaitTime; } }

    [Header("Misc")]
    [SerializeField] float knockBackMultiplier = 1;
    public float KnockBackMultiplier { get { return knockBackMultiplier; } }

    [Tooltip("Should the enemy be able to receive knockback?")]
    [SerializeField] bool canReceiveKnockback = true;
    public bool CanReceiveKnockback { get { return canReceiveKnockback; } }
    #endregion

}
