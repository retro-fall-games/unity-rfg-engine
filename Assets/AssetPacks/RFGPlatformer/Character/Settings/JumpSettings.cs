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