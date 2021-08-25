
using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Brain")]
    public class AIBrainBehaviour : MonoBehaviour
    {
      [Header("Decision Trees")]

      [Tooltip("The decision tree to use when aggro is false")]
      public Decision RootDecision;

      [Tooltip("The decision tree to use when aggro is true")]
      public Decision AggroRootDecision;

      [Tooltip("The current decision that has been made")]
      public Decision CurrentDecision;

      [Tooltip("The previous decision that has been made")]
      public Decision PreviousDecision;

      [HideInInspector]
      private float _decisionTimeElapsed = 0f;
      private Character _character;
      private Aggro _aggro;
      private bool _hasAggro;

      private void Awake()
      {
        _character = GetComponent<Character>();
        _aggro = GetComponent<Aggro>();

        // Start with the normal decision tree
        CurrentDecision = RootDecision;
        PreviousDecision = CurrentDecision;
      }

      private void LateUpdate()
      {
        if (CurrentDecision != null)
        {
          _decisionTimeElapsed += Time.deltaTime;
          if (_decisionTimeElapsed >= CurrentDecision.DecisionSpeed)
          {
            _decisionTimeElapsed = 0;
            MakeDecision();
          }
        }
      }

      private void MakeDecision()
      {
        if (CurrentDecision.Decisions.Count == 0)
        {
          if (PreviousDecision == null)
          {
            PreviousDecision = CurrentDecision;
            CurrentDecision = RootDecision;
          }
          else
          {
            CurrentDecision = PreviousDecision;
          }
        }
        else
        {
          int decisionIndex = UnityEngine.Random.Range(0, CurrentDecision.Decisions.Count);
          PreviousDecision = CurrentDecision;
          CurrentDecision = CurrentDecision.Decisions[decisionIndex];
        }
        if (CurrentDecision.State != null)
        {
          ChangeState(CurrentDecision.State.GetType());
        }
      }

      public void RestorePreviousDecision()
      {
        if (PreviousDecision != null)
        {
          CurrentDecision = PreviousDecision;
          ChangeState(CurrentDecision.State.GetType());
        }
        else
        {
          CurrentDecision = RootDecision;
          ChangeState(CurrentDecision.State.GetType());
        }
      }

      private void OnAggroChange(bool aggro)
      {
        _hasAggro = aggro;
        if (AggroRootDecision != null)
        {
          if (_hasAggro)
          {
            PreviousDecision = null;
            CurrentDecision = AggroRootDecision;
            if (CurrentDecision.State != null)
            {
              ChangeState(CurrentDecision.State.GetType());
            }
          }
          else if (!_hasAggro)
          {
            PreviousDecision = null;
            CurrentDecision = RootDecision;
            if (CurrentDecision.State != null)
            {
              ChangeState(CurrentDecision.State.GetType());
            }
          }
        }
      }

      private void ChangeState(Type type)
      {
        if (_character.HasState(type))
        {
          _character.ChangeState(CurrentDecision.State.GetType());
        }
        else
        {
          if (_hasAggro)
          {
            CurrentDecision = AggroRootDecision;
          }
          else
          {
            CurrentDecision = RootDecision;
          }
        }
      }

      private void OnEnable()
      {
        if (_aggro != null)
        {
          _aggro.OnAggroChange += OnAggroChange;
        }
      }

      private void OnDisable()
      {
        if (_aggro != null)
        {
          _aggro.OnAggroChange -= OnAggroChange;
        }
      }

    }
  }
}