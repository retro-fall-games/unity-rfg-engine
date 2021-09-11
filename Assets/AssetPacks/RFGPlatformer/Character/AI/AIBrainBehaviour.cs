
using System;
using System.Collections.Generic;
using UnityEngine;
using RFG.BehaviourTree;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Brain")]
    public class AIBrainBehaviour : MonoBehaviour, INodeContext
    {
      [Header("AI Brain Settings")]
      public AIBrain DefaultAIBrain;

      [HideInInspector] public AIBrain CurrentAIBrain;
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

      [Header("Weapons To Equip")]
      public WeaponItem PrimaryWeapon;
      public WeaponItem SecondaryWeapon;

      [HideInInspector]
      public AIStateContext Context => _ctx;
      public bool HasAggro { get; set; }
      private float _decisionTimeElapsed = 0f;
      private float _lastStunTime = 0f;

      private Dictionary<Type, AIState> _states;
      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private Animator _animator;
      private MovementPath _movementPath;
      private EquipmentSet _equipmentSet;
      private AIStateContext _ctx;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
        _movementPath = GetComponent<MovementPath>();
        _equipmentSet = GetComponent<EquipmentSet>();

        // Override any base brain properties
        ResetAIBrain();

        _states = new Dictionary<Type, AIState>();
        foreach (AIState state in CurrentAIBrain.States)
        {
          _states.Add(state.GetType(), state);
        }

        // Start with the normal decision tree
        CurrentDecision = CurrentAIBrain.RootDecision;
        PreviousDecision = CurrentDecision;

        // Equip Weapons
        if (PrimaryWeapon != null)
        {
          _equipmentSet.EquipPrimaryWeapon(PrimaryWeapon);
        }
        if (SecondaryWeapon != null)
        {
          _equipmentSet.EquipSecondaryWeapon(SecondaryWeapon);
        }
      }

      private void Start()
      {
        _ctx = new AIStateContext();
        _ctx.transform = _transform;
        _ctx.character = _character;
        _ctx.characterContext = _character.Context;
        _ctx.controller = _controller;
        _ctx.aggro = aggro;
        _ctx.animator = _animator;
        _ctx.movementPath = _movementPath;
        _ctx.equipmentSet = _equipmentSet;
        _ctx.aiBrain = CurrentAIBrain;
        _ctx.aiState = this;
        _ctx.RotateSpeed = RotateSpeed;
        ResetToDefaultState();
      }

      private void LateUpdate()
      {
        // if (CurrentState != null)
        // {
        //   Type newStateType = CurrentState.Execute(_ctx);
        //   if (newStateType != null)
        //   {
        //     ChangeState(newStateType);
        //   }
        // }

        // if (CurrentDecision != null && CurrentStateType != typeof(AIStunState))
        // {
        //   _decisionTimeElapsed += Time.deltaTime;
        //   if (_decisionTimeElapsed >= CurrentDecision.DecisionSpeed)
        //   {
        //     _decisionTimeElapsed = 0;
        //     MakeDecision();
        //   }
        // }

        // if (CurrentStateType == typeof(AIStunState))
        // {
        //   if (Time.time - _lastStunTime > StunCooldownTime)
        //   {
        //     RestorePreviousState();
        //   }
        // }
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
        if (CurrentAIBrain.DefaultState != null)
        {
          ChangeState(CurrentAIBrain.DefaultState.GetType());
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
            CurrentDecision = CurrentAIBrain.RootDecision;
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
          CurrentDecision = CurrentAIBrain.RootDecision;
          ChangeAIState(CurrentDecision.State.GetType());
        }
      }

      private void OnAggroChange(bool aggro)
      {
        HasAggro = aggro;
        if (CurrentAIBrain.AggroRootDecision != null)
        {
          if (HasAggro)
          {
            PreviousDecision = null;
            CurrentDecision = CurrentAIBrain.AggroRootDecision;
            if (CurrentDecision.State != null)
            {
              ChangeAIState(CurrentDecision.State.GetType());
            }
          }
          else if (!HasAggro)
          {
            PreviousDecision = null;
            CurrentDecision = CurrentAIBrain.RootDecision;
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
          if (HasAggro)
          {
            CurrentDecision = CurrentAIBrain.AggroRootDecision;
          }
          else
          {
            CurrentDecision = CurrentAIBrain.RootDecision;
          }
        }
      }

      public void OverrideAIBrain(AIBrain brain)
      {
        if (brain == null)
          return;

        AIBrain brainClone = brain.CloneInstance<AIBrain>();

        if (brainClone.BaseAIBrain != null)
        {
          OverrideAIBrain(brainClone.BaseAIBrain);
        }

        if (brainClone.States != null && brainClone.States.Length > 0)
        {
          CurrentAIBrain.States = brainClone.States;
        }

        if (brainClone.DefaultState != null)
        {
          CurrentAIBrain.DefaultState = brainClone.DefaultState;
        }

        if (brainClone.RootDecision != null)
        {
          CurrentAIBrain.RootDecision = brainClone.RootDecision;
        }

        if (brainClone.AggroRootDecision != null)
        {
          CurrentAIBrain.AggroRootDecision = brainClone.AggroRootDecision;
        }

        if (brainClone.CanFollowVertically != CurrentAIBrain.CanFollowVertically)
        {
          CurrentAIBrain.CanFollowVertically = brainClone.CanFollowVertically;
        }
      }

      public void ResetAIBrain()
      {
        CurrentAIBrain = DefaultAIBrain;
        OverrideAIBrain(DefaultAIBrain.BaseAIBrain);
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