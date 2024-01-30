using UnityEngine;

/// <summary>
/// This script moves a projectile towards a target position. and spawns a hit effect when it reaches the target position or hits a object..
/// </summary>
public class Projectile : MonoBehaviour
{
    [Tooltip("The scriptable object that contains all the values for this projectile.")]
    [SerializeField] ProjectileSO projectileValues;

    [Tooltip("The prefab that is spawned when the projectile hits a object.")]
    [SerializeField] GameObject projectileIndicator;

    [Tooltip("A multiplier for the y position of the projectile indicator. This is used to make sure the projectile indicator is spawned above the ground.")]
    [SerializeField] float projectileIndicatorYOffset;

    [Tooltip("The layer mask that contains all the layers that the projectile can hit.")]
    [SerializeField] LayerMask destroyLayerMask;

    [SerializeField] GameObject mesh;

    // Used to make sure the hit effect is only spawned once.
    bool hitEffectSpawned;
    bool activated;
    float waitTime = 1;

    private void Awake()
    {
        mesh.SetActive(false);
    }

    public void Update()
    {  
      if(waitTime <= 0 && !activated)
      {
            activated = true;
            mesh.SetActive(true);
      }

      waitTime -= Time.deltaTime;

      if (activated)
      {
            
          Move();
          OnTargetReached();
      }
    }

    #region Methods

    /// <summary>
    /// Finds the position of the target object and sets the target position to it.
    /// Resets the parent of the projectileIndicator so movement is not affected by the parent.
    /// </summary>
    /// <param name="targetPosition"></param> The current position of the target object.
    /// <param name="offset"></param> The offset from the target position.
    public void SetTarget(Vector3 targetPosition)
    {      
        projectileIndicator.transform.position = new Vector3(targetPosition.x, targetPosition.y * projectileIndicatorYOffset, targetPosition.z); 
        projectileIndicator.transform.parent = null;
    }

    /// <summary>
    /// Moves the projectile towards the target position.
    /// </summary>
    void Move()
    {
        if (Time.timeScale == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, projectileIndicator.transform.position, projectileValues.Speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, projectileIndicator.transform.position) > 1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + projectileValues.YMovementAnimationCurve.Evaluate(transform.position.y) * projectileValues.YMovementAnimationCurveMultiplier, transform.position.z);
            }
        }
    }

    /// <summary>
    /// Spawns a hit effect when the projectile reaches the target position and destroys the projectile and its indicator.
    /// </summary>
    void OnTargetReached()
    {
        if (!hitEffectSpawned && Vector3.Distance(transform.position, projectileIndicator.transform.position) < .5f)
        {
            Destroy();
        }
    }
    #endregion

    #region Triggering

    /// <summary>
    /// Spawns a hit effect when the projectile hits a object and destroys the projectile and its indicator.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if ((destroyLayerMask.value & (1 << other.transform.gameObject.layer)) > 0 && !hitEffectSpawned)
        {
            Destroy();
        }
    }

    /// <summary>
    /// Spawns a hit effect when the projectile hits a object and destroys the projectile and its indicator.
    /// </summary>
    private void Destroy()
    {
        hitEffectSpawned = true;
        // Spawn hit effect
        GameObject newHitEffect = Instantiate(projectileValues.ProjectileHitEffectPrefab, transform.position, Quaternion.identity);
        Destroy(projectileIndicator);
        Destroy(gameObject);
    }

    #endregion
}
