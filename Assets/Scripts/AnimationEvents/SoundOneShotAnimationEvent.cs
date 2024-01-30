using UnityEngine;

/// <summary>
/// A script that holds a method for playing a audioclip once.
/// </summary>
public class SoundOneShotAnimationEvent : MonoBehaviour
{
    [Tooltip("The audioSource to play the clip from.")]
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        if(!audioSource && GetComponent<AudioSource>())
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
