
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Brain")]
    public class AIBrainBehaviour : MonoBehaviour
    {
      public class AIStateContext
      {
        public Transform transform;
        public Character character;
        public CharacterController2D controller;
        public Aggro aggro;
        public Animator animator;
        public MovementPath movementPath;
        public AIBrain aiBrain;
        public AIBrainBehaviour aiState;
        public bool JustRotated = false;
        public float LastTimeRotated = 0f;
        public float RotateSpeed = 0f;
        public bool RunningCooldown = false;
        public float RunningPower = 0f;
        public float LastTimeRunningCooldown = 0f;
      }

      [Header("AI Brain Settings")]
      public AIBrain AIBrain;
      public AIState CurrentState;
      public Type PreviousStateType { get; private set; }
      public Type CurrentStateType { get; private set; }

      [Header("Decision Tree")]
      [Tooltip("The current AI decision that has been made")]
      public AIDecision CurrentDecision;

      [Tooltip("The previous AI decision that has been made")]
      public AIDecision PreviousDecision;

      [Header("Settings")]
      public Aggro aggro;

      [Tooltip("The speed / time it takes to rotate and make decisions")]
      public float RotateSpeed;

      [Tooltip("The stun cooldown time to start making decisions again")]
      public float StunCooldownTime = 5f;

      [HideInInspector]
      private float _decisionTimeElapsed = 0f;
      private bool _hasAggro = false;
      private float _lastStunTime = 0f;

      private AIBrain _aiBrain;
      private Dictionary<Type, AIState> _states;
      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private Animator _animator;
      private MovementPath _movementPath;
      private AIStateContext _ctx;
      private AIBrain.SettingsSetOverride _defaultSettings;
      private AIBrain.SettingsSetOverride _previousSettings;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
        _movementPath = GetComponent<MovementPath>();
        _defaultSettings = new AIBrain.SettingsSetOverride();
        _previousSettings = new AIBrain.SettingsSetOverride();

        if (AIBrain.DefaultBrain != null)
        {
          _aiBrain = AIBrain.DefaultBrain.CreateNewInstance<AIBrain>();
          _aiBrain.DefaultBrain = null;
          _aiBrain.States = AIBrain.DefaultBrain.States;
          _aiBrain.DefaultState = AIBrain.DefaultBrain.DefaultState;
          _aiBrain.RootDecision = AIBrain.DefaultBrain.RootDecision;
          _aiBrain.AggroRootDecision = AIBrain.DefaultBrain.AggroRootDecision;
          _aiBrain.IdleSettings = AIBrain.DefaultBrain.IdleSettings;
          _aiBrain.AttackSettings = AIBrain.DefaultBrain.AttackSettings;
          _aiBrain.WalkingSettings = AIBrain.DefaultBrain.WalkingSettings;
          _aiBrain.RunningSettings = AIBrain.DefaultBrain.RunningSettings;
          _aiBrain.JumpSettings = AIBrain.DefaultBrain.JumpSettings;
          if (AIBrain.OverrideDefaultStates)
          {
            _aiBrain.States = AIBrain.States;
            _aiBrain.DefaultState = AIBrain.DefaultState;
          }
          if (AIBrain.OverrideDefaultDecisionTrees)
          {
            _aiBrain.RootDecision = AIBrain.RootDecision;
            _aiBrain.AggroRootDecision = AIBrain.AggroRootDecision;
          }
          if (AIBrain.OverrideDefaultSettings)
          {
            _aiBrain.IdleSettings = AIBrain.IdleSettings;
            _aiBrain.AttackSettings = AIBrain.AttackSettings;
            _aiBrain.WalkingSettings = AIBrain.WalkingSettings;
            _aiBrain.RunningSettings = AIBrain.RunningSettings;
            _aiBrain.JumpSettings = AIBrain.JumpSettings;
          }
          if (AIBrain.OverrideDefaultSettingsSetOverrides)
          {
            _aiBrain.SettingsSetOverrides = AIBrain.SettingsSetOverrides;
          }
        }
        else
        {
          _aiBrain = AIBrain;
        }

        _defaultSettings.IdleSettings = _aiBrain.IdleSettings;
        _defaultSettings.AttackSettings = _aiBrain.AttackSettings;
        _defaultSettings.WalkingSettings = _aiBrain.WalkingSettings;
        _defaultSettings.RunningSettings = _aiBrain.RunningSettings;
        _defaultSettings.JumpSettings = _aiBrain.JumpSettings;

        _ctx = new AIStateContext();
        _ctx.transform = _transform;
        _ctx.character = _character;
        _ctx.controller = _controller;
        _ctx.aggro = aggro;
        _ctx.animator = _animator;
        _ctx.movementPath = _movementPath;
        _ctx.aiBrain = _aiBrain;
        _ctx.aiState = this;
        _ctx.RotateSpeed = RotateSpeed;
        if (_aiBrain.RunningSettings != null)
        {
          _ctx.RunningPower = _aiBrain.RunningSettings.RunningPower;
        }

        _states = new Dictionary<Type, AIState>();
        foreach (AIState state in _aiBrain.States)
        {
          _states.Add(state.GetType(), state);
        }

        // Start with the normal decision tree
        CurrentDecision = _aiBrain.RootDecision;
        PreviousDecision = CurrentDecision;
      }

      private void Start()
      {
        ResetToDefaultState();
      }

      private void Update()
      {
        Type newStateType = CurrentState.Execute(_ctx);
        if (newStateType != null)
        {
          ChangeState(newStateType);
        }
      }

      private void LateUpdate()
      {
        if (CurrentDecision != null && CurrentStateType != typeof(AIStunState))
        {
          _decisionTimeElapsed += Time.deltaTime;
          if (_decisionTimeElapsed >= CurrentDecision.DecisionSpeed)
          {
            _decisionTimeElapsed = 0;
            MakeDecision();
          }
        }

        if (CurrentStateType == typeof(AIStunState))
        {
          if (Time.time - _lastStunTime > StunCooldownTime)
          {
            RestorePreviousState();
          }
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
          CurrentState.Exit(_ctx);
        }
        CurrentState = _states[newStateType];
        CurrentStateType = newStateType;
        CurrentState.Enter(_ctx);
      }

      public void ResetToDefaultState()
      {
        CurrentState = null;
        if (_aiBrain.DefaultState != null)
        {
          ChangeState(_aiBrain.DefaultState.GetType());
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

      private void MakeDecision()
      {
        if (CurrentDecision.AIDecisions.Count == 0)
        {
          if (PreviousDecision == null)
          {
            PreviousDecision = CurrentDecision;
            CurrentDecision = _aiBrain.RootDecision;
          }
          else
          {
            CurrentDecision = PreviousDecision;
          }
        }
        else
        {
          int decisionIndex = UnityEngine.Random.Range(0, CurrentDecision.AIDecisions.Count);
          PreviousDecision = CurrentDecision;
          CurrentDecision = CurrentDecision.AIDecisions[decisionIndex];
        }
        if (CurrentDecision.State != null)
        {
          ChangeAIState(CurrentDecision.State.GetType());
        }
      }

      public void RestorePreviousDecision()
      {
        if (PreviousDecision != null)
        {
          CurrentDecision = PreviousDecision;
          ChangeAIState(CurrentDecision.State.GetType());
        }
        else
        {
          CurrentDecision = _aiBrain.RootDecision;
          ChangeAIState(CurrentDecision.State.GetType());
        }
      }

      public void ChangeSettingsSetOverride(int index)
      {
        if (index >= 0 && index < _aiBrain.SettingsSetOverrides.Length)
        {
          AIBrain.SettingsSetOverride overrides = _aiBrain.SettingsSetOverrides[index];
          if (overrides != null)
          {
            if (overrides.IdleSettings != null)
            {
              _previousSettings.IdleSettings = _aiBrain.IdleSettings;
              _aiBrain.IdleSettings = overrides.IdleSettings;
            }
            if (overrides.AttackSettings != null)
            {
              _previousSettings.AttackSettings = _aiBrain.AttackSettings;
              _aiBrain.AttackSettings = overrides.AttackSettings;
            }
            if (overrides.WalkingSettings != null)
            {
              _previousSettings.WalkingSettings = _aiBrain.WalkingSettings;
              _aiBrain.WalkingSettings = overrides.WalkingSettings;
            }
            if (overrides.RunningSettings != null)
            {
              _previousSettings.RunningSettings = _aiBrain.RunningSettings;
              _aiBrain.RunningSettings = overrides.RunningSettings;
            }
            if (overrides.JumpSettings != null)
            {
              _previousSettings.JumpSettings = _aiBrain.JumpSettings;
              _aiBrain.JumpSettings = overrides.JumpSettings;
            }
          }
        }
      }

      public void RestoreDefaultsSettingsSetOverride(int index)
      {
        if (_defaultSettings.IdleSettings != null)
        {
          _aiBrain.IdleSettings = _defaultSettings.IdleSettings;
        }
        if (_defaultSettings.AttackSettings != null)
        {
          _aiBrain.AttackSettings = _defaultSettings.AttackSettings;
        }
        if (_defaultSettings.WalkingSettings != null)
        {
          _aiBrain.WalkingSettings = _defaultSettings.WalkingSettings;
        }
        if (_defaultSettings.RunningSettings != null)
        {
          _aiBrain.RunningSettings = _defaultSettings.RunningSettings;
        }
        if (_defaultSettings.JumpSettings != null)
        {
          _aiBrain.JumpSettings = _defaultSettings.JumpSettings;
        }
      }

      public void RestorePreviousSettingsSetOverride(int index)
      {
        if (_previousSettings.IdleSettings != null)
        {
          _aiBrain.IdleSettings = _previousSettings.IdleSettings;
        }
        if (_previousSettings.AttackSettings != null)
        {
          _aiBrain.AttackSettings = _previousSettings.AttackSettings;
        }
        if (_previousSettings.WalkingSettings != null)
        {
          _aiBrain.WalkingSettings = _previousSettings.WalkingSettings;
        }
        if (_previousSettings.RunningSettings != null)
        {
          _aiBrain.RunningSettings = _previousSettings.RunningSettings;
        }
        if (_previousSettings.JumpSettings != null)
        {
          _aiBrain.JumpSettings = _previousSettings.JumpSettings;
        }
      }

      private void OnAggroChange(bool aggro)
      {
        _hasAggro = aggro;
        if (_aiBrain.AggroRootDecision != null)
        {
          if (_hasAggro)
          {
            PreviousDecision = null;
            CurrentDecision = _aiBrain.AggroRootDecision;
            if (CurrentDecision.State != null)
            {
              ChangeAIState(CurrentDecision.State.GetType());
            }
          }
          else if (!_hasAggro)
          {
            PreviousDecision = null;
            CurrentDecision = _aiBrain.RootDecision;
            if (CurrentDecision.State != null)
            {
              ChangeAIState(CurrentDecision.State.GetType());
            }
          }
        }
      }

      private void ChangeAIState(Type type)
      {
        if (HasState(type))
        {
          ChangeState(CurrentDecision.State.GetType());
        }
        else
        {
          if (_hasAggro)
          {
            CurrentDecision = _aiBrain.AggroRootDecision;
          }
          else
          {
            CurrentDecision = _aiBrain.RootDecision;
          }
        }
      }

      private void OnEnable()
      {
        if (aggro != null)
        {
          aggro.OnAggroChange += OnAggroChange;
        }
      }

      private void OnDisable()
      {
        if (aggro != null)
        {
          aggro.OnAggroChange -= OnAggroChange;
        }
      }

    }
  }
}