using System;
using UnityEngine;

namespace RFG
{
  [Serializable]
  public class CharacterControllerParameters2D
  {
    [Header("Gravity")]
    public float gravity = -25f;

    [Header("Speed")]
    public Vector2 maxVelocity = new Vector2(100f, 100f);
    public float speedFactor = 3f;
    public float groundSpeedFactor = 10f;
    public float airSpeedFactor = 5f;

    [Header("Slopes")]
    [Range(0, 90)]
    public float maxSlopeAngle = 30f;

    [Header("Parameters")]
    public float skinWidth = 0.02f;


  }
}