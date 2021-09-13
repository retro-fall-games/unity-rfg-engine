using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace StateMachine
  {
    [Serializable]
    public class StateMachine
    {
      public State[] States;
      public State DefaultState;
      public State CurrentState;
      public Type PreviousStateType { get; private set; }
      public Type CurrentStateType { get; private set; }
      public IStateContext Context { get { return _context; } set { _context = value; } }
      private Dictionary<Type, State> _states = new Dictionary<Type, State>();
      private IStateContext _context;

      public void Init()
      {
        if (States == null || States.Length == 0)
        {
          Debug.LogWarning("Init: There are no states");
          return;
        }

        foreach (State state in States)
        {
          _states.Add(state.GetType(), state);
        }
      }

      public void Update()
      {
        if (_context == null)
        {
          Debug.LogWarning("Update: No context defined");
          return;
        }

        if (CurrentState == null)
          ResetToDefaultState();

        Type newStateType = CurrentState.Execute(_context);
        if (newStateType != null)
        {
          ChangeState(newStateType);
        }
      }

      public void ChangeState(Type newStateType)
      {
        if (_context == null)
        {
          Debug.LogWarning("Change State: No context defined");
          return;
        }

        if (!HasState(newStateType))
        {
          Debug.LogWarning($"State not defined: {newStateType.ToString()}");
          return;
        }

        // Dont change if current state
        if (_states[newStateType].Equals(CurrentState))
        {
          return;
        }

        // Exit the previous state if there was one
        if (CurrentState != null)
        {
          PreviousStateType = CurrentState.GetType();
          CurrentState.Exit(_context);
        }

        // Enter the new state
        CurrentState = _states[newStateType];
        CurrentStateType = newStateType;
        CurrentState.Enter(_context);
      }

      public void ResetToDefaultState()
      {
        CurrentState = null;
        if (DefaultState != null)
        {
          ChangeState(DefaultState.GetType());
        }
        else
        {
          Debug.LogWarning("No default state defined");
        }
      }

      public void RestorePreviousState()
      {
        ChangeState(PreviousStateType);
      }

      public bool HasState(Type type)
      {
        return _states.ContainsKey(type);
      }

      public void Bind(IStateContext context)
      {
        _context = context;
      }
    }
  }
}