using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Walking Settings", menuName = "RFG/Platformer/Character/Settings/Walking")]
    public class WalkingSettings : ScriptableObject
    {
      [Header("Settings")]
      public float WalkingSpeed = 5f;

      [Header("Effects")]
      public string[] WalkingEffects;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for walking")]
      public string WalkingClip;
      [Tooltip("Define what animation to play for idle")]
      public string IdleClip;
    }
  }
}