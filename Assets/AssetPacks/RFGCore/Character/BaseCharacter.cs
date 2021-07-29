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
    Dashing,
    Knockback
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

  public enum CharacterAbilities
  {
    DoubleJump,
    WallCling,
    WallJump,
    Dash,
  }

  public class BaseCharacter : MonoBehaviour, IPooledObject
  {
    [Header("Settings")]
    public CharacterType characterType = CharacterType.Player;
    public bool destroyOnKill = true;
    public bool deactivateOnKill = true;

    [Header("Audio")]
    public string[] spawnSoundFx;
    public string[] deathSoundFx;

    [HideInInspector]
    public StateMachine<CharacterStates> CharacterState => _characterState;
    public StateMachine<MovementStates> MovementState => _movementState;
    public StateMachine<AIStates> AIState => _aiState;
    public StateMachine<AIMovementStates> AIMovementState => _aiMovementState;
    public InputManager InputManager => _inputMananger;
    private StateMachine<CharacterStates> _characterState;
    private StateMachine<MovementStates> _movementState;
    private StateMachine<AIStates> _aiState;
    private StateMachine<AIMovementStates> _aiMovementState;
    private List<BaseCharacterBehaviour> _Behaviours;
    private InputManager _inputMananger;
    public event Action OnBirth;
    public event Action OnKill;

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

      // Create all the Behaviours
      _Behaviours = new List<BaseCharacterBehaviour>();
      _Behaviours.AddRange(GetComponents<BaseCharacterBehaviour>());
    }

    private void Start()
    {
      InitBehaviours();
    }

    public void OnObjectSpawn()
    {
      Birth();
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
      foreach (BaseCharacterBehaviour Behaviour in _Behaviours)
      {
        Behaviour.InitBehaviour();
      }
    }


    private void EarlyProcessBehaviours()
    {
      foreach (BaseCharacterBehaviour Behaviour in _Behaviours)
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
      foreach (BaseCharacterBehaviour Behaviour in _Behaviours)
      {
        Behaviour.ProcessBehaviour();
      }
    }

    private void LateProcessBehaviours()
    {
      foreach (BaseCharacterBehaviour Behaviour in _Behaviours)
      {
        Behaviour.LateProcessBehaviour();
      }
    }

    public T FindBehaviour<T>() where T : BaseCharacterBehaviour
    {
      Type BehaviourType = typeof(T);

      foreach (BaseCharacterBehaviour Behaviour in _Behaviours)
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
      // LogExt.Log<BaseCharacter>("Character State Change: " + state);
    }

    private void OnMovementStateChange(MovementStates state)
    {
      // LogExt.Log<BaseCharacter>("Movement State Change: " + state);
    }

    private void OnAIStateChange(AIStates state)
    {
      // LogExt.Log<BaseCharacter>("AI State Change: " + state);
    }

    private void OnAIMovementStateChange(AIMovementStates state)
    {
      // LogExt.Log<BaseCharacter>("AI Movement State Change: " + state);
    }

    public virtual void Birth()
    {
      if (spawnSoundFx != null && spawnSoundFx.Length > 0)
      {
        FXAudio.Instance.Play(spawnSoundFx, false);
      }
      _characterState.ChangeState(CharacterStates.Alive);
      HealthBehaviour health = FindBehaviour<HealthBehaviour>();
      if (health != null)
      {
        health.Reset();
      }
      gameObject.SetActive(true);
      OnBirth?.Invoke();
    }

    public virtual void Kill()
    {
      if (deathSoundFx != null && deathSoundFx.Length > 0)
      {
        FXAudio.Instance.Play(deathSoundFx, false);
      }
      _characterState.ChangeState(CharacterStates.Dead);
      if (destroyOnKill)
      {
        Destroy(gameObject);
      }
      else if (deactivateOnKill)
      {
        gameObject.SetActive(false);
      }
      OnKill?.Invoke();
    }

  }
}