/// <summary>
/// Handles the current state of an entity and its transitions.
/// </summary>
public class FiniteStateMachine
{
    // The current state of the entity.
    State currentState;
    public State CurrentState { get { return currentState; } }
    public FiniteStateMachine(State firstState)
    {
        currentState = firstState;
        currentState.EnterState();
    }

    public void Update()
    {
        currentState.UpdateState();
    }

    /// <summary>
    /// Sets the current state to be the new state and handles transitioning.
    /// </summary>
    /// <param name="newState"></param> The state to transition to.
    public void SetState(State newState)
    {
        if (newState == null || currentState == newState) return;
        else
        {
            currentState.ExitState(); // Exit the current state.
            newState.EnterState(); // Enter the new state.
            currentState = newState; // Set the current state to the new state.
        }
    }
}
