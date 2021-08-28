using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Brain", menuName = "RFG/Platformer/Character/AI State/Brain")]
    public class AIBrain : ScriptableObject
    {
      [Serializable]
      public class SettingsSetOverride
      {
        [Header("Settings")]
        /// <summary>Idle Settings to know effects</summary>
        [Tooltip("Idle Settings to know effects")]
        public IdleSettings IdleSettings;

        /// <summary>Attack Settings to know speed and effects</summary>
        [Tooltip("Attack Settings to know speed and effects")]
        public AttackSettings AttackSettings;

        /// <summary>Walking Settings to know how fast to move horizontally</summary>
        [Tooltip("Walking Settings to know how fast to move horizontally when aggro is false")]
        public WalkingSettings WalkingSettings;

        /// <summary>Running Settings to know how fast to move horizontally</summary>
        [Tooltip("Running Settings to know how fast to move horizontally when aggro is true")]
        public RunningSettings RunningSettings;

        /// <summary>Jump Settings to know how many jumps left and jump restrictions</summary>
        [Tooltip("Jump Settings to know how many jumps left and jump restrictions")]
        public JumpSettings JumpSettings;
      }


      [Header("Brain States")]
      public AIBrain DefaultBrain;
      public bool OverrideDefaultStates = false;
      public bool OverrideDefaultDecisionTrees = false;
      public bool OverrideDefaultSettings = false;
      public bool OverrideDefaultSettingsSetOverrides = false;
      public AIState[] States;
      public AIState DefaultState;

      [Header("Decision Trees")]
      [Tooltip("The AI decision tree to use when aggro is false")]
      public AIDecision RootDecision;

      [Tooltip("The AI decision tree to use when aggro is true")]
      public AIDecision AggroRootDecision;

      [Header("Settings")]
      /// <summary>Idle Settings to know effects</summary>
      [Tooltip("Idle Settings to know effects")]
      public IdleSettings IdleSettings;

      /// <summary>Attack Settings to know speed and effects</summary>
      [Tooltip("Attack Settings to know speed and effects")]
      public AttackSettings AttackSettings;

      /// <summary>Walking Settings to know how fast to move horizontally</summary>
      [Tooltip("Walking Settings to know how fast to move horizontally when aggro is false")]
      public WalkingSettings WalkingSettings;

      /// <summary>Running Settings to know how fast to move horizontally</summary>
      [Tooltip("Running Settings to know how fast to move horizontally when aggro is true")]
      public RunningSettings RunningSettings;

      /// <summary>Jump Settings to know how many jumps left and jump restrictions</summary>
      [Tooltip("Jump Settings to know how many jumps left and jump restrictions")]
      public JumpSettings JumpSettings;

      [Header("Overrides")]
      /// <summary>Settings Set Override, useful for bosses that go into different modes</summary>
      [Tooltip("Settings Set Override, useful for bosses that go into different modes")]
      public SettingsSetOverride[] SettingsSetOverrides;

    }
  }
}