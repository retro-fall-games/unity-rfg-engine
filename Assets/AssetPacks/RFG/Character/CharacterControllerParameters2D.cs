using System;
using UnityEngine;

namespace RFG.Character
{
  [Serializable]
  public class CharacterControllerParameters2D
  {
    [Header("Gravity")]
    public float gravity = -30f;
    public float fallMultiplier = 1f;
    public float ascentMultiplier = 1f;

    [Header("Speed")]
    public Vector2 maxVelocity = new Vector2(100f, 100f);

    [Header("Slopes")]
    [Range(0, 90)]
    public float maxSlopeAngle = 30f;
    public AnimationCurve slopeAngleSpeedFactor = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 1f));
  }
}