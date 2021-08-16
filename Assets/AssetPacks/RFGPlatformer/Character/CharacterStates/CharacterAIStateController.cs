using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/State/Character AI State Controller")]
    public class CharacterAIStateController : MonoBehaviour
    {
      public CharacterState[] States;
      public CharacterState DefaultState;
      public CharacterState CurrentState;
      public Type PreviousStateType { get; private set; }
      public Type CurrentStateType { get; private set; }

      [HideInInspector]
      private CharacterStateController.CharacterStateContext _stateContext;
      private Dictionary<Type, CharacterState> _states;

      private void Awake()
      {
        _stateContext = new CharacterStateController.CharacterStateContext();
        _stateContext.transform = transform;
        _stateContext.character = GetComponent<Character>();
        _stateContext.animator = GetComponent<Animator>();
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
        Type newStateType = CurrentState.Execute(_stateContext);
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
          CurrentState.Exit(_stateContext);
        }
        CurrentState = _states[newStateType];
        CurrentStateType = newStateType;
        CurrentState.Enter(_stateContext);
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