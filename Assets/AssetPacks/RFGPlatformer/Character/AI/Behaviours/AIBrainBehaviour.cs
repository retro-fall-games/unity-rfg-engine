using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Brain")]
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

      private void CheckAggro()
      {
        if (_aggro != null && _aggro.HasAggro)
        {
          PreviousDecision = null;
          CurrentDecision = AggroRootDecision;
          if (CurrentDecision.State != null)
          {
            _character.ChangeState(CurrentDecision.State.GetType());
          }
        }
        else
        {
          PreviousDecision = null;
          CurrentDecision = RootDecision;
          if (CurrentDecision.State != null)
          {
            _character.ChangeState(CurrentDecision.State.GetType());
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
          int decisionIndex = UnityEngine.Random.Range(0, CurrentDecision.Decisions.Count - 1);
          PreviousDecision = CurrentDecision;
          CurrentDecision = CurrentDecision.Decisions[decisionIndex];
        }
        if (CurrentDecision.State != null)
        {
          _character.ChangeState(CurrentDecision.State.GetType());
        }
      }

    }
  }
}