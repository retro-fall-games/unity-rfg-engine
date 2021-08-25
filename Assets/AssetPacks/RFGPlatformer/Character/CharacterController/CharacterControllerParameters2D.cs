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
      public float Gravity = -25f;

      [Header("Speed")]
      public Vector2 MaxVelocity = new Vector2(100f, 100f);
      public float SpeedFactor = 3f;
      public float GroundSpeedFactor = 10f;
      public float AirSpeedFactor = 5f;

      [Header("Slopes")]
      [Range(0, 90)]
      public float MaxSlopeAngle = 30f;

      [Header("Parameters")]
      public float SkinWidth = 0.02f;
      public float Weight = 1f;

    }
  }
}