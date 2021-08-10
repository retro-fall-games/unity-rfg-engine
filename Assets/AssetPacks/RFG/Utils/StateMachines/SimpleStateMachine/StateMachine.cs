using System;
using UnityEngine;

namespace RFG
{
  public class StateMachine<T> where T : struct
  {
    public GameObject gameObject;
    public T CurrentState { get; protected set; }
    public T PreviousState { get; protected set; }
    public event Action<T> OnStateChange;

    public StateMachine(GameObject gameObject, bool triggerEvents)
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
    }

    public void RestorePreviousState()
    {
      CurrentState = PreviousState;
      OnStateChange?.Invoke(CurrentState);
    }

  }
}