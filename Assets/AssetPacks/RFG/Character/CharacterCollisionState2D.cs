public class CharacterCollisionState2D
{
  public bool IsCollidingRight { get; set; }
  public bool IsCollidingLeft { get; set; }
  public bool IsCollidingAbove { get; set; }
  public bool IsCollidingBelow { get; set; }
  public bool HasCollision
  {
    get
    {
      return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow;
    }
  }
  public float DistanceToLeftCollider;
  public float DistanceToRightCollider;
  public float LateralSlopeAngle { get; set; }
  public float BelowSlopeAngle { get; set; }
  public bool SlopeAngleOK { get; set; }
  public bool OnAMovingPlatform { get; set; }
  public bool IsGrounded
  {
    get
    {
      return IsCollidingBelow;
    }
  }
  public bool IsFalling { get; set; }
  public bool IsJumping { get; set; }
  public bool WasGroundedLastFrame { get; set; }
  public bool WasTouchingTheCeilingLastFrame { get; set; }
  public bool JustGotGrounded { get; set; }
  public bool ColliderResized { get; set; }
  public bool TouchingLevelBounds { get; set; }
  public void Reset()
  {
    IsCollidingLeft = false;
    IsCollidingRight = false;
    IsCollidingAbove = false;
    IsCollidingBelow = false;
    DistanceToLeftCollider = -1;
    DistanceToRightCollider = -1;
    SlopeAngleOK = false;
    JustGotGrounded = false;
    IsFalling = false;
    LateralSlopeAngle = 0;
  }
}
