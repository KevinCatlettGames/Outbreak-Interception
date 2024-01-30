using System.Collections;
using UnityEngine;

/// <summary>
/// The LevelExit class enables the exit trigger when all enemies in the scene have been defeated, 
/// by subscribing to the OnAllEnemiesDefeated event.
/// When the player enters the exit trigger, the scene will transition to the next scene after a certain amount of time.
/// </summary>
public class LevelExit : MonoBehaviour
{
    #region Variables
    [Tooltip("The collider that will be enabled when all enemies have been defeated.")]
    [SerializeField] Collider exitCollider;

    [SerializeField] Collider[] collidersToDeactivate;
    [SerializeField] Collider[] collidersToActivate;

    [SerializeField] GameObject doorObject;

    [Tooltip("The build index of the scene that will be loaded when the player enters the exit trigger.")]
    [SerializeField] int nextSceneBuildIndex;

    [SerializeField] GameObject redLight;
    [SerializeField] GameObject greenLight;
    #endregion

    #region Methods
    void Start()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.OnAllEnemiesDefeated += EnableExitTrigger;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (exitCollider.enabled && other.CompareTag("Player"))
        {
            Debug.Log("Exiting Level");
            if (SceneOpener.Instance)
                SceneOpener.Instance.OpenSceneNonAdditive(nextSceneBuildIndex);
        }
    }

    /// <summary>
    /// Enables the exit trigger and changes the color of the door to green.
    /// </summary>
    void EnableExitTrigger()
    {


        exitCollider.enabled = true;
        foreach(Collider collider in collidersToDeactivate)
        {
            collider.enabled = false;
        }
        foreach(Collider collider in collidersToActivate)
        {
            collider.enabled = true;
        }
        doorObject.SetActive(false);

        if(redLight)
        redLight.SetActive(false);
        if(greenLight)
        greenLight.SetActive(true);
    }
    #endregion
}

