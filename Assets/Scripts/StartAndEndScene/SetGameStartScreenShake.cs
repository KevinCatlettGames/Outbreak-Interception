using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// A script handling the call of the screen shake on the CameraShaker instance. 
/// </summary>
public class SetGameStartScreenShake : MonoBehaviour
{
    [Tooltip("The intensity of the screen shake while the timeline is running.")]
    [SerializeField] float camShakeIntensity = 1f;

    CameraShaker camShaker;
    PlayableDirector director;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<PlayableDirector>())
        {
            director = GetComponent<PlayableDirector>();
        }

        if (CameraShaker.instance && director)
        {
            camShaker = CameraShaker.instance;
            camShaker.ShakeCamera((float)director.duration, camShakeIntensity);
        }
    }
  
}
