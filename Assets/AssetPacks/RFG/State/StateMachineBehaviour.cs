using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/State/State Machine Behaviour")]
  public class StateMachineBehaviour : MonoBehaviour
  {
    public State[] States;
    public State DefaultState;
    public State CurrentState;
    public Type PreviousStateType { get; private set; }
    public Type CurrentStateType { get; private set; }
    public IStateContext Context { get { return _context; } set { _context = value; } }

    [HideInInspector]
    private Dictionary<Type, State> _states;
    private IStateContext _context;

    protected virtual void Awake()
    {
      // Set the default context
      StateAnimatorContext context = new StateAnimatorContext();
      context.transform = transform;
      context.animator = GetComponent<Animator>();

      // Set the context interface
      _context = context;

      // Setup the states
      _states = new Dictionary<Type, State>();
      foreach (State state in States)
      {
        _states.Add(state.GetType(), state);
      }
    }

    private void Start()
    {
      ResetToDefaultState();
    }

    private void Update()
    {
      Type newStateType = CurrentState.Execute(_context);
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
        CurrentState.Exit(_context);
      }
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
    }

    public void RestorePreviousState()
    {
      ChangeState(PreviousStateType);
    }

    public bool HasState(Type type)
    {
      return _states.ContainsKey(type);
    }
  }
}