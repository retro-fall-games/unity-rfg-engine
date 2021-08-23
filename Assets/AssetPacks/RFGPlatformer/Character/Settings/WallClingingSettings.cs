using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Wall Clinging Settings", menuName = "RFG/Platformer/Character/Settings/Wall Clinging")]
    public class WallClingingSettings : ScriptableObject
    {
      [Header("Settings")]
      [Range(0.01f, 1f)]
      public float WallClingingSlowFactor = 0.6f;
      public float RaycastVerticalOffset = 0f;
      public float WallClingingTolerance = 0.3f;
      public float Threshold = 0.1f;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for wall clinging")]
      public string WallClingingClip;

      [Header("Effects")]
      public string[] WallClingingEffects;

    }
  }
}