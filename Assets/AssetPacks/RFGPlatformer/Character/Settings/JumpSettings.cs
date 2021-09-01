using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Jump Settings", menuName = "RFG/Platformer/Character/Settings/Jump")]
    public class JumpSettings : ScriptableObject
    {
      public enum JumpRestrictions
      {
        CanJumpOnGround,
        CanJumpAnywhere,
        CantJump
      }

      [Header("Jump Parameters")]

      /// <summary>How high can the controller jump</summary>
      [Tooltip("How high can the controller jump")]
      public float JumpHeight = 12f;
      public float OneWayPlatformFallVelocity = -10f;

      /// the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)
      [Tooltip("the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)")]
      public Vector2 JumpThreshold = new Vector2(0.1f, 0.4f);

      /// duration (in seconds) we need to disable collisions when jumping down a 1 way platform
      [Tooltip("duration (in seconds) we need to disable collisions when jumping down a 1 way platform")]
      public float OneWayPlatformsJumpCollisionOffDuration = 0.3f;

      /// duration (in seconds) we need to disable collisions when jumping off a moving platform
      [Tooltip("duration (in seconds) we need to disable collisions when jumping off a moving platform")]
      public float MovingPlatformsJumpCollisionOffDuration = 0.05f;

      [Header("Jump Restrictions")]
      public JumpRestrictions Restrictions;
      public int NumberOfJumps = 1;
      public bool CanJumpDownOneWayPlatforms = true;

      [Header("Proportional Jumps")]
      public bool JumpIsProportionalToThePressTime = true;
      public float JumpMinAirTime = 0.1f;
      public float JumpReleaseForceFactor = 2f;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for jumping")]
      public string JumpingClip;

      [Header("Effects")]
      public string[] JumpEffects;
      public string[] LandEffects;

    }
  }
}