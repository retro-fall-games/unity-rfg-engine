using System;
using UnityEngine;
using RFG.Utils.StateMachines;

namespace RFG.Events
{
  public struct EventStateChangeEvent<T> where T : struct
  {
    public GameObject gameObject;
    public EventStateMachine<T> eventStateMachine;
    public T newState;
    public T previousState;

    public EventStateChangeEvent(EventStateMachine<T> eventStateMachine)
    {
      this.gameObject = eventStateMachine.gameObject;
      this.eventStateMachine = eventStateMachine;
      newState = eventStateMachine.CurrentState;
      previousState = eventStateMachine.PreviousState;
    }

  }
}