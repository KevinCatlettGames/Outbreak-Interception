using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEngine.Rendering;

/// <summary>
/// The enemy entity class. This class is used to manage the states of the enemy.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyEntity : Entity
{
    #region Variables
    [Header("Enemy Values Scriptable Object")]
    // Enemy values scriptable object
    [Tooltip("The enemy values scriptable object that will be used to store all the values that are needed for the enemy to function.")]
    [SerializeField] EnemyValues values;
    public EnemyValues Values { get { return values; } }

    [Header("Player Recognition")]
    // Player recognition
    [Tooltip("The position from which a raycast will be shot in the direction of the player to check if line of sight is true.")]
    [SerializeField] Transform sightStartPosition;
    public Transform SightStartPosition { get { return sightStartPosition; }}

    bool dead;
    public bool Dead { get { return dead; }}

    [Tooltip("The collider to disable when dead so there is no collision with the player.")]
    [SerializeField] Collider[] collidersToDisableOnDeath;
    public Collider[] CollidersToDisableOnDeath { get {  return collidersToDisableOnDeath; }}


    // The last known position of the player while chasing him 
    Vector3 lastPlayerLocation;

    public Vector3 LastPlayerLocation { get { return lastPlayerLocation; } set { lastPlayerLocation = value; } }

    [Tooltip("The duration until the enemy will be knockedback again after the last knockback.")]
    [SerializeField] float totalTimeBetweenKnockbacks;

    bool currentlyKnockbacked;

    [SerializeField] ParticleSystem chargeHitParticleSystem;
    [SerializeField] GameObject bloodSplatter;
    [Header("Animation")]
    // Animation
    [Tooltip("The animator used by this enemy.")]
    [SerializeField] Animator animator;
    public Animator Animator { get {  return animator; }}

    AudioSource audioSource;
    public AudioSource AudioSource { get {  return audioSource; }}
    
    Transform target;
    public Transform Target { get { return target; } }

    NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; }}

    Rigidbody rigidBody;
    public Rigidbody Rigidbody { get { return rigidBody;} }

    HealthSystem healthSystem;
    public HealthSystem HealthSystem { get {  return healthSystem; }}


    // States
    State idleState;
    public State IdleState { get { return idleState;}}

    State roamingState;
    public State RoamingState { get { return roamingState; }}

    State fleeState;
    public State FleeState { get { return fleeState; }}

    State chaseState;
    public State ChaseState { get { return chaseState; }}

    State attackState;
    public State AttackState { get { return attackState; } }

    State deathState;
    public State DeathState { get { return deathState; } }

    State hitState;
    public State HitState { get { return hitState; } }

    State chargeAttackState;
    public State ChargeAttackState { get { return chargeAttackState; } }

    public Vector3 chaseTargetPosition;


    [SerializeField] bool canRoam;
    public bool CanRoam { get { return canRoam; }}

    [SerializeField] bool canChase = true;
    public bool CanChase { get {  return canChase; }}


    #endregion

    #region Unity Methods
    void Start()
    {
        Initialize();
        if(EnemyManager.Instance)
        EnemyManager.Instance.AddToEnemiesInScene(this);
    }

    void OnDisable()
    {
        healthSystem.OnDeathEvent.RemoveListener(Die);
        healthSystem.OnDamageEvent.RemoveListener(Damaged);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes the enemy entity.
    /// </summary>
    void Initialize()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = values.RoamSpeed;
        }

        if (GetComponent<Rigidbody>() != null)
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        if (GameObject.FindGameObjectWithTag("EnemyFocusPoint") != null)
            target = GameObject.FindGameObjectWithTag("EnemyFocusPoint").transform;

        if (GetComponent<HealthSystem>() != null)
        {
            healthSystem = GetComponent<HealthSystem>();
            healthSystem.Initialize(values.MaxHealth, values.MaxHealth);
            healthSystem.OnDamageEvent.AddListener(Damaged);
            healthSystem.OnDeathEvent.AddListener(Die);

        }
        if (GetComponent<AudioSource>() != null)
            audioSource = GetComponent<AudioSource>();

        idleState = new EnemyIdleState(this);
        roamingState = new EnemyRoamState(this);
        fleeState = new EnemyFleeState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        deathState = new EnemyDeathState(this);
        hitState = new EnemyHitState(this);
        chargeAttackState = new EnemyChargeAttackState(this);

        if (canRoam)
        {
            int randomValue = Random.Range(0, 10);

            if (randomValue < 5)
            {
                stateMachine = new FiniteStateMachine(idleState);
            }
            else if (randomValue >= 5)
            {
                stateMachine = new FiniteStateMachine(roamingState);
            }
        }
        else
        {
            stateMachine = new FiniteStateMachine(idleState);
        }
    }

    /// <summary>
    /// Performs a raycast from this object in the direction of the player to check if line of sight is true.
    /// </summary>
    /// <returns></returns> A boolean value that is true if the player is in sight and false if not.
    public bool IsPlayerInSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(sightStartPosition.position, -(sightStartPosition.position - target.transform.position), out hit, 500, values.SightInfluencers))
        {
            Debug.DrawLine(sightStartPosition.position, hit.point);

            if (hit.transform.CompareTag("Player"))
            {
                // Debug.Log("Player hit");

                return true;

            }
            else if (hit.transform.CompareTag("Wall"))
            {
                // Debug.Log("Hit wall");

                return false;
            }

            // Debug.Log(hit.transform.name);

        }
        return false;
    }

    /// <summary>
    /// Play a animation and a sound effect.
    /// </summary>
    /// <param name="animationName"></param> The name of the animation that will be played.
    /// <param name="audioClip"></param> The audio clip that will be played.
    public void PerformStateEffect(string animationName, AudioClip audioClip)
    {
        if (Animator && animationName != null)
            Animator.Play(animationName);

        if (AudioSource && audioClip)
            AudioSource.PlayOneShot(audioClip);
    }

    /// <summary>
    /// Spawns a projectile.
    /// </summary>
    public void SpawnProjectile()
    {
        

        if (healthSystem.GetCurrentHealth() > 0)
        {
            transform.LookAt(target.transform.position);
            StartCoroutine(PerformStateEffectCoroutine(1, values.AttackAnimationName, values.AttackClip));

            for (int i = 0; i < values.ProjectileAmount; i++)
            {
                Vector3 moveToPosition = Vector3.zero;
                if (i != 0)
                {
                    // Get the position to where the target is aiming to walk to.
                    Vector3 targetPosition = (target.position + (target.GetComponentInParent<Rigidbody>().velocity * values.ProjectileDistance));

                    // Randomize the position a bit.
                    Vector3 positionRandomizer =
                        new Vector2(Random.Range(-values.ProjectileTargetOffset.x, values.ProjectileTargetOffset.x),
                        Random.Range(-values.ProjectileTargetOffset.y, values.ProjectileTargetOffset.y));

                    // Add the randomizer to the target position. (The y value of the position randomizer is actually the z value of the moveToPosition).
                    moveToPosition = targetPosition + new Vector3(positionRandomizer.x, targetPosition.y, positionRandomizer.y);
                }
                else
                {
                    moveToPosition = (target.position);
                }
                // Spawn the projectile and cache it. 
                GameObject newProjectile = Instantiate(values.ProjectilePrefab, transform.position, Quaternion.identity);

                // Set the position where the projectile should move to.
                newProjectile.GetComponent<Projectile>().SetTarget(moveToPosition);
            }
        }
    }

    /// <summary>
    /// Sets the chase target position.
    /// </summary>
    /// <param name="position"></param> The position that will be set as the chase target position.
    public void SetChaseTargetPosition(Vector3 position)
    {
      chaseTargetPosition = position;
    }

    /// <summary>
    /// Sets the rigidbodies velocity to zero stopping the enemy from moving aslong as the agent is disabled.
    /// </summary>
    public void SetRigidbodyVelocityToZero()
    {
        rigidBody.velocity = Vector3.zero;
    }

    /// <summary>
    /// Damages the target by calling the HealthChange method on its Healthsystem by the attackDamage amount stated in the values scriptableobject.
    /// </summary>
    public void DamageTargetMelee()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < values.AttackDistance +1.5f)
        {
            Vector3 heading = target.position - transform.position;
            float dot = Vector3.Dot(heading, transform.forward);
            if (dot > 1f || Vector3.Distance(transform.position, target.transform.position) < values.AttackDistance)
            {
                // Infront of enemy
                target.GetComponentInParent<HealthSystem>().HealthChange(-values.AttackDamage, values.KnockBack, transform.forward);
                PerformStateEffect(null, values.AttackClip);
                if (values.DoesChargeAttack && chargeHitParticleSystem)
                {
                    chargeHitParticleSystem.Emit(20);
                }
            }
        }     
    }

    /// <summary>
    /// Damages the enemy.
    /// </summary>
    /// <param name="hitDirection"></param> The direction of the hit. 
    /// <param name="knockBack"></param> The amount of knockback that will be applied to the enemy.
    void Damaged(Vector3 hitDirection, float knockBack)
    {
        Instantiate(bloodSplatter, transform.position, transform.rotation);

        if (rigidBody && hitDirection != Vector3.zero && !currentlyKnockbacked && values.CanReceiveKnockback)
        {
          //  stateMachine.SetState(HitState);
            currentlyKnockbacked = true;
            agent.enabled = false;
            float knockBackTime = .5f;
            Invoke("ReenableAgent", knockBackTime);

            StartCoroutine(ReenableKnockbackCoroutine());

            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce((hitDirection * knockBack) * 5, ForceMode.Impulse);
            PerformStateEffect(values.HitAnimationName, values.HitClip);

        }
        else if(!values.CanReceiveKnockback && stateMachine.CurrentState != ChargeAttackState)
        {
            stateMachine.SetState(ChaseState);
        }   
    }

    /// <summary>
    /// Begins the death sequence.
    /// </summary>
    /// <param name="hitDirection"></param> The direction of the hit.
    /// <param name="knockBack"></param> The amount of knockBack that will be applied to the enemy. 
    void Die(Vector3 hitDirection, float knockBack)
    {
        dead = true;
        EnemyManager.Instance.RemoveFromEnemiesInScene(this);
        Instantiate(bloodSplatter, transform.position, transform.rotation);
        StartCoroutine(PerformDeathCoroutine()); 
    }

    /// <summary>
    /// Sets the rigidbody to kinematic and enables the agent.
    /// </summary>
    void ReenableAgent()
    {
        if (healthSystem.GetCurrentHealth() > 0)
        {
            PerformStateEffect(values.RunAnimationName, null);
        }
        else
        {
            PerformStateEffect(values.DeathAnimationName, null);
        }
        agent.enabled = true;
    }

    /// <summary>
    /// Destroy this gameObject after the deathWaitTime. 
    /// </summary>
    /// <returns></returns>
    IEnumerator PerformDeathCoroutine()
    {
        yield return new WaitForSeconds(values.DeathWaitTime);
        agent.baseOffset = .2f;
        agent.enabled = false;
      //  Destroy(gameObject);
    }

    IEnumerator ReenableKnockbackCoroutine()
    {
        yield return new WaitForSeconds(totalTimeBetweenKnockbacks);
        currentlyKnockbacked = false;
    }

    IEnumerator PerformStateEffectCoroutine(float waitTime, string animationName, AudioClip audioClip)
    {
        yield return new WaitForSeconds(waitTime);
        if(healthSystem.GetCurrentHealth() > 0)
         PerformStateEffect(animationName, audioClip);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall") && stateMachine.CurrentState == hitState)
        {
            SetRigidbodyVelocityToZero();
        }
    }
    #endregion
}