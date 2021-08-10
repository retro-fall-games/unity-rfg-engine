using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {

    public enum CharacterType { Player, AI }

    public enum MovementState
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

    public enum AIState
    {
      Idle,
      Wandering,
      MovementPath,
      Attacking
    }

    public enum AIMovementState
    {
      Idle,
      WalkingLeft,
      WalkingRight,
      RunningLeft,
      RunningRight,
      JumpingLeft,
      JumpingRight,
    }

    [AddComponentMenu("RFG/Platformer/Character/Character")]
    public class Character : MonoBehaviour, IPooledObject
    {
      [Header("Settings")]
      public CharacterType CharacterType = CharacterType.Player;
      public bool ObjectPool = false;
      public Transform SpawnAt;

      [HideInInspector]
      public CharacterStateController CharacterState => _characterState;
      public CharacterMovementStateController CharacterMovementState => _movementState;
      public MovementState MovementState => _oldMovementState;
      public CharacterController2D Controller => _controller;
      public CharacterAbilityController Abilities => _abilities;
      public CharacterInputController Input => _input;

      private MovementState _oldMovementState;

      private CharacterStateController _characterState;
      private CharacterMovementStateController _movementState;
      private CharacterController2D _controller;
      private CharacterAbilityController _abilities;
      private CharacterInputController _input;
      private Dictionary<int, LevelPortal> _levelPortals;

      private void Awake()
      {
        _characterState = GetComponent<CharacterStateController>();
        _movementState = GetComponent<CharacterMovementStateController>();
        _controller = GetComponent<CharacterController2D>();
        _abilities = GetComponent<CharacterAbilityController>();
        _input = GetComponent<CharacterInputController>();

        if (CharacterType == CharacterType.Player)
        {
          CalculatePlayerSpawnAt();
        }
      }

      private void Start()
      {
        OnObjectSpawn();
      }

      public void OnObjectSpawn()
      {
        _characterState.Reset();
        _movementState.Reset();
      }

      public void CalculatePlayerSpawnAt()
      {
        SpawnAt = transform;

        _levelPortals = new Dictionary<int, LevelPortal>();
        LevelPortal _levelPortal = GameObject.FindObjectOfType<LevelPortal>();
        if (_levelPortal)
        {
          int index = _levelPortal.index;
          _levelPortals.Add(index, _levelPortal);
        }

        Vector3 spawnAt = CheckpointManager.Instance.CurrentCheckpoint.position;

        // If coming from a level portal the warp the player to that portal
        int levelPortalTo = PlayerPrefs.GetInt("levelPortalTo", -1);
        if (levelPortalTo != -1)
        {
          if (levelPortalTo >= 0 && levelPortalTo <= _levelPortals.Count)
          {
            LevelPortal levelPortal = _levelPortals[levelPortalTo];
            levelPortal.JustWarped = true;
            spawnAt = levelPortal.transform.position + levelPortal.spawnOffset;
            PlayerPrefs.SetInt("levelPortalTo", -1);
          }
        }

        SpawnAt.position = spawnAt;
      }

    }
  }
}