using System.Collections;
using UnityEngine;

public class MoveObjectOnce : MonoBehaviour
{
    [SerializeField] Vector3 moveVector;
    [SerializeField] bool moveOnStart;

    private void Start()
    {
        if(moveOnStart)
            transform.position = transform.position + moveVector;
    }
    public void MoveOnce()
    {
        StartCoroutine(WaitAndMove());
    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(2f);
        transform.position = transform.position + moveVector;
    }

}
