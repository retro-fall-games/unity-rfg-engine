using System;
using UnityEngine;

namespace RFG.Utils.StateMachines
{
  public class StateMachine<T>
  {
    public GameObject gameObject;
    public T CurrentState { get; protected set; }
    public T PreviousState { get; protected set; }
    public event Action<T> OnStateChange;

    public StateMachine(GameObject gameObject)
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