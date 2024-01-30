// Written by Kevin Catlett.

/// <summary>
/// Derives from the GameMusic class.
/// Creates a singleton.
/// </summary>
public class LevelMusic : GameMusic 
{
    // Singleton referencing this instance. 
    public static LevelMusic instance;
    
    #region Methods
    /// <summary>
    /// Makes this a singleton. Destroys this if a singleton has already been set.
    /// </summary>
    void Awake()
    {
        // Setting the singleton.
        if (instance == null) { instance = this; }
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    // Further logic can be added if the level music should have unique functionality.
}
