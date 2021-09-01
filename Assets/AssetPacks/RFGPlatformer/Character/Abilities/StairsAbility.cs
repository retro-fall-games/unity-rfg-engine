using UnityEngine;
using UnityEngine.InputSystem;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Stairs")]
    public class StairsAbility : MonoBehaviour, IAbility
    {

      [Header("Input")]

      /// <summary>Input Action to read the xy axis</summary>
      [Tooltip("Input Action to read the xy axis")]
      public InputActionReference XYAxis;

      /// the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)
      [Tooltip("the minimum horizontal and vertical value you need to reach to trigger movement on an analog controller (joystick for example)")]
      public Vector2 Threshold = new Vector2(0.1f, 0.4f);

      [Header("Status")]

      /// true if the character is on stairs this frame, false otherwise
      [ReadOnly]
      [Tooltip("true if the character is on stairs this frame, false otherwise")]
      public bool OnStairs = false;

      /// true if there are stairs below our character
      [ReadOnly]
      [Tooltip("true if there are stairs below our character")]
      public bool StairsBelow = false;

      /// true if there are stairs ahead of our character
      [ReadOnly]
      [Tooltip("true if there are stairs ahead of our character")]
      public bool StairsAhead = false;

      [Header("Ahead stairs detection")]

      /// the offset to apply when raycasting for stairs
      [Tooltip("the offset to apply when raycasting for stairs")]
      public Vector3 StairsAheadDetectionRaycastOrigin = new Vector3(-0.75f, 0f, 0f);

      /// the length of the raycast looking for stairs
      [Tooltip("the length of the raycast looking for stairs")]
      public float StairsAheadDetectionRaycastLength = 2f;

      /// the offset to apply when raycasting for stairs
      [Tooltip("the offset to apply when raycasting for stairs")]
      public Vector3 StairsBelowDetectionRaycastOrigin = new Vector3(-0.2f, 0f, 0f);

      /// the length of the raycast looking for stairs
      [Tooltip("the length of the raycast looking for stairs")]
      public float StairsBelowDetectionRaycastLength = 0.25f;

      /// the duration, in seconds, during which collisions with one way platforms should be ignored when starting to get down a stair
      [Tooltip("the duration, in seconds, during which collisions with one way platforms should be ignored when starting to get down a stair")]
      public float StairsBelowLockTime = 0.2f;

      private bool _stairsInputUp = false;
      private bool _stairsInputDown = false;
      private float _stairsAheadAngle;
      private float _stairsBelowAngle;
      private Vector3 _raycastOrigin;
      private Vector3 _raycastDirection;
      private Collider2D _goingDownEntryBoundsCollider;
      private float _goingDownEntryAt;

      private CharacterController2D _controller;

      private void Awake()
      {
        _controller = GetComponent<CharacterController2D>();
      }

      private void Update()
      {
        float verticalInput = XYAxis.action.ReadValue<Vector2>().y;
        _stairsInputUp = (verticalInput > Threshold.y);
        _stairsInputDown = (verticalInput < -Threshold.y);
        HandleEntryBounds();
        CheckIfStairsAhead();
        CheckIfStairsBelow();
        CheckIfOnStairways();
        HandleStairsAuthorization();
      }

      /// <summary>
      /// Sets the character in looking up state and asks the camera to look up
      /// </summary>
      private void HandleStairsAuthorization()
      {

        bool authorize = true;
        if (_controller.State.IsGrounded // or if we're  grounded
            && (!_controller.State.IsJumping) // or if we're jumping
            && (!_controller.State.IsWallJumping) // or if we're wall jumping
            && (!_controller.State.IsDashing))
        {
          if (StairsAhead && !OnStairs)
          {
            if (!_stairsInputUp)
            {
              authorize = false;
            }
            if ((_stairsAheadAngle < 0) || (_stairsAheadAngle >= 90f))
            {
              authorize = false;
            }
          }

          if (StairsBelow && !OnStairs && _controller.OneWayPlatformMask.Contains(_controller.StandingOn.layer))
          {
            if (_stairsInputDown)
            {
              if ((_stairsBelowAngle > 0) && (_stairsBelowAngle <= 90f))
              {
                _controller.State.IsJumping = true;
                _goingDownEntryBoundsCollider = _controller.StandingOnCollider;
                _controller.PlatformMask -= _controller.OneWayPlatformMask;
                _controller.PlatformMask -= _controller.OneWayMovingPlatformMask;
                _controller.PlatformMask |= _controller.StairsMask;
                _goingDownEntryAt = Time.time;
              }
            }
          }
        }

        if (authorize)
        {
          AuthorizeStairs();
        }
        else
        {
          DenyStairs();
        }
      }

      /// <summary>
      /// Restores collisions once we're out of the stairs and if enough time has passed
      /// </summary>
      private void HandleEntryBounds()
      {
        if (_goingDownEntryBoundsCollider == null)
        {
          return;
        }
        if (Time.time - _goingDownEntryAt < StairsBelowLockTime)
        {
          return;
        }
        if (!_goingDownEntryBoundsCollider.bounds.Contains(_controller.ColliderBottomPosition))
        {
          _controller.CollisionsOn();
          _goingDownEntryBoundsCollider = null;
        }
      }

      /// <summary>
      /// Authorizes collisions with stairs
      /// </summary>
      private void AuthorizeStairs()
      {
        _controller.CollisionsOnWithStairs();
      }

      /// <summary>
      /// Prevents collisions with stairs
      /// </summary>
      private void DenyStairs()
      {
        _controller.CollisionsOffWithStairs();
      }

      /// <summary>
      /// Casts a ray to see if there are stairs in front of the character
      /// </summary>
      private void CheckIfStairsAhead()
      {
        StairsAhead = false;

        if (_controller.State.IsFacingRight)
        {
          _raycastOrigin = transform.position + StairsAheadDetectionRaycastOrigin.x * transform.right + StairsAheadDetectionRaycastOrigin.y * transform.up;
          _raycastDirection = transform.right;
        }
        else
        {
          _raycastOrigin = transform.position + StairsAheadDetectionRaycastOrigin.x * transform.right + StairsAheadDetectionRaycastOrigin.y * transform.up;
          _raycastDirection = transform.right;
        }

        // we cast our ray in front of us
        RaycastHit2D hit = RFG.Physics2D.RayCast(_raycastOrigin, _raycastDirection, StairsAheadDetectionRaycastLength, _controller.StairsMask, Color.yellow, true);

        if (hit)
        {
          _stairsAheadAngle = Mathf.Abs(Vector2.Angle(hit.normal, transform.up));
          StairsAhead = true;
        }
      }

      /// <summary>
      /// Casts a ray to see if there are stairs below the character
      /// </summary>
      private void CheckIfStairsBelow()
      {
        StairsBelow = false;

        _raycastOrigin = _controller.BoundsCenter;
        if (_controller.State.IsFacingRight)
        {
          _raycastOrigin = _controller.ColliderBottomPosition + StairsBelowDetectionRaycastOrigin.x * transform.right + StairsBelowDetectionRaycastOrigin.y * transform.up;
        }
        else
        {
          _raycastOrigin = _controller.ColliderBottomPosition + StairsBelowDetectionRaycastOrigin.x * transform.right + StairsBelowDetectionRaycastOrigin.y * transform.up;
        }

        RaycastHit2D hit = RFG.Physics2D.RayCast(_raycastOrigin, -transform.up, StairsBelowDetectionRaycastLength, _controller.StairsMask, Color.yellow, true);

        if (hit)
        {
          if (_controller.State.IsFacingRight)
          {
            _stairsBelowAngle = Mathf.Abs(Vector2.Angle(hit.normal, transform.right));
          }
          else
          {
            _stairsBelowAngle = Mathf.Abs(Vector2.Angle(hit.normal, transform.right));
          }

          StairsBelow = true;
        }
        else
        {
          Debug.Log("Not Hit");
        }
      }

      /// <summary>
      /// Checks if the character is currently standing on stairs
      /// </summary>
      private void CheckIfOnStairways()
      {
        OnStairs = false;
        if (_controller.StandingOn != null)
        {
          if (_controller.StairsMask.Contains(_controller.StandingOn.layer))
          {
            OnStairs = true;
          }
        }
      }

      private void OnEnable()
      {
        XYAxis.action.Enable();
      }

      private void OnDisable()
      {
        XYAxis.action.Disable();
      }


    }
  }
}

