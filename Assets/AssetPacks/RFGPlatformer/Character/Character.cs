using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public enum CharacterType { Player, AI }

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
    [Header("Settings")]
    public CharacterType characterType = CharacterType.Player;

    public StateMachine<CharacterStates> CharacterState => _characterState;
    public StateMachine<MovementStates> MovementState => _movementState;
    public CharacterController2D Controller => _controller;
    public CharacterInput CharacterInput => _characterInput;

    private StateMachine<CharacterStates> _characterState;
    private StateMachine<MovementStates> _movementState;
    private List<CharacterBehaviour> _Behaviours;
    private CharacterController2D _controller;
    private CharacterInput _characterInput;
    private TickStateMachine _aiStateMachine;

    protected virtual void Awake()
    {
      _characterState = new StateMachine<CharacterStates>(gameObject, true);
      _movementState = new StateMachine<MovementStates>(gameObject, true);
      _controller = GetComponent<CharacterController2D>();
      _characterInput = GetComponent<CharacterInput>();

      // Create all the Behaviours
      _Behaviours = new List<CharacterBehaviour>();
      _Behaviours.AddRange(GetComponents<CharacterBehaviour>());

      MovementState.OnStateChange += OnMovementStateChange;
    }

    private void Start()
    {
      InitBehaviours();
    }

    private void Update()
    {
      EarlyProcessBehaviours();
      if (Time.timeScale != 0f)
      {
        ProcessBehaviours();
        LateProcessBehaviours();
      }
    }

    private void InitBehaviours()
    {
      foreach (CharacterBehaviour Behaviour in _Behaviours)
      {
        Behaviour.InitBehaviour();
      }
    }


    private void EarlyProcessBehaviours()
    {
      foreach (CharacterBehaviour Behaviour in _Behaviours)
      {
        Behaviour.EarlyProcessBehaviour();
      }
    }

    private void ProcessBehaviours()
    {
      if (_characterState.CurrentState == CharacterStates.Dead)
      {
        return;
      }
      foreach (CharacterBehaviour Behaviour in _Behaviours)
      {
        Behaviour.ProcessBehaviour();
      }
    }

    private void LateProcessBehaviours()
    {
      foreach (CharacterBehaviour Behaviour in _Behaviours)
      {
        Behaviour.LateProcessBehaviour();
      }
    }

    public T FindBehaviour<T>() where T : CharacterBehaviour
    {
      Type BehaviourType = typeof(T);

      foreach (CharacterBehaviour Behaviour in _Behaviours)
      {
        if (Behaviour is T characterBehaviour)
        {
          return characterBehaviour;
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