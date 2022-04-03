using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Class for in-game timer. </summary>
public class Timer : MonoBehaviour
{
    // Inspector-editable variables
    /// <summary> The default time for this timer. </summar>
    [SerializeField]
    private float defaultTime = 60;
    /// <summary> Text to display the timer. </summary>
    [SerializeField]
    private Text text;
    // private variables
    /// <summary> The current time remaining. </summary>
    public float currentTime{ get; private set; }
    /// <summary> Whether the timer is running. </summary>
    public bool running { get; private set; } = false;
    private List<Action> finishActions = new List<Action>();
    // private functions
    void Update()
    {
        if (!running) return;

        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            currentTime = 0;
            running = false;
            // call finish actions
            foreach(Action action in finishActions) action();
        }

        int displayTime = Mathf.CeilToInt(currentTime);
        if(displayTime == 1) text.text = "1 second remaining";
        else text.text = string.Format("{0} seconds remaining", displayTime);
    }
    // public functions
    /// <summary> Pauses the timer. </summary>
    public void Pause()
    {
        running = false;
    }
    /// <summary> Resumes the timer. </summary>
    public void Resume()
    {
        if(currentTime > 0) running = true;
    }
    /// <summary> Resets the timer. </summary>
    public void ResetTime()
    {
        running = true;
        currentTime = defaultTime;
    }
    /// <summary> Add an action to the list of actions to perform when the timer finishes. </summary>
    public void AddFinishAction(Action action)
    {
        finishActions.Add(action);
    }
    /// <summary> Remove an action from the list of actions to perform when the timer finishes. </summary>
    public void RemoveFinishAction(Action action)
    {
        finishActions.Remove(action);
    }
}
