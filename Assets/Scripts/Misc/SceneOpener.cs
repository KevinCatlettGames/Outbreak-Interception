using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads a scene by build index.
/// </summary>
public class SceneOpener : MonoBehaviour
{
    public static SceneOpener Instance;

    [Tooltip("The animator that will play the transition animation.")]
    [SerializeField] Animator transitionAnimator;

    [Tooltip("The time until the scene should switch, after the transition animation has played.")]
    [SerializeField] float transitionTime = 2;

    [SerializeField] LevelUI levelUI;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1;
    }

    /// <summary>
    /// Saves, triggers the transition effect, then loads the scene with the specified build index.
    /// </summary>
    /// <param name="sceneBuildIndex"></param> The build index of the scene to load.
    public void OpenSceneNonAdditive(int sceneBuildIndex)
    {
        if(transitionAnimator)
            transitionAnimator.SetTrigger("DoTransition");

            StartCoroutine(LoadSceneCoroutine(sceneBuildIndex));

    }

    /// <summary>
    /// Saves, triggers the transition effect, then loads the scene with the specified build index additively.
    /// </summary>
    /// <param name="sceneBuildIndex"></param> The build index of the scene to load.
    public void OpenSceneAdditive(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Opens the scene with the same build index as the current scene.
    /// </summary>
    public void RestartScene()
    {
        levelUI.Paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Unloads the scene with the specified build index.
    /// </summary>
    /// <param name="sceneBuildIndex"></param>
    public void UnloadScene(int sceneBuildIndex)
    {
        SceneManager.UnloadSceneAsync(sceneBuildIndex);
    }

    /// <summary>
    /// Waits until the transition animation has played, then loads the scene with the specified build index.
    /// </summary>
    /// <param name="sceneBuildIndex"></param>
    /// <returns></returns>
    public IEnumerator LoadSceneCoroutine(int sceneBuildIndex)
    {
        yield return new WaitForSecondsUnscaled(transitionTime);
        SceneManager.LoadScene(sceneBuildIndex);
    }


    /// <summary>
    /// Saves, triggers the transition effect, then loads the scene with the specified build index.
    /// </summary>
    /// <param name="sceneBuildIndex"></param> The build index of the scene to load.
    public void OpenSceneNonAdditiveThroughButton(int sceneBuildIndex)
    {
        if (levelUI && !levelUI.Paused || levelUI && levelUI.GameOverCanvas.activeSelf) return;

        if (transitionAnimator)
            transitionAnimator.SetTrigger("DoTransition");

        StartCoroutine(LoadSceneCoroutine(sceneBuildIndex));

    }
}
