using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Decision Node", menuName = "RFG/Platformer/Tree/AI Decision Node")]
    public class AIDecision : ScriptableObject
    {
      [Header("Settings")]
      [Tooltip("How often a decision will be made")]
      public float DecisionSpeed = 3f;
      [Tooltip("The weight offset of making a decision")]
      public int DecisionOffset = 10;

      [Header("AI State")]
      [Tooltip("The AI state that is associated with this decision")]
      public AIState State;

      [Header("Nodes")]
      [Tooltip("The tree nodes of all AI decisions to be made")]
      public List<AIDecision> AIDecisions;
    }
  }
}