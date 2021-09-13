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
    }
  }
}