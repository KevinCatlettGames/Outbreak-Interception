using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The WaitForSecondsUnscaled class is a custom yield instruction that allows you to wait for a certain amount of time
/// without the time being scaled by Time.timeScale. This is useful for pausing the game without pausing the coroutines.
/// </summary>
public class WaitForSecondsUnscaled : CustomYieldInstruction
{
    // The time at which the wait will end.
    private float endTime;

    /// <summary>
    /// The WaitForSecondsUnscaled constructor.
    /// </summary>
    /// <param name="time"></param> The amount of time to wait.
    public WaitForSecondsUnscaled(float time)
    {
        endTime = Time.unscaledTime + time;
    }

    /// <summary>
    /// The keepWaiting property is used by the coroutine system to determine if the coroutine should continue waiting.
    /// </summary>
    public override bool keepWaiting
    {
        get { return Time.unscaledTime < endTime; }
    }
}
