using System.Collections.Generic;
using UnityEngine;

namespace RFG
{

  [CreateAssetMenu(fileName = "New Decision Node", menuName = "RFG/Tree/Decision Node")]
  public class Decision : ScriptableObject
  {
    [Header("Settings")]
    [Tooltip("How often a decision will be made")]
    public float DecisionSpeed = 3f;
    [Tooltip("The weight offset of making a decision")]
    public int DecisionOffset = 10;

    [Header("State")]
    [Tooltip("The state that is associated with this decision")]
    public State State;

    [Header("Nodes")]
    [Tooltip("The tree nodes of all decisions to be made")]
    public List<Decision> Decisions;
  }
}

