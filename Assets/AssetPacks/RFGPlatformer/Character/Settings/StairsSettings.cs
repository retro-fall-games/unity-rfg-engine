using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Stairs Settings", menuName = "RFG/Platformer/Character/Settings/Stairs")]
    public class StairsSettings : ScriptableObject
    {
      [Header("Input Setting")]
      /// the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)
      [Tooltip("the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)")]
      public Vector2 Threshold = new Vector2(0.1f, 0.4f);

      [Header("Ahead stairs detection")]

      /// the offset to apply when raycasting for stairs
      [Tooltip("the offset to apply when raycasting for stairs")]
      public Vector3 StairsAheadDetectionRaycastOrigin = new Vector3(-2f, 0f, 0f);

      /// the length of the raycast looking for stairs
      [Tooltip("the length of the raycast looking for stairs")]
      public float StairsAheadDetectionRaycastLength = 4f;

      /// the offset to apply when raycasting for stairs
      [Tooltip("the offset to apply when raycasting for stairs")]
      public Vector3 StairsBelowDetectionRaycastOrigin = new Vector3(-0.2f, 0f, 0f);

      /// the length of the raycast looking for stairs
      [Tooltip("the length of the raycast looking for stairs")]
      public float StairsBelowDetectionRaycastLength = 0.5f;

      /// the duration, in seconds, during which collisions with one way platforms should be ignored when starting to get down a stair
      [Tooltip("the duration, in seconds, during which collisions with one way platforms should be ignored when starting to get down a stair")]
      public float StairsBelowLockTime = 0.2f;
    }
  }
}