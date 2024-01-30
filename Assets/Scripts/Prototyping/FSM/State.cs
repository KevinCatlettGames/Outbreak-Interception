using UnityEngine;

/// <summary>
/// The base class for all states. This class is abstract and cannot be instantiated.
/// </summary>
public class State
{
    // The entity that is using this state.
    protected Entity entity;

    public State(Entity entity)
    {
        this.entity = entity;
    }

    #region Virtual Methods
    /// <summary>
    /// Called when the state is entered for initialization of the state.
    /// </summary>
    public virtual void EnterState()
    {
        //Debug.Log("Entered State");
    }

    /// <summary>
    ///  Called every frame while the state is active.
    /// </summary>
    public virtual void UpdateState()
    {
        //Debug.Log("Updating State");
    }

    /// <summary>
    /// Called every frame while the state is active.
    /// </summary>
    public virtual void CheckSwitchState()
    {
        //Debug.Log("Checking Switch State");
    }

    /// <summary>
    /// Called when the state is exited for refreshing the state.
    /// </summary>
    public virtual void ExitState()
    {
        //Debug.Log("Exiting State");
    }
   #endregion
}
