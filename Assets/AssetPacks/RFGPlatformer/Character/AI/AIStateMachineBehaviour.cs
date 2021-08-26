using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI State/AI State Machine Behaviour")]
    public class AIStateMachineBehaviour : MonoBehaviour
    {




      // [HideInInspector]


      // protected virtual void Awake()
      // {

      // }

      // private void Start()
      // {
      //   ResetToDefaultState();
      // }

      // private void Update()
      // {
      //   Type newStateType = CurrentState.Execute(_ctx);
      //   if (newStateType != null)
      //   {
      //     ChangeState(newStateType);
      //   }
      // }

      // public void ChangeState(Type newStateType)
      // {
      //   if (_states[newStateType].Equals(CurrentState))
      //   {
      //     return;
      //   }
      //   if (CurrentState != null)
      //   {
      //     PreviousStateType = CurrentState.GetType();
      //     CurrentState.Exit(_ctx);
      //   }
      //   CurrentState = _states[newStateType];
      //   CurrentStateType = newStateType;
      //   CurrentState.Enter(_ctx);
      // }

      // public void ResetToDefaultState()
      // {
      //   CurrentState = null;
      //   if (_aiBrain.DefaultState != null)
      //   {
      //     ChangeState(_aiBrain.DefaultState.GetType());
      //   }
      // }

      // public void RestorePreviousState()
      // {
      //   ChangeState(PreviousStateType);
      // }

      // public bool HasState(Type type)
      // {
      //   return _states.ContainsKey(type);
      // }
    }
  }
}