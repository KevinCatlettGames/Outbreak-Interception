using UnityEngine;

/// <summary>
/// The base class for all entities.
/// </summary>
public class Entity : MonoBehaviour
{
    protected FiniteStateMachine stateMachine;
    public FiniteStateMachine StateMachine { get { return stateMachine; } }

    private void Update()
    {
        stateMachine.Update();
    }
}
