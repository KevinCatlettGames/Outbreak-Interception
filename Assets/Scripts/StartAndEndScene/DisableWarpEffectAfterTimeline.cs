using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Gets the duration of the timeline and stops the particle system once it is over by calling a coroutine.
/// </summary>
public class DisableWarpEffectAfterTimeline : MonoBehaviour
{
    [Tooltip("The director of the timeline.")]
    [SerializeField] PlayableDirector director;

    [Tooltip("The amount that will be added when positive or subtracted when negative from the timelines duration.")]
    [SerializeField] float stopTimeOffset = -1f;

    [Tooltip("The Particle System that displays a warp  effect.")]
    [SerializeField] ParticleSystem warpParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        if(director)
        {
            float warpTime = (float)director.duration + stopTimeOffset;          
            StartCoroutine(DisableWarpEffectCoroutine(warpTime));
        }
    }

    IEnumerator DisableWarpEffectCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        if(warpParticleSystem)
        {
            warpParticleSystem.Stop();
        }
    }

}
