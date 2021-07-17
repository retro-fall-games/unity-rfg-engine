using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/State Machines/Tick State Machine")]
  public class TickStateMachine : MonoBehaviour
  {
    public TickBaseState CurrentState { get; private set; }
    public event Action<TickBaseState> OnStateChange;
    private TickBaseState DefaultState { get; set; }
    private Dictionary<Type, TickBaseState> availableStates;

    private void Update()
    {
      if (CurrentState == null)
      {
        CurrentState = GetDefaultState();
        CurrentState.OnEnter();
      }

      var nextState = CurrentState?.Tick();

      if (nextState != null && nextState != CurrentState?.GetType())
      {
        SwitchToNewState(nextState);
      }
    }

    public void SwitchToNewState(Type nextState)
    {
      if (CurrentState != null)
      {
        CurrentState.OnExit();
      }
      if (availableStates.ContainsKey(nextState))
      {
        CurrentState = availableStates[nextState];
      }
      else
      {
        CurrentState = GetDefaultState();
      }
      CurrentState.OnEnter();
      OnStateChange?.Invoke(CurrentState);
    }

    public void SwitchToDefaultState()
    {
      if (CurrentState != null)
      {
        CurrentState.OnExit();
      }
      CurrentState = GetDefaultState();
      CurrentState.OnEnter();
      OnStateChange?.Invoke(CurrentState);
    }

    public void SetStates(Dictionary<Type, TickBaseState> states)
    {
      availableStates = states;
    }

    public TickBaseState GetDefaultState()
    {
      if (DefaultState == null)
      {
        DefaultState = availableStates.Values.First();
      }
      return DefaultState;
    }

    public void SetDefaultState(Type stateType)
    {
      if (availableStates.ContainsKey(stateType))
      {
        DefaultState = availableStates[stateType];
      }
      else
      {
        DefaultState = availableStates.Values.First();
      }
    }

    public void AddState(Type type, TickBaseState state)
    {
      availableStates.Add(type, state);
    }
  }

}