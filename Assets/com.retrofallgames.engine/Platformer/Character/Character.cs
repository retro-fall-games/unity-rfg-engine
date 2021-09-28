using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Platformer
{
  using Core;
  using Interactions;

  public enum CharacterType { Player, AI }
  [AddComponentMenu("RFG/Platformer/Character/Character")]
  public class Character : MonoBehaviour, IPooledObject
  {
    [Header("Type")]
    public CharacterType CharacterType = CharacterType.Player;

    [Header("Location")]
    public Transform SpawnAt;

    [Header("Settings")]
    public InputPack InputPack;
    public SettingsPack SettingsPack;

    [Header("Character State")]
    public RFG.StateMachine.StateMachine CharacterState;

    [Header("Movement State")]
    public RFG.StateMachine.StateMachine MovementState;

    [HideInInspector]
    public StateCharacterContext Context => _characterContext;
    public CharacterController2D Controller => _controller;
    private StateCharacterContext _characterContext = new StateCharacterContext();
    private CharacterController2D _controller;
    private Dictionary<int, LevelPortal> _levelPortals;
    private List<Component> _abilities;

    private void Awake()
    {
      InitContext();
      InitAbilities();
      InitLevelPortals();
    }

    private void InitContext()
    {
      _characterContext = new StateCharacterContext();
      _controller = GetComponent<CharacterController2D>();
      _characterContext.transform = transform;
      _characterContext.animator = GetComponent<Animator>();
      _characterContext.character = this;
      _characterContext.controller = _controller;
      _characterContext.inputPack = InputPack;
      _characterContext.DefaultSettingsPack = SettingsPack;
      _characterContext.healthBehaviour = GetComponent<HealthBehaviour>();

      // Bind the character context to the state context
      CharacterState.Init();
      CharacterState.Bind(_characterContext);

      MovementState.Init();
      MovementState.Bind(_characterContext);
    }

    private void InitAbilities()
    {
      Component[] abilities = GetComponents(typeof(IAbility)) as Component[];
      if (abilities.Length > 0)
      {
        _abilities = new List<Component>(abilities);
      }
    }

    private void InitLevelPortals()
    {
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

    private void Update()
    {
      CharacterState.Update();
      MovementState.Update();
    }

    public void SetMovementStatePack(RFG.StateMachine.StatePack statePack)
    {
      MovementState.SetStatePack(statePack);
    }

    public void RestoreDefaultMovementStatePack()
    {
      MovementState.RestoreDefaultStatePack();
    }

    public void OverrideSettingsPack(SettingsPack settings)
    {
      _characterContext.OverrideSettingsPack(settings);
    }

    public void ResetSettingsPack()
    {
      _characterContext.ResetSettingsPack();
    }

    public void OnObjectSpawn(params object[] objects)
    {
      CharacterState.ResetToDefaultState();
      MovementState.ResetToDefaultState();
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
          if (_levelPortals.ContainsKey(levelPortalTo))
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
      CharacterState.ChangeState(typeof(DeathState));
    }

    public IEnumerator Respawn()
    {
      yield return new WaitForSecondsRealtime(1f);
      CharacterState.ChangeState(typeof(SpawnState));
      gameObject.SetActive(true);
    }

    public void EnableAllAbilities()
    {
      if (_abilities != null)
      {
        foreach (Behaviour ability in _abilities)
        {
          ability.enabled = true;
        }
      }
    }

    public void DisableAllAbilities()
    {
      if (_abilities != null)
      {
        foreach (Behaviour ability in _abilities)
        {
          ability.enabled = false;
        }
      }
    }

    private void OnEnable()
    {
      if (_characterContext.inputPack != null)
      {
        if (_characterContext.inputPack.Movement != null)
        {
          _characterContext.inputPack.Movement.action.Enable();
        }
      }
    }

    private void OnDisable()
    {
      if (_characterContext.inputPack != null)
      {
        if (_characterContext.inputPack.Movement != null)
        {
          _characterContext.inputPack.Movement.action.Disable();
        }
      }
    }

  }
}