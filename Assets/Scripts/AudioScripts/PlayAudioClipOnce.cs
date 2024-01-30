using UnityEngine;

/// <summary>
///  This script plays a audio clip once.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayAudioClipOnce : MonoBehaviour
{
    // The audio source that plays the audio clip.
    AudioSource audioSource;

    [Tooltip("The audio clip that is played once.")]
    [SerializeField] AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
