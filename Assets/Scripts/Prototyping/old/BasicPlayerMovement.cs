using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Left click agent movement for quick prototyping.
/// </summary>
public class BasicPlayerMovement : MonoBehaviour
{
    public float speed = 5;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if(hit.transform.CompareTag("Ground"))
                agent.SetDestination(hit.point);
            }
        }
    }
}
