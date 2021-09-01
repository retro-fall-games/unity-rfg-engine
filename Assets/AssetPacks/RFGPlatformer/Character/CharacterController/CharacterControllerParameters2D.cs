using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Character Controller Parameters 2D", menuName = "RFG/Platformer/Character/Character Controller Parameters 2D")]
    public class CharacterControllerParameters2D : ScriptableObject
    {
      [Header("Gravity")]

      /// The force to apply vertically at all times
      [Tooltip("The force to apply vertically at all times")]
      public float Gravity = -25f;

      /// a multiplier applied to the character's gravity when going down
      [Tooltip("a multiplier applied to the character's gravity when going down")]
      public float FallMultiplier = 1f;

      /// a multiplier applied to the character's gravity when going up
      [Tooltip("a multiplier applied to the character's gravity when going up")]
      public float AscentMultiplier = 1f;

      [Header("Speed")]
      public Vector2 MaxVelocity = new Vector2(100f, 100f);
      public float SpeedFactor = 3f;
      public float GroundSpeedFactor = 10f;
      public float AirSpeedFactor = 5f;

      [Header("Slopes")]
      [Range(0, 90)]
      public float MaxSlopeAngle = 30f;

      /// the speed multiplier to apply when walking on a slope
      [Tooltip("the speed multiplier to apply when walking on a slope")]
      public AnimationCurve SlopeAngleSpeedFactor = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 1f));

      [Header("Parameters")]
      public float SkinWidth = 0.02f;
      public float Weight = 1f;

      [Header("Physics2D Interaction [Experimental]")]

      /// if set to true, the character will transfer its force to all the rigidbodies it collides with horizontally
      [Tooltip("if set to true, the character will transfer its force to all the rigidbodies it collides with horizontally")]
      public bool Physics2DInteraction = true;
      /// the force applied to the objects the character encounters
      [Tooltip("the force applied to the objects the character encounters")]
      public float Physics2DPushForce = 2.0f;

    }
  }
}