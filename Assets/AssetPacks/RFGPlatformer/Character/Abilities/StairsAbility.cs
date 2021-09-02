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

      [Header("Settings")]
      public StairsSettings StairsSettings;

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
        _stairsInputUp = (verticalInput > StairsSettings.Threshold.y);
        _stairsInputDown = (verticalInput < -StairsSettings.Threshold.y);
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
        if (_controller.State.IsGrounded && !_controller.State.IsJumping && !_controller.State.IsWallJumping && !_controller.State.IsDashing)
        {
          // If there are stairs ahead and you're not on stairs
          if (StairsAhead && !OnStairs)
          {
            // if you have no input to go up the stairs
            if (!_stairsInputUp)
            {
              authorize = false;
            }
            // or if the stairs ahead angle is too much, then dont turn on collisions with stairs
            if (_stairsAheadAngle < 0 || _stairsAheadAngle >= 90f)
            {
              authorize = false;
            }
          }

          // If there are stairs below and you're not on stairs and the One Way Platform mask is the one you are standing on
          if (StairsBelow && !OnStairs && _controller.OneWayPlatformMask.Contains(_controller.StandingOn.layer))
          {
            // If you have input going down
            if (_stairsInputDown)
            {
              // and the angle is good to go
              if (_stairsBelowAngle > 0 && _stairsBelowAngle <= 90f)
              {
                // Then jump through the one way platform
                _controller.State.IsJumping = true;

                // Record what collider we were standing on
                _goingDownEntryBoundsCollider = _controller.StandingOnCollider;

                // Remove one way platforms in the platform mask
                _controller.PlatformMask -= _controller.OneWayPlatformMask;
                _controller.PlatformMask -= _controller.OneWayMovingPlatformMask;

                // Add stairs to the platform mask
                _controller.PlatformMask |= _controller.StairsMask;

                // Record the time when you went through the one way platform
                _goingDownEntryAt = Time.time;
              }
            }
          }
        }

        if (authorize)
        {
          _controller.CollisionsOnWithStairs();
        }
        else
        {
          _controller.CollisionsOffWithStairs();
        }
      }

      /// <summary>
      /// Restores collisions once we're out of the stairs and if enough time has passed
      /// </summary>
      private void HandleEntryBounds()
      {
        // If we weren't standing on any collider then return
        if (_goingDownEntryBoundsCollider == null)
        {
          return;
        }

        // If the time hasn't passed yet to exceed the StairsBelow lock time then return
        if (Time.time - _goingDownEntryAt < StairsSettings.StairsBelowLockTime)
        {
          return;
        }

        // Getting here means we have a collider we were standing on, we have passed the lock time
        // and the collider doesn't contain the controllers collider bottom position
        // then turn back on collisions
        if (!_goingDownEntryBoundsCollider.bounds.Contains(_controller.ColliderBottomPosition))
        {
          _controller.CollisionsOn();
          _goingDownEntryBoundsCollider = null;
        }
      }

      /// <summary>
      /// Casts a ray to see if there are stairs in front of the character
      /// </summary>
      private void CheckIfStairsAhead()
      {
        StairsAhead = false;

        if (_controller.State.IsFacingRight)
        {
          _raycastOrigin = transform.position + StairsSettings.StairsAheadDetectionRaycastOrigin.x * Vector3.right + StairsSettings.StairsAheadDetectionRaycastOrigin.y * transform.up;
          _raycastDirection = Vector3.right;
        }
        else
        {
          _raycastOrigin = transform.position - StairsSettings.StairsAheadDetectionRaycastOrigin.x * Vector3.right + StairsSettings.StairsAheadDetectionRaycastOrigin.y * transform.up;
          _raycastDirection = -Vector3.right;
        }

        // we cast our ray in front of us
        RaycastHit2D hit = RFG.Physics2D.RayCast(_raycastOrigin, _raycastDirection, StairsSettings.StairsAheadDetectionRaycastLength, _controller.StairsMask, Color.yellow, true);

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
          _raycastOrigin = _controller.ColliderBottomPosition + StairsSettings.StairsBelowDetectionRaycastOrigin.x * Vector3.right + StairsSettings.StairsBelowDetectionRaycastOrigin.y * transform.up;
        }
        else
        {
          _raycastOrigin = _controller.ColliderBottomPosition - StairsSettings.StairsBelowDetectionRaycastOrigin.x * Vector3.right + StairsSettings.StairsBelowDetectionRaycastOrigin.y * transform.up;
        }

        RaycastHit2D hit = RFG.Physics2D.RayCast(_raycastOrigin, -transform.up, StairsSettings.StairsBelowDetectionRaycastLength, _controller.StairsMask, Color.yellow, true);

        if (hit)
        {
          if (_controller.State.IsFacingRight)
          {
            _stairsBelowAngle = Mathf.Abs(Vector2.Angle(hit.normal, Vector3.right));
          }
          else
          {
            _stairsBelowAngle = Mathf.Abs(Vector2.Angle(hit.normal, -Vector3.right));
          }

          StairsBelow = true;
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
          // Are we actually standing on a layer with the Stairs layer mask?
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

