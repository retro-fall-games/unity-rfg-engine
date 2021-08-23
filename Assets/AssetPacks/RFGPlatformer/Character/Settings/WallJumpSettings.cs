using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Wall Jump Settings", menuName = "RFG/Platformer/Character/Settings/Wall Jump")]
    public class WallJumpSettings : ScriptableObject
    {
      [Header("Settings")]
      public float Threshold = 0.01f;
      public Vector2 WallJumpForce = new Vector2(10f, 4f);

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for wall clinging")]
      public string WallJumpClip;

      [Header("Effect")]
      public string[] JumpEffects;

    }
  }
}