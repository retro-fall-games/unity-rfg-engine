using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class CharacterStateController : MonoBehaviour
    {
      public CharacterState[] States;
      public CharacterState DefaultState;
      public CharacterState CurrentState;

      [HideInInspector]
      private Character _character;
      private Dictionary<Type, CharacterState> _states;

      private void Awake()
      {
        _character = GetComponent<Character>();
        _states = new Dictionary<Type, CharacterState>();
        foreach (CharacterState state in States)
        {
          _states.Add(state.GetType(), state);
        }
      }

      private void Start()
      {
        Reset();
      }

      private void Update()
      {
        Type newStateType = CurrentState.Execute(_character);
        if (newStateType != null)
        {
          ChangeState(newStateType);
        }
      }

      public void ChangeState(Type newStateType)
      {
        if (CurrentState != null)
        {
          CurrentState.Exit(_character);
        }
        CurrentState = _states[newStateType];
        CurrentState.Enter(_character);
      }

      public void Reset()
      {
        CurrentState = null;
        if (DefaultState != null)
        {
          ChangeState(DefaultState.GetType());
        }
      }
    }
  }
}