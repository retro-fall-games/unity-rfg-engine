using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Idle Settings", menuName = "RFG/Platformer/Character/AI Settings/Idle")]
    public class AIIdleSettings : ScriptableObject
    {
      [Header("Effects")]
      public string[] IdleEffects;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for running")]
      public string IdleClip;
    }
  }
}