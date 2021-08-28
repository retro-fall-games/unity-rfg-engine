using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public enum CharacterType { Player, AI }
    [AddComponentMenu("RFG/Platformer/Character/Character")]
    public class Character : StateMachineBehaviour, IPooledObject
    {
      [Header("Settings")]
      public CharacterType CharacterType = CharacterType.Player;
      public Transform SpawnAt;

      [HideInInspector]
      public CharacterController2D Controller => _controller;
      private CharacterController2D _controller;
      private Dictionary<int, LevelPortal> _levelPortals;

      protected override void Awake()
      {
        base.Awake();
        _controller = GetComponent<CharacterController2D>();
        if (CharacterType == CharacterType.Player)
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
        ResetToDefaultState();
      }

      public void CalculatePlayerSpawnAt()
      {
        SpawnAt = transform;

        Vector3 spawnAt = CheckpointManager.Instance.CurrentCheckpoint.position;

        if (CharacterType == CharacterType.Player)
        {
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
        }

        SpawnAt.position = spawnAt;
      }

      public void Kill()
      {
        StartCoroutine(KillCo());
      }

      public IEnumerator KillCo()
      {
        yield return new WaitForSeconds(0.1f);
        ChangeState(typeof(DeathState));
      }

      public IEnumerator Respawn()
      {
        yield return new WaitForSecondsRealtime(1f);
        ChangeState(typeof(SpawnState));
        gameObject.SetActive(true);
      }

    }
  }
}