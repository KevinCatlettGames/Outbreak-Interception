// Written by Kevin Catlett.

/// <summary>
/// Derives from the GameMusic class.
/// Creates a singleton.
/// </summary>
public class MenuMusic : GameMusic
{
    // Singleton referencing this instance. 
    public static MenuMusic instance;
    
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
    // Further functionality can be added if the menu music should have unique functionality.
}
