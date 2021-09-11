using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Brain", menuName = "RFG/Platformer/Character/AI State/Brain")]
    public class AIBrain : ScriptableObject
    {
      [Header("States")]
      public AIState[] States;
      public AIState DefaultState;

      [Header("Decision Trees")]
      [Tooltip("The AI decision tree to use when aggro is false")]
      public AIDecision RootDecision;

      [Tooltip("The AI decision tree to use when aggro is true")]
      public AIDecision AggroRootDecision;

      [Header("Settings")]

      /// <summary>This is meant for flying ai, can the ai follow vertically</summary>
      [Tooltip("This is meant for flying ai, can the ai follow vertically")]
      public bool CanFollowVertically = false;

      [Header("Base")]
      public AIBrain BaseAIBrain;

    }
  }
}