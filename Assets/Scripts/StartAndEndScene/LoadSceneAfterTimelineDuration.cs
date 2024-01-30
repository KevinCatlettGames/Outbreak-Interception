using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class LoadSceneAfterTimelineDuration : MonoBehaviour
{
    SceneOpener sceneOpener;
    PlayableDirector director;
    [SerializeField] int nextSceneBuildIndex = 2;
    // Start is called before the first frame update
    void Start()
    {     
        if(SceneOpener.Instance)
        {
            sceneOpener = SceneOpener.Instance;

            if (GetComponent<PlayableDirector>())
            {
                director = GetComponent<PlayableDirector>();
                StartCoroutine(LoadSceneAfterDuration((float)director.duration));
            }
        }      
    }

    IEnumerator LoadSceneAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        sceneOpener.OpenSceneNonAdditive(nextSceneBuildIndex);
        if(CursorHandler.instance)
        {
            CursorHandler.instance.SetGameplayCursor();
        }
    }
}
