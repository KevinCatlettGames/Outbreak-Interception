using System.Collections;
using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour
{
    [SerializeField] float liveDuration = 1;

    public void Awake()
    {
       StartCoroutine(DestroyCoroutine());
    }
    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(liveDuration);
        Destroy(gameObject);
    }
}
