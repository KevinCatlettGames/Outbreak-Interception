using System.Collections;
using UnityEngine;

public class ChangeSceneAfterCall : MonoBehaviour
{
    [SerializeField] int buildIndex; 

    [SerializeField] float duration;
    SceneOpener sceneOpener;

    private void Start()
    {
        sceneOpener = SceneOpener.Instance;
    }
    public void Begin()
    {
        StartCoroutine(TransitionCoroutine());
    }

    IEnumerator TransitionCoroutine()
    {
        yield return new WaitForSeconds(duration);
        sceneOpener.OpenSceneNonAdditive(buildIndex);
    }
}


