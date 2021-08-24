using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Attack Settings", menuName = "RFG/Platformer/Character/AI Settings/Attack")]
    public class AIAttackSettings : ScriptableObject
    {
      [Header("Settings")]
      public float RunSpeed = 5f;
      public float AttackSpeed = 5f;

      [Header("Effects")]
      public string[] RunningEffects;
      public string[] AttackEffects;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for running")]
      public string RunningClip;

      [Tooltip("Define what animation to play for attacking")]
      public string AttackClip;

    }
  }
}