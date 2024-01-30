using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script is used to move the endPoint of the laser to the point of collision.
/// It also turns the laser off for a certain amount of time after a duration.
/// </summary>
public class LaserEndPointMovement : MonoBehaviour
{
    #region Variables
    [Tooltip("The layermask that will be used to check for collisions with the laser.")]
    [SerializeField] LayerMask stopLayermask;

    [Tooltip("The speed at which the endPoint will move to the point of collision.")]
    [SerializeField] float speed;

    [Tooltip("If this is true, the laser will reset after a certain amount of time.")]
    [SerializeField] bool useReset;

    [Tooltip("The duration after which the laser will reset.")]
    [SerializeField] float resetTime;

    [Tooltip("The duration after which the laser will turn on again after it has been reset.")]
    [SerializeField] float offTime = 2f;

    [Tooltip("The particles that will be turned on and off.")]
    [SerializeField] GameObject[] particles;

    [SerializeField] Vector2 laserPointNormalValue;
    [SerializeField] Vector2 laserPointOnValue;
    [SerializeField] Material laserPointMaterial;
    // The current reset time. This is used to reset the laser after a certain amount of time.
    float currentResetTime;

    // The current off time. This is used to turn the laser off for a certain amount of time after it has been reset.
    float currentOffTime;

    // Is the laser stopped?
    bool isStopped = true;

    // Is the laser off?
    bool isOff = true;

    // The start position of the laser. This is used to reset the laser.
    Vector3 startPosition;

    // The object that is currently being touched by the laser.
    public GameObject currentlyTouchedObject;

    // Has the end sound been played?
    bool invokedEndEvent; 

    // Action that gets invoked when the laser starts.
    public Action OnStart;

    // Action that gets invoked when the laser ends.
    public Action OnEnd;

    [SerializeField] DynamicLineRendererDrawing dynamicLineRendererDrawing;

    #endregion
    #region Unity Methods
    /// <summary>
    /// Initializes the start position and the current reset time.
    /// </summary>
    void Awake()
    {
        if (useReset)
            currentResetTime = resetTime;

        startPosition = transform.position;
        currentOffTime = offTime;

        laserPointMaterial.mainTextureScale = laserPointNormalValue;
    }

    /// <summary>
    /// Turns off the particles and sets the parent to null.
    /// </summary>
    void Start()
    {
        transform.parent = null;
        foreach (GameObject particle in particles)
        {
            particle.SetActive(false);
        }
    }

    void Update()
    {
        if(!isOff && laserPointMaterial.mainTextureScale.x < laserPointOnValue.x && laserPointMaterial.mainTextureScale.y < laserPointOnValue.y)
        {
            laserPointMaterial.mainTextureScale += Vector2.one * .5f * Time.deltaTime;
        }
        if (dynamicLineRendererDrawing.IsActive)
        {
            if (currentlyTouchedObject == null && !isOff)
            {
                isStopped = false;
            }

            Move();
            DoReset();
            TurnOn();
        }
    }
    #endregion
    #region Methods
    /// <summary>
    /// Checks if the currently touched object is null and if so, sets isStopped to false so the laser continues moving.
    /// </summary>
    //void CheckCurrentlyTouchedObject()
    //{
    //    if(currentlyTouchedObject == null && !isOff)
    //        isStopped = false;
    //}

    /// <summary>
    /// Moves the laser (the endpoint) forward if the laser is not stopped currently.
    /// </summary>
    void Move()
    {
        if (isStopped) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    /// <summary>
    /// Turns the laser off after a duration by moving this back to the start position. 
    /// Also disables the particle effects. 
    /// </summary>
    void DoReset()
    {

        if (!useReset) return;

        currentResetTime -= Time.deltaTime;

        if(currentResetTime <= 1 && !invokedEndEvent)
        {
            invokedEndEvent = true;
            OnEnd?.Invoke();
        }

        if(currentResetTime <= 0)
        {           
            currentResetTime = resetTime;
            invokedEndEvent = false;
            isStopped = true;
            isOff = true;
            transform.position = startPosition;
            laserPointMaterial.mainTextureScale = laserPointNormalValue;
            foreach(GameObject particle in particles)
            {
                particle.SetActive(false);
            }           
        }
    }

    /// <summary>
    /// Turns the laser on after a duration and allows the move method to move this transform forward.
    /// Also enables the particle effects of the laser. 
    /// </summary>
    void TurnOn()
    {
        if(!isOff) return;

        currentOffTime -= Time.deltaTime;
        if(currentOffTime <= 0)
        {
            isOff = false;
            isStopped = false;
            currentOffTime = offTime;

            foreach (GameObject particle in particles)
            {
                particle.SetActive(true);
            }

            OnStart?.Invoke();
        }
    }
    #endregion
    #region Triggers
    void OnTriggerEnter(Collider other)
    {
        if (!isOff && !isStopped && (stopLayermask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
           isStopped = true;
           currentlyTouchedObject = other.transform.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(!isOff && currentlyTouchedObject != null && other.gameObject == currentlyTouchedObject && (stopLayermask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            currentlyTouchedObject = null;
            isStopped = false;
        }
    }
    #endregion
}
