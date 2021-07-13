using UnityEngine;
using RFG.Utils;

namespace RFG.Events
{
  public struct StateChangeEvent<T> where T : struct
  {
    public GameObject gameObject;
    public StateMachine<T> stateMachine;
    public T newState;
    public T previousState;

    public StateChangeEvent(StateMachine<T> stateMachine)
    {
      this.gameObject = stateMachine.gameObject;
      this.stateMachine = stateMachine;
      newState = stateMachine.CurrentState;
      previousState = stateMachine.PreviousState;
    }

  }
}