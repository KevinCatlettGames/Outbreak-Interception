using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChangeLightAfterTimelineDuration : MonoBehaviour
{
    PlayableDirector director;
    [SerializeField] Color colorToChangeTo;
    [SerializeField] Light lightToChangeColorOf; 

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PlayableDirector>())
        {
                director = GetComponent<PlayableDirector>();
                StartCoroutine(ChangeLightAfterDurationCoroutine((float)director.duration));
        }        
    }

    IEnumerator ChangeLightAfterDurationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        lightToChangeColorOf.color = colorToChangeTo;
    }
}
