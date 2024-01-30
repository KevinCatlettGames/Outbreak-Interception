using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Entails functionality for the Main Menu.
/// Changes the start game button onClick event depending on the values in the keyDataHandler singleton.
/// </summary>
public class MainMenu : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        if(GetComponent<AudioSource>())
            audioSource = GetComponent<AudioSource>();
    }

    #region Methods
    /// <summary>
    /// Activates cursor visibility.
    /// Gets a reference to the keyDataHandler singleton
    /// Sets the start game button values.
    /// Subscribes to the OnStartGame event.
    /// </summary>
    void Start()
    {
        Cursor.visible = true;

        if (CursorHandler.instance)
        {
            CursorHandler.instance.SetMenuCursor();
        }
    }

    /// <summary>
    /// Quits the application.
    /// Exits play mode if called from within the editor.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR

        if (EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif

    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource)
            audioSource.PlayOneShot(clip);
    }
    #endregion
}
