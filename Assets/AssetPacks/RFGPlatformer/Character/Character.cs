using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public enum CharacterType { Player, AI }
    [AddComponentMenu("RFG/Platformer/Character/Character")]
    public class Character : MonoBehaviour, IPooledObject
    {
      [Header("Settings")]
      public CharacterType CharacterType = CharacterType.Player;
      public Transform SpawnAt;

      [HideInInspector]
      public CharacterStateController CharacterState => _characterState;
      public CharacterMovementStateController CharacterMovementState => _movementState;
      public CharacterController2D Controller => _controller;
      public CharacterAbilityController Abilities => _abilities;
      public CharacterBehaviourController Behaviours => _behaviours;
      public CharacterInputController Input => _input;
      public CharacterAIStateController AIState => _aiState;
      public CharacterAIMovementStateController AIMovementState => _aiMovementState;

      private CharacterStateController _characterState;
      private CharacterMovementStateController _movementState;
      private CharacterController2D _controller;
      private CharacterAbilityController _abilities;
      private CharacterBehaviourController _behaviours;
      private CharacterInputController _input;
      private CharacterAIStateController _aiState;
      private CharacterAIMovementStateController _aiMovementState;
      private Dictionary<int, LevelPortal> _levelPortals;

      private void Awake()
      {
        _characterState = GetComponent<CharacterStateController>();
        _movementState = GetComponent<CharacterMovementStateController>();
        _controller = GetComponent<CharacterController2D>();
        _abilities = GetComponent<CharacterAbilityController>();
        _behaviours = GetComponent<CharacterBehaviourController>();
        _input = GetComponent<CharacterInputController>();

        if (CharacterType != CharacterType.Player)
        {
          _aiState = GetComponent<CharacterAIStateController>();
          _aiMovementState = GetComponent<CharacterAIMovementStateController>();
        }
        else
        {
          _levelPortals = new Dictionary<int, LevelPortal>();
          LevelPortal[] levelPortals = GameObject.FindObjectsOfType<LevelPortal>();
          foreach (LevelPortal portal in levelPortals)
          {
            _levelPortals.Add(portal.Index, portal);
          }
        }
      }

      public void OnObjectSpawn(params object[] objects)
      {
        _characterState.Reset();
        _movementState.Reset();
        if (CharacterType == CharacterType.AI)
        {
          Controller.ResetVelocity();
          Controller.enabled = true;
          _characterState.ChangeState(typeof(AliveState));
        }
      }

      public void CalculatePlayerSpawnAt()
      {
        SpawnAt = transform;

        Vector3 spawnAt = CheckpointManager.Instance.CurrentCheckpoint.position;

        // If coming from a level portal the warp the player to that portal
        int levelPortalTo = PlayerPrefs.GetInt("levelPortalTo", -1);
        if (levelPortalTo != -1)
        {
          PlayerPrefs.SetInt("levelPortalTo", -1);
          if (levelPortalTo >= 0 && levelPortalTo <= _levelPortals.Count)
          {
            LevelPortal levelPortal = _levelPortals[levelPortalTo];
            spawnAt = levelPortal.transform.position + levelPortal.SpawnOffset;
          }
        }

        SpawnAt.position = spawnAt;
      }

      public void Kill()
      {
        _characterState.ChangeState(typeof(DeathState));
      }

      public IEnumerator Respawn()
      {
        yield return new WaitForSecondsRealtime(1f);
        _characterState.ChangeState(typeof(SpawnState));
        gameObject.SetActive(true);
      }

    }
  }
}