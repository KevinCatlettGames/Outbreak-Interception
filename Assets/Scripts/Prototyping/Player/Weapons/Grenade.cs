using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the grenade projectile.
/// </summary>
public class Grenade : MonoBehaviour
{
    #region Variables

    [Tooltip("The scriptable object that contains all the values for this projectile.")]
    [SerializeField] ProjectileSO projectileValues;

    [Tooltip("The amount of knockback the grenade applies.")]
    [SerializeField] float knockback;

    // The script to apply the explosion damage and knockback and spawn the effect.
    Explosion explosion;

    // Where the grenade is aimed at.
    Vector3 destination;

    // These bools ensure that things dont happen twice.
    bool hitEffectSpawned = false;
    bool isLauched = false;

    #endregion

    #region Unity Methods

    void Start()
    {
        explosion = GetComponent<Explosion>();
    }

    /// <summary>
    /// Moves the grenade to the destination eonce it has been launched.
    /// </summary>
    void Update()
    {
        if (isLauched)
        {
            Move();
        }
    }

    #endregion

    #region Launch

    /// <summary>
    /// This method is called by an outside script to launch a grenade once the according input is given.
    /// </summary>
    /// <param name="destination"></param>
    public void Launch(Vector3 destination)
    {
        this.destination = destination;
        isLauched= true;
    }

    /// <summary>
    /// This method moves the grenade towards the destination and along an animation curve.
    /// Once the grenade is close enough to the destination it blows up.
    /// </summary>
    void Move()
    {
        if (Time.timeScale == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, projectileValues.Speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destination) > 1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + projectileValues.YMovementAnimationCurve.Evaluate(transform.position.y) * projectileValues.YMovementAnimationCurveMultiplier, transform.position.z);
            }
            else
            {
                BlowUp();
            }

        }
    }

    #endregion

    #region Explosion

    /// <summary>
    /// The grenade also explodes when it collides with any enemy or object.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
       BlowUp();
    }

    /// <summary>
    /// This method instantiates the explosion object which plays a sound spawns an effect and does the damage and knockback.
    /// Then the grenade destroys itself.
    /// </summary>
    private void BlowUp()
    {
        if (hitEffectSpawned == false && projectileValues.ProjectileHitEffectPrefab != null)
        {
            hitEffectSpawned = true;
            // Spawn hit effect
            GameObject newHitEffect = Instantiate(projectileValues.ProjectileHitEffectPrefab, transform.position, Quaternion.identity);
            explosion = newHitEffect.GetComponent<Explosion>();
            explosion.Explode(projectileValues.Damage, knockback);
            Destroy(gameObject);
        }
    }

    #endregion
}
