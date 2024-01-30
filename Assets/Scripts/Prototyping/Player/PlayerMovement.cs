using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class converts the inputs of the player to movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Player Stats")]
    [Tooltip("Player stats scriptable object.")]
    [SerializeField] PlayerStats stats;
    public PlayerStats Stats { get { return stats; } }

    [Header("Dash")]
    [Tooltip("Velocity of the dash.")]
    [SerializeField] float dashSpeed;

    [Tooltip("Duration of the dash.")]
    [SerializeField] float dashDuration;

    [Tooltip("Cooldown of the dash.")]
    [SerializeField] float dashCooldown;

    [Header("Knockback")]
    [Tooltip("Factor for the relationship between knockback and stun time.")]
    [SerializeField] float knockbackStun;

    [Header("Aiming")]
    [Tooltip("Target the torso will be facing.")]
    [SerializeField] Transform aimTarget;

    // Static vectors for the movement and aim direction to be used by other scripts.
    public static Vector3 MoveVector;
    public static Vector3 AimVector;

    // Camera and plane used for aiming.
    private Camera mainCam;
    private Plane groundPlane = new Plane(Vector3.up, -1.25f);

    // Vector for used for aiming.
    private Vector3 targetVector;

    // Vector representing the x and y inputs for the player movement.
    private Vector2 inputVector;

    // Rigidbody is used for movement and knockback.
    private Rigidbody rb;

    // Healthsystem is deactivated during knockbackstun.
    private HealthSystem healthSystem;

    // Timer for stun time.
    private float knockbackTimer;

    // Timer for dash duration.
    private float dashTimer;

    // Timer for dash cooldown.
    private float dashCooldownTimer;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        mainCam = Camera.main;
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// The update counts down every active timer and calls the Aim() method.
    /// If the player finishes beeing stunned or dashing by knockback the healthsystem will reactivate.
    /// The convertinputs method is called when the player exits a dash or stun this allows the player to start moving again immediately.
    /// </summary>
    private void Update()
    {
        Aim();
        if (knockbackTimer > 0)
        {
            knockbackTimer = Countdown(knockbackTimer);
            if (knockbackTimer <= 0)
            {
                healthSystem.invounerable = false;
                ConvertInputs();
            }
        }
        if (dashTimer > 0)
        {
            dashTimer = Countdown(dashTimer);
            if (dashTimer <= 0)
            {
                healthSystem.dashInounerable = false;
                ConvertInputs();
            }
        }
        dashCooldownTimer = Countdown(dashCooldownTimer);
    }

    /// <summary>
    /// The Move() method controlls the velocity of the rigidbody but is only called when the player is nighter hit nor dashing.
    /// </summary>
    private void FixedUpdate()
    {
        if (knockbackTimer <= 0 && dashTimer <= 0)
        {
            Move();
        }
        else
        {
            MoveVector = Vector3.zero;
        }
    }

    /// <summary>
    /// This method just subtracts time.deltatime from any timer greater than 0.
    /// </summary>
    /// <param name="timer"> timer. </param>
    /// <returns> timer - time.deltatime </returns>
    private float Countdown(float timer)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }
        return timer;
    }

    #endregion

    #region Movement

    /// <summary>
    /// This method sets the velocity of the rigidbody to the movementvector multiplied with the movementspeed which is dependent on the inputs.
    /// Y is set to 0 because the player is not supposed to move upwards.
    /// </summary>
    private void Move()
    {
        MoveVector.y = 0;
        MoveVector.Normalize();
        MoveVector *= stats.Speed;
        rb.velocity = MoveVector;
    }
    
    /// <summary>
    /// A ray is cast from the camera towards the cursor through the ground plane.
    /// From the point where the ray and the plane intersect the targetvector is formed. 
    /// </summary>
    private void Aim()
    {
        float distance;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray, out distance))
        {
            targetVector = ray.GetPoint(distance);
            aimTarget.position = targetVector;
        }
        targetVector.x -= transform.position.x;
        targetVector.y = 0;
        targetVector.z -= transform.position.z;
        AimVector = targetVector;
    }

    #endregion

    #region Inputs

    /// <summary>
    /// Takes the movement inputs from the player as a vector 2 vor vertical and horisontal movement. 
    /// </summary>
    /// <param name="input"></param>
    public void MoveInput(InputAction.CallbackContext input)
    {
        inputVector = input.ReadValue<Vector2>();
        if (knockbackTimer <= 0 && dashTimer <= 0)
        {
            ConvertInputs();          
        }
    }

    /// <summary>
    /// Converts the vector 2 input to a vector 3 movementvector.
    /// </summary>
    private void ConvertInputs()
    {
        MoveVector.x = -inputVector.x;
        MoveVector.z = -inputVector.y;
    }

    /// <summary>
    /// If the input is pressed, the player isnt stunned and has a dash available,
    /// then the player will move very fast into the direction they are currently moving in for the duration of the dash.
    /// The player is invounerable during dashing.
    /// </summary>
    /// <param name="input"></param>
    public void DashInput(InputAction.CallbackContext input)
    {
        if (dashCooldownTimer <= 0 && input.started && MoveVector != Vector3.zero)
        {
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            MoveVector.y = 0;
            MoveVector.Normalize();
            MoveVector *= dashSpeed;
            rb.velocity = MoveVector;
            healthSystem.dashInounerable = true;
        }
    }

    #endregion

    #region Damage

    /// <summary>
    /// If the damage dealt to the player has knockback the player will be stunned for a short duration and kicked into a direction.
    /// During this stun the health system will de disabled, thus preventing further damage.
    /// </summary>
    /// <param name="direction"> Direction the player is knocked in. </param>
    /// <param name="knockback"> Amount of knockback. </param>
    public void OnDamage(Vector3 direction, float knockback)
    {
        
        if (knockback > 0)
        {
            dashTimer = 0;
            healthSystem.invounerable = true;
            knockbackTimer += knockback * knockbackStun;
            CameraShaker.instance.ShakeCamera(.2f,knockback);
            direction *= knockback;
            rb.AddForce(direction, ForceMode.Impulse);
        }
        else
        {
            CameraShaker.instance.ShakeCamera(.2f, 3);
        }
    }

    #endregion

}
