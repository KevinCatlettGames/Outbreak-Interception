using UnityEngine;

/// <summary>
/// This class is used to store all the values that are needed for the projectile to function.
/// </summary>
[CreateAssetMenu(fileName = "ProjectileSO", menuName = "ScriptableObjects/ProjectileSO", order = 1)]
public class ProjectileSO : ScriptableObject
{
    [Tooltip("The speed of the projectile.")]
    [SerializeField] float speed;
    public float Speed { get { return speed; } }

    [Tooltip("The damage inflicted on objects with a health system component.")]
    [SerializeField] float damage;
    public float Damage { get { return damage; } }

    [Tooltip("The vertical offset of the projectile while traveling to its destination.")]
    [SerializeField] AnimationCurve yMovementAnimationCurve;
    public AnimationCurve YMovementAnimationCurve { get { return yMovementAnimationCurve;   } }

    [Tooltip("The multiplier of the yMovementAnimationCurve. This is used to make the projectile travel higher or lower.")]
    [SerializeField] float yMovementAnimationCurveMultiplier;
    public float YMovementAnimationCurveMultiplier { get { return yMovementAnimationCurveMultiplier; } }

    [Tooltip("The hit effect that is instantiated when the projectile hits an object or reaches its destination.")]
    [SerializeField] GameObject projectileHitEffectPrefab;
    public GameObject ProjectileHitEffectPrefab { get { return projectileHitEffectPrefab; } }

    [Tooltip("The sound that is played when the projectile hits an object or reaches its destination.")]
    [SerializeField] AudioClip projectileHitSound;
}
