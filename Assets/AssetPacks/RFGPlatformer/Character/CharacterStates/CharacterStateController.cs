using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/State/Character State Controller")]
    public class CharacterStateController : MonoBehaviour
    {
      public CharacterState[] States;
      public CharacterState DefaultState;
      public CharacterState CurrentState;
      public Type PreviousStateType { get; private set; }
      public Type CurrentStateType { get; private set; }

      [HideInInspector]
      private Character _character;
      private Dictionary<Type, CharacterState> _states;

      private void Awake()
      {
        _character = GetComponent<Character>();
        _states = new Dictionary<Type, CharacterState>();
        foreach (CharacterState state in States)
        {
          state.Init(_character);
          _states.Add(state.GetType(), state);
        }
      }

      private void Start()
      {
        Reset();
      }

      private void Update()
      {
        Type newStateType = CurrentState.Execute();
        if (newStateType != null)
        {
          ChangeState(newStateType);
        }
      }

      public void ChangeState(Type newStateType)
      {
        if (_states[newStateType].Equals(CurrentState))
        {
          return;
        }
        if (CurrentState != null)
        {
          PreviousStateType = CurrentState.GetType();
          CurrentState.Exit();
        }
        CurrentState = _states[newStateType];
        CurrentStateType = newStateType;
        CurrentState.Enter();
      }

      public void Reset()
      {
        CurrentState = null;
        if (DefaultState != null)
        {
          ChangeState(DefaultState.GetType());
        }
      }

      public void RestorePreviousState()
      {
        ChangeState(PreviousStateType);
      }
    }
  }
}