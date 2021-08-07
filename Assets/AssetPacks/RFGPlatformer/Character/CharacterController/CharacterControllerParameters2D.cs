using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [Serializable]
    public class CharacterControllerParameters2D
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

    }
  }
}