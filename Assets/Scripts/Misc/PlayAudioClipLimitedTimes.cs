using UnityEngine;

public class PlayAudioClipLimitedTimes : MonoBehaviour
{
    [SerializeField] int clipPlayAmount = 1;

    int currentAmount = 0;
    [SerializeField] AudioSource audioS;
    [SerializeField] AudioClip audioC;


    public void PlayOnce()
    {
        if (audioS && audioC && currentAmount < clipPlayAmount)
        {
            audioS.PlayOneShot(audioC);
            currentAmount++;
        }
    }
}
