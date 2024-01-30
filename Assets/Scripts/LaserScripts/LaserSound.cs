using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the laser sound.
/// </summary>
[RequireComponent(typeof(LaserEndPointMovement))]
public class LaserSound : MonoBehaviour
{
    #region Variables
    [Tooltip("The audio source that will be used to play the laser sounds.")]
    [SerializeField] AudioSource audioSource;

    [Tooltip("The audio clip that will be played when the laser starts.")]
    [SerializeField] AudioClip laserStart;

    [Tooltip("The audio clip that will be played when the laser is looping.")]
    [SerializeField] AudioClip laserLoop;

    [Tooltip("The audio clip that will be played when the laser ends.")]
    [SerializeField] AudioClip laserEnd;

    LaserEndPointMovement laserMovement;
    #endregion
    #region Unity Methods
    void Start()
    {
        audioSource.clip = laserLoop;
        laserMovement = GetComponent<LaserEndPointMovement>();
        laserMovement.OnStart += OnStart;
        laserMovement.OnEnd += OnEnd;
    }
    #endregion
    #region Methods

    /// <summary>
    /// Plays the laser start sound and starts the loop coroutine. Gets invoked when the laser starts.
    /// </summary>
    void OnStart()
    {
        audioSource.PlayOneShot(laserStart);
        StartCoroutine(PlayLoopCoroutine(laserStart.length));
    }

    /// <summary>
    /// Plays the laser end sound. Gets invoked when the laser ends.
    /// </summary>
    void OnEnd()
    {
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.PlayOneShot(laserEnd);        
    }

    /// <summary>
    /// Plays the laser loop sound. 
    /// </summary>
    /// <param name="length"></param> The duration until the loop starts.
    /// <returns></returns> Waits for seconds of the start sound.
    IEnumerator PlayLoopCoroutine(float length)
    {
        yield return new WaitForSeconds(length);
        audioSource.loop = true;
        audioSource.Play();
    }
    #endregion
}
