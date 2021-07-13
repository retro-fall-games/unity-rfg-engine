using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG.Utils;
using RFG.Core;

namespace RFG.Platformer
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
    DoubleJumping,
    WallJumping,
    Jetpacking,
    Crawling,
    Crouching,
    LadderClimbing,
    Dashing,
    Pushing,
    WallClinging
  }

  public static class MovementStatesExtensions
  {
    public static bool IsJumpingState(this MovementStates state)
    {
      return state == MovementStates.Jumping
        || state == MovementStates.DoubleJumping
        || state == MovementStates.WallJumping;
    }
    public static bool CantJumpState(this MovementStates state, bool canWallJump = false)
    {
      return state == MovementStates.Jetpacking
        || state == MovementStates.Dashing
        || state == MovementStates.Pushing
        || (state == MovementStates.WallClinging && !canWallJump);
    }
  }


  [AddComponentMenu("RFG Platformer/Character")]
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
  }
}