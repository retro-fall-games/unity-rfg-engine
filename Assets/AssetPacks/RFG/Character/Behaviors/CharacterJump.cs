using UnityEngine;

namespace RFG.Character
{
  public class CharacterJump : CharacterBehavior
  {

    public enum JumpBehavior
    {
      CanJumpOnGround,
      CanJumpOnGroundAndFromLadders,
      CanJumpAnywhere,
      CantJump,
      CanJumpAnywhereAnyNumberOfTimes
    }

    [Header("Jump Behavior")]
    public int numberOfJumps = 2;
    public float jumpHeight = 3f;
    public JumpBehavior jumpBehavior = JumpBehavior.CanJumpAnywhere;
    public bool canJumpDownOneWayPlatforms = true;

    [Header("Proportional Jumps")]
    public bool jumpIsProportionalToThePressTime = true;
    public float jumpMinAirTime = 0.1f;
    public float jumpReleaseForceFactor = 2f;

    [Header("Timeframe")]
    public float jumpTriggerTime = 0f;
    public float inputBufferDuration = 0f;

    [Header("Collisions")]
    public float oneWayPlatformsJumpCollisionOffDuration = 0.3f;
    public float movingPlatformsJumpCollisionOffDuration = 0.05f;

    [HideInInspector]
    public bool JumpHappenedThisFrame { get; set; }
    public bool CanStopJump { get; set; }

    private int _numberOfJumpsLeft;
    private float _jumpButtonPressTime = 0f;
    private float _lastJumpAt = 0f;
    private bool _jumpButtonPressed = false;
    private bool _jumpButtonReleased = false;
    private bool _doubleJumping = false;
    private int _initialNumberOfJumps;
    private float _lastTimeGrounded = 0f;
    private float _lastInputBufferJumpAt = 0f;
    private bool _jumpTriggerTime = false;



    public override void Process()
    {
      HandleInput();
      HandleMovement();
    }

    private void HandleInput()
    {
      if (_character.InputManager.JumpButton.State.CurrentState == Input.ButtonStates.Down)
      {
        JumpStart();
      }
    }

    private void HandleMovement()
    {

    }

    private void JumpStart()
    {

    }
  }
}