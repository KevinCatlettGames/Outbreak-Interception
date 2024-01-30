// Written by Kevin Catlett. 

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Music manager base class. Destroys itself once the scene changes to a scene where it should not exist.
/// </summary>
public class GameMusic : MonoBehaviour
{
    [Tooltip("Destroy this gameobject if one of these scenes get loaded.")]
    [SerializeField] string[] scenesWhereDestroy;

    AudioSource audioSource;

    #region Methods
    /// <summary>
    /// Makes this gameObject not destroy itself on scene change.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        if(audioSource && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// Subscribe to the activeSceneChanged event of the SceneManager.
    /// </summary>
    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneLoaded;
       // SceneManager.sceneLoaded += OnSceneLoadedAdditive;
       // SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    /// <summary>
    /// Unsubscribes to the activeSceneChanged event of the SceneManager.
    /// </summary>
    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneLoaded;
        //SceneManager.sceneLoaded -= OnSceneLoadedAdditive;
    }
    
    /// <summary>
    /// Subscribed to the activeSceneChanged event on the SceneManager.
    /// Destroys this gameObject if it should not be in the newly loaded scene.
    /// </summary>
    /// <param name="oldScene"></param> The scene which this gameObject has come from.
    /// <param name="newScene"></param> The scene which this gameObject has just entered.
    void OnSceneLoaded(Scene oldScene, Scene newScene)
    {
        foreach (string o in scenesWhereDestroy) // Check if this scene is a scene where this should be destroyed in. 
        {
            if (newScene.name == o) // If this should be destroyed.
            {
                SceneManager.activeSceneChanged -= OnSceneLoaded; // Unsubscribe!
                Destroy(gameObject); 
            }
        }
    }

    /// <summary>
    /// Subscribed to the sceneLoaded event on the SceneManager.
    /// Destroys this gameObject if it should not be in the newly loaded scene.
    /// Is used for additive scene loading.
    /// </summary>
    /// <param name="scene"></param> The scene which this gameObject has just entered.
    /// <param name="mode"></param> The mode in which the scene was loaded.
    //void OnSceneLoadedAdditive(Scene scene, LoadSceneMode mode)
    //{
    //    foreach (string o in scenesWhereDestroy) // Check if this scene is a scene where this should be destroyed in. 
    //    {
    //        if (scene.name == o) // If this should be destroyed.
    //        {
    //            if(audioSource)
    //               audioSource.volume = 0;
    //        }
    //    }
    //}

    //void OnSceneUnloaded(Scene scene)
    //{
    //    foreach (string o in scenesWhereDestroy) // Check if this scene is a scene where this should be destroyed in. 
    //    {
    //        if (scene.name == o) // If this should be destroyed.
    //        {
    //            if (audioSource)
    //            {
    //                audioSource.volume = 1;
    //                audioSource.Play();
    //            }

    //            if(Object.FindObjectOfType<MenuMusic>())
    //            Destroy(Object.FindObjectOfType<MenuMusic>().gameObject);
    //        }
    //    }
    //}
    #endregion


}
