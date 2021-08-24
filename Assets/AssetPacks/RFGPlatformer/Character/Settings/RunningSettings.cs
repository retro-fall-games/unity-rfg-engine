using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Running Settings", menuName = "RFG/Platformer/Character/Settings/Running")]
    public class RunningSettings : ScriptableObject
    {
      [Header("Settings")]
      public float RunningSpeed = 5f;

      [Header("Effects")]
      public string[] RunningEffects;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for walking")]
      public string RunningClip;
    }
  }
}