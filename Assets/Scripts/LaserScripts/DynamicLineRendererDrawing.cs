using UnityEngine;

/// <summary>
/// This script is used to draw a line between two points and generate a collider between them.
/// It also checks if the line is colliding with any object in the collisionLayerMask and if so, it will move the endPoint to the point of collision.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class DynamicLineRendererDrawing : MonoBehaviour
{
    #region Variables
    [Tooltip("The layer mask that will be used to check for collisions with the line.")]
    [SerializeField] LayerMask collisionLayerMask;

    [Tooltip("The start point of the line.")]
    [SerializeField] Transform startPoint;

    [Tooltip("The end point of the line.")]
    [SerializeField] Transform endPoint;

    [Tooltip("The colliders center will be positioned to the center of the line. This offset will move it further to the start or to the end depending on its value.")]
    [SerializeField] float colliderCenterOffset = 0;

    [Tooltip("The duration between each line update. This is used to make the line update slower and smoother and stop it updating every frame, which is not needed.")]
    [SerializeField] float lineUpdateDuration = .05f;

    [SerializeField] bool useTriggerToActivate; 

    bool isActive;
    public bool IsActive {  get { return isActive; } set {  isActive = value; } }


    // The line renderer component
    LineRenderer lineRenderer;

    // The box collider component
    BoxCollider boxCollider;

    // The current line update duration
    float currentLineUpdateDuration;
    #endregion
    #region Unity Methods
    /// <summary>
    /// Initializes the update duration and gets the required components.
    /// Also sets the start point of the line.
    /// </summary>
    void Awake()
    {

        if(useTriggerToActivate)
        {
            isActive = false;
        }
        else
        {
            isActive = true;
        }
        currentLineUpdateDuration = lineUpdateDuration;

        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, startPoint.position);
    }

    /// <summary>
    /// Updates the line and generates the collider.
    /// </summary>
    void LateUpdate()
    {
        if(isActive)
        UpdateLine();        
    }
    #endregion
    #region Methods
    /// <summary>
    /// Updates the line after the update duration and calls the generate collider method.
    /// </summary>
    void UpdateLine()
    {
        currentLineUpdateDuration -= Time.deltaTime;

        if (currentLineUpdateDuration <= 0)
        {
            currentLineUpdateDuration = lineUpdateDuration;
            lineRenderer.SetPosition(1, endPoint.position);
        }

        GenerateCollider();
    }

    /// <summary>
    ///  Generates the collider between the start and end point of the line.
    /// </summary>
    void GenerateCollider()
    {
       float lineLength = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y, (lineLength / 2) + colliderCenterOffset);
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, lineLength);
    }

    /// <summary>
    /// Checks if the line is colliding with any object in the collisionLayerMask and if so, it will move the endPoint to the point of collision simulating the blocking of the laser.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if ((collisionLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            float distance = Vector3.Distance(startPoint.position, other.transform.position);
            endPoint.transform.position = startPoint.position + (startPoint.forward * distance);
        }
    }
    #endregion
}
