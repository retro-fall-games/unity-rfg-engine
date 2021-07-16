using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public enum CharacterStates
  {
    Alive,
    Dead,
    Paused,
  }

  public enum MovementStates
  {
    Idle,
    Walking,
    Running,
    Falling,
    Jumping,
    WallJumping,
    WallClinging,
    Dashing
  }
  [AddComponentMenu("RFG Platformer/Character/Character")]
  public class Character : MonoBehaviour
  {
    public StateMachine<CharacterStates> CharacterState => _characterState;
    public StateMachine<MovementStates> MovementState => _movementState;
    public CharacterController2D Controller => _controller;
    public CharacterInput CharacterInput => _characterInput;

    private StateMachine<CharacterStates> _characterState;
    private StateMachine<MovementStates> _movementState;
    private List<CharacterBehavior> _behaviors;
    private CharacterController2D _controller;
    private CharacterInput _characterInput;


    private void Awake()
    {
      _characterState = new StateMachine<CharacterStates>(gameObject, true);
      _movementState = new StateMachine<MovementStates>(gameObject, true);
      _controller = GetComponent<CharacterController2D>();
      _characterInput = GetComponent<CharacterInput>();

      // Create all the behaviors
      _behaviors = new List<CharacterBehavior>();
      _behaviors.AddRange(GetComponents<CharacterBehavior>());

      MovementState.OnStateChange += OnMovementStateChange;
    }

    private void Start()
    {
      InitBehaviors();
    }

    private void Update()
    {
      EarlyProcessBehaviors();
      if (Time.timeScale != 0f)
      {
        ProcessBehaviors();
        LateProcessBehaviors();
      }
    }

    private void InitBehaviors()
    {
      foreach (CharacterBehavior behavior in _behaviors)
      {
        behavior.InitBehavior();
      }
    }


    private void EarlyProcessBehaviors()
    {
      foreach (CharacterBehavior behavior in _behaviors)
      {
        behavior.EarlyProcessBehavior();
      }
    }

    private void ProcessBehaviors()
    {
      if (_characterState.CurrentState == CharacterStates.Dead)
      {
        return;
      }
      foreach (CharacterBehavior behavior in _behaviors)
      {
        behavior.ProcessBehavior();
      }
    }

    private void LateProcessBehaviors()
    {
      foreach (CharacterBehavior behavior in _behaviors)
      {
        behavior.LateProcessBehavior();
      }
    }

    public T FindBehavior<T>() where T : CharacterBehavior
    {
      Type behaviorType = typeof(T);

      foreach (CharacterBehavior behavior in _behaviors)
      {
        if (behavior is T characterBehavior)
        {
          return characterBehavior;
        }
      }
      return null;
    }

    private void OnMovementStateChange(MovementStates state)
    {
      // Debug.Log("Movement State Change: " + state);
    }

    public void Birth()
    {
      _characterState.ChangeState(CharacterStates.Alive);
      _controller.enabled = true;
    }

    public void Kill()
    {
      _characterState.ChangeState(CharacterStates.Dead);
      _controller.enabled = false;
    }


  }
}