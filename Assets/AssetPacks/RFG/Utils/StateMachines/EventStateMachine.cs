using System;
using UnityEngine;
using RFG.Events;

namespace RFG.Utils.StateMachines
{
  public class EventStateMachine<T> where T : struct
  {
    public GameObject gameObject;
    public T CurrentState { get; protected set; }
    public T PreviousState { get; protected set; }
    public event Action<T> OnStateChange;

    public EventStateMachine(GameObject gameObject)
    {
      this.gameObject = gameObject;
    }

    public void ChangeState(T newState)
    {
      if (newState.Equals(CurrentState))
      {
        return;
      }
      PreviousState = CurrentState;
      CurrentState = newState;
      OnStateChange?.Invoke(CurrentState);
      EventManager.TriggerEvent(new EventStateChangeEvent<T>(this));
    }

    public void RestorePreviousState()
    {
      CurrentState = PreviousState;
      OnStateChange?.Invoke(CurrentState);
    }

  }
}