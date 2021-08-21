namespace RFG
{
  namespace Platformer
  {
    public class CharacterControllerState2D
    {
      public bool IsCollidingRight { get; set; }
      public bool IsCollidingLeft { get; set; }
      public bool IsCollidingAbove { get; set; }
      public bool IsCollidingBelow { get; set; }
      public bool IsMovingDownSlope { get; set; }
      public bool IsMovingUpSlope { get; set; }
      public bool IsGrounded { get { return IsCollidingBelow; } }
      public bool JustGotGrounded { get; set; }
      public bool WasGroundedLastFrame { get; set; }
      public float SlopeAngle { get; set; }
      public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }
      public bool IsFacingRight { get; set; }
      public bool IsFalling { get; set; }
      public bool IsJumping { get; set; }
      public bool IsStandingOnStairs { get; set; }

      // public float DistanceToLeftCollider;
      // public float DistanceToRightCollider;
      // public float LateralSlopeAngle { get; set; }
      // public float BelowSlopeAngle { get; set; }
      // public bool SlopeAngleOK { get; set; }
      // public bool OnAMovingPlatform { get; set; }

      // public bool WasGroundedLastFrame { get; set; }
      // public bool WasTouchingTheCeilingLastFrame { get; set; }
      // public bool ColliderResized { get; set; }
      // public bool TouchingLevelBounds { get; set; }

      public void Reset()
      {
        IsCollidingLeft = false;
        IsCollidingRight = false;
        IsCollidingAbove = false;
        IsCollidingBelow = false;
        IsMovingDownSlope = false;
        IsMovingUpSlope = false;
        JustGotGrounded = false;
        IsFalling = true;
        SlopeAngle = 0f;
      }

      public override string ToString()
      {
        return $"Controller R:{IsCollidingRight} L:{IsCollidingLeft} A:{IsCollidingAbove} B:{IsCollidingBelow} D-SLOPE:{IsMovingDownSlope} U-SLOPE:{IsMovingUpSlope} SLOPE-ANGLE: {SlopeAngle}";
      }
    }
  }
}