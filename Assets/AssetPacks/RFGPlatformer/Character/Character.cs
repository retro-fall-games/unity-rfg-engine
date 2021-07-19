using System;
using System.Collections;
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

  public enum AIStates
  {
    Idle,
    Wandering,
    MovementPath,
    Attacking
  }

  public enum AIMovementStates
  {
    Idle,
    WalkingLeft,
    WalkingRight,
    RunningLeft,
    RunningRight,
    JumpingLeft,
    JumpingRight,
  }


  [AddComponentMenu("RFG Platformer/Character/Character")]
  public class Character : MonoBehaviour
  {
    [Header("Settings")]
    public CharacterType characterType = CharacterType.Player;

    [Header("Debug")]
    public bool showDebugLog = false;

    [HideInInspector]
    public StateMachine<CharacterStates> CharacterState => _characterState;
    public StateMachine<MovementStates> MovementState => _movementState;
    public StateMachine<AIStates> AIState => _aiState;
    public StateMachine<AIMovementStates> AIMovementState => _aiMovementState;
    public CharacterController2D Controller => _controller;
    public CharacterInput CharacterInput => _characterInput;

    private StateMachine<CharacterStates> _characterState;
    private StateMachine<MovementStates> _movementState;
    private StateMachine<AIStates> _aiState;
    private StateMachine<AIMovementStates> _aiMovementState;
    private List<CharacterBehaviour> _Behaviours;
    private CharacterController2D _controller;
    private CharacterInput _characterInput;
    private TickStateMachine _aiStateMachine;

    protected virtual void Awake()
    {
      // States
      _characterState = new StateMachine<CharacterStates>(gameObject, true);
      _movementState = new StateMachine<MovementStates>(gameObject, true);

      // State Listeners
      _characterState.OnStateChange += OnCharacterStateChange;
      _movementState.OnStateChange += OnMovementStateChange;

      // AI
      if (characterType == CharacterType.AI)
      {
        _aiState = new StateMachine<AIStates>(gameObject, true);
        _aiMovementState = new StateMachine<AIMovementStates>(gameObject, true);

        // State Listeners
        _aiState.OnStateChange += OnAIStateChange;
        _aiMovementState.OnStateChange += OnAIMovementStateChange;

        // Default AI state
        _aiState.ChangeState(AIStates.Wandering);
        _aiMovementState.ChangeState(AIMovementStates.Idle);
      }

      // Controllers
      _controller = GetComponent<CharacterController2D>();
      _characterInput = GetComponent<CharacterInput>();

      // Create all the Behaviours
      _Behaviours = new List<CharacterBehaviour>();
      _Behaviours.AddRange(GetComponents<CharacterBehaviour>());

      HealthBehaviour health = FindBehaviour<HealthBehaviour>();

      if (health != null)
      {
        health.OnKill += Kill;
      }

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

    private void OnCharacterStateChange(CharacterStates state)
    {
      if (showDebugLog)
      {
        Debug.Log("Character State Change: " + state);
      }
    }

    private void OnMovementStateChange(MovementStates state)
    {
      if (showDebugLog)
      {
        Debug.Log("Movement State Change: " + state);
      }
    }

    private void OnAIStateChange(AIStates state)
    {
      if (showDebugLog)
      {
        Debug.Log("AI State Change: " + state);
      }
    }

    private void OnAIMovementStateChange(AIMovementStates state)
    {
      if (showDebugLog)
      {
        Debug.Log("AI Movement State Change: " + state);
      }
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
      StartCoroutine(KillCo());

    }

    private IEnumerator KillCo()
    {
      yield return new WaitForSeconds(0.1f);
      if (characterType == CharacterType.Player)
      {
        EventManager.TriggerEvent(new PlayerKillEvent(this));
      }
      else
      {
        EventManager.TriggerEvent(new CharacterKillEvent(this));
        Destroy(gameObject);
      }
    }

  }
}