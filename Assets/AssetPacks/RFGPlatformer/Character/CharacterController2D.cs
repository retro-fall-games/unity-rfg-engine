using UnityEngine;
using RFG.Utils;

namespace RFG.Platformer
{
  public class CharacterController2D : MonoBehaviour
  {

    [Header("Parameters")]
    public CharacterControllerParameters2D defaultParameters;
    public CharacterControllerState2D State { get; private set; }
    public CharacterControllerParameters2D Parameters => _overrideParameters ?? defaultParameters;

    [Header("Layer Masks")]
    public LayerMask platformMask;
    public LayerMask oneWayPlatformMask;
    public LayerMask movingPlatformMask;
    public LayerMask oneWayMovingPlatformMask;
    public LayerMask stairsMask;

    [Header("Raycasts")]
    public int numberOfHorizontalRays = 8;
    public int numberOfVerticalRays = 4;

    [HideInInspector]
    public Vector2 Velocity { get { return _velocity; } }
    public bool HandleCollisions { get; set; }
    public GameObject StandingOn { get; private set; }
    public Vector3 PlatformVelocity { get; private set; }
    public bool IgnoreOneWayPlatformsThisFrame { get; set; }
    public static readonly float slopeLimitTanget = Mathf.Tan(75f * Mathf.Deg2Rad);
    public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90f, 1.5f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));
    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider;
    private float _verticalDistanceBetweenRays;
    private float _horizontalDistanceBetweenRays;
    private CharacterControllerParameters2D _overrideParameters;
    private Vector3 _raycastTopLeft;
    private Vector3 _raycastBottomRight;
    private Vector3 _raycastBottomLeft;
    private Vector3 _activeLocalPlatformPoint;
    private Vector3 _activeGlobalPlatformPoint;
    private GameObject _lastStandingOn;


    private void Awake()
    {
      State = new CharacterControllerState2D();
      _transform = transform;
      _localScale = transform.localScale;
      _boxCollider = GetComponent<BoxCollider2D>();

      CalculateDistanceBetweenRays();

      HandleCollisions = true;

      oneWayPlatformMask |= oneWayMovingPlatformMask;
      movingPlatformMask |= oneWayMovingPlatformMask;

      platformMask |= oneWayPlatformMask;
      platformMask |= movingPlatformMask;
      platformMask |= oneWayMovingPlatformMask;

    }

    private void Start()
    {
      State.IsFacingRight = transform.localScale.x > 0;
    }

    public void AddForce(Vector2 force)
    {
      _velocity += force;
    }

    public void AddHorizontalForce(float x)
    {
      _velocity.x += x;
    }

    public void AddVerticalForce(float y)
    {
      _velocity.y += y;
    }

    public void SetForce(Vector2 force)
    {
      _velocity = force;
    }

    public void SetHorizontalForce(float x)
    {
      _velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
      _velocity.y = y;
    }

    public void ResetVelocity()
    {
      _velocity = new Vector2(0f, 0f);
    }

    public void SetOverrideParameters(CharacterControllerParameters2D parameters)
    {
      _overrideParameters = parameters;
    }

    public void Flip()
    {
      transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
      State.IsFacingRight = transform.localScale.x > 0;
    }

    private void Update()
    {
      _velocity.y += Parameters.gravity * Time.deltaTime;
      Move(_velocity * Time.deltaTime);

      if (!State.WasGroundedLastFrame && State.IsCollidingBelow)
      {
        State.JustGotGrounded = true;
      }
    }

    private void Move(Vector2 deltaMovement)
    {
      State.WasGroundedLastFrame = State.IsCollidingBelow;
      State.Reset();

      if (HandleCollisions)
      {
        HandleMovingPlaforms();
        CalculateRayOrigins();

        // If they are affected by gravity and is grounded means they are on a slope
        if (deltaMovement.y < 0 && State.WasGroundedLastFrame)
        {
          HandleVerticalSlope(ref deltaMovement);
        }

        // Only if we are moving horizontal
        if (deltaMovement.x != 0f)
        {
          MoveHorizontally(ref deltaMovement);
        }

        // Only if we are moving vertical
        if (deltaMovement.y != 0f)
        {
          MoveVertically(ref deltaMovement);
        }

        CorrectHorizontalPlacement(ref deltaMovement, true);
        CorrectHorizontalPlacement(ref deltaMovement, false);

      }

      _transform.Translate(deltaMovement, Space.World);

      // Set velocity to the delta we have calculated
      if (Time.deltaTime > 0)
      {
        _velocity = deltaMovement / Time.deltaTime;
      }

      // Clamp velocity to max velocity
      _velocity.x = Mathf.Min(_velocity.x, Parameters.maxVelocity.x);
      _velocity.y = Mathf.Min(_velocity.y, Parameters.maxVelocity.y);

      if (State.IsMovingUpSlope)
      {
        _velocity.y = 0;
      }

      if (StandingOn != null)
      {
        _activeGlobalPlatformPoint = _transform.position;
        _activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(_transform.position);

        if (_lastStandingOn != StandingOn)
        {
          if (_lastStandingOn != null)
          {
            _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
          }
          StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
          _lastStandingOn = StandingOn;
        }
        else if (StandingOn != null)
        {
          StandingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
        }
      }
      else if (_lastStandingOn != null)
      {
        _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
        _lastStandingOn = null;
      }

      IgnoreOneWayPlatformsThisFrame = false;

    }

    private void HandleMovingPlaforms()
    {
      if (StandingOn != null && (movingPlatformMask.Contains(StandingOn.layer) || oneWayMovingPlatformMask.Contains(StandingOn.layer)))
      {
        var newGlobalPlatformPoint = StandingOn.transform.TransformPoint(_activeLocalPlatformPoint);
        var moveDistance = newGlobalPlatformPoint - _activeGlobalPlatformPoint;

        if (moveDistance != Vector3.zero)
        {
          transform.Translate(moveDistance, Space.World);
        }

        PlatformVelocity = (newGlobalPlatformPoint - _activeGlobalPlatformPoint) / Time.deltaTime;
      }
      else
      {
        PlatformVelocity = Vector3.zero;
      }

      StandingOn = null;
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
    {
      var halfWidth = (_boxCollider.size.x * _localScale.x) / 2;
      var rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;

      if (isRight)
      {
        rayOrigin.x -= halfWidth - Parameters.skinWidth;
      }
      else
      {
        rayOrigin.x += halfWidth - Parameters.skinWidth;
      }

      var rayDirection = isRight ? Vector2.right : Vector2.left;
      var offset = 0f;

      for (var i = 1; i < numberOfHorizontalRays - 1; i++)
      {
        var rayVector = new Vector2(deltaMovement.x + rayOrigin.x, deltaMovement.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));
        var raycastHit = RFG.Utils.Physics2D.Raycast(rayVector, rayDirection, halfWidth, movingPlatformMask);

        if (!raycastHit)
        {
          continue;
        }

        offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));
      }

      deltaMovement.x += offset;

    }

    private void CalculateDistanceBetweenRays()
    {
      // figure out the distance between our rays in both directions
      // horizontal
      var colliderUseableHeight = _boxCollider.size.y * Mathf.Abs(_localScale.y) - (2f * Parameters.skinWidth);
      _verticalDistanceBetweenRays = colliderUseableHeight / (numberOfHorizontalRays - 1);

      // vertical
      var colliderUseableWidth = _boxCollider.size.x * Mathf.Abs(_localScale.x) - (2f * Parameters.skinWidth);
      _horizontalDistanceBetweenRays = colliderUseableWidth / (numberOfVerticalRays - 1);
    }

    private void CalculateRayOrigins()
    {
      // Precompute where the rays will be shot out from, current postion of the player 
      // our raycasts need to be fired from the bounds inset by the skinWidth
      var modifiedBounds = _boxCollider.bounds;
      modifiedBounds.Expand(-2f * Parameters.skinWidth);

      _raycastTopLeft = new Vector2(modifiedBounds.min.x, modifiedBounds.max.y);
      _raycastBottomRight = new Vector2(modifiedBounds.max.x, modifiedBounds.min.y);
      _raycastBottomLeft = modifiedBounds.min;

    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
      var isGoingRight = deltaMovement.x > 0;
      var rayDistance = Mathf.Abs(deltaMovement.x) + Parameters.skinWidth;
      var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
      var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

      for (var i = 0; i < numberOfHorizontalRays; i++)
      {
        var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
        RaycastHit2D raycastHit;

        // if we are grounded we will include oneWayPlatforms only on the first ray (the bottom one). this will allow us to
        // walk up sloped oneWayPlatforms
        if (i == 0 && State.WasGroundedLastFrame)
        {
          raycastHit = RFG.Utils.Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask);
        }
        else
        {
          raycastHit = RFG.Utils.Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask & ~oneWayPlatformMask);
        }

        if (!raycastHit)
        {
          continue;
        }

        if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), isGoingRight))
        {
          // if we weren't grounded last frame, that means we're landing on a slope horizontally.
          // this ensures that we stay flush to that slope
          if (!State.WasGroundedLastFrame)
          {
            float flushDistance = Mathf.Sign(deltaMovement.x) * (raycastHit.distance - Parameters.skinWidth);
            _transform.Translate(new Vector2(flushDistance, 0));
          }
          break;
        }

        // Sets the deltaMovement to the furthest we can move without going further into the obstacle
        deltaMovement.x = raycastHit.point.x - rayVector.x;
        rayDistance = Mathf.Abs(deltaMovement.x);

        if (isGoingRight)
        {
          deltaMovement.x -= Parameters.skinWidth;
          State.IsCollidingRight = true;
        }
        else
        {
          deltaMovement.x += Parameters.skinWidth;
          State.IsCollidingLeft = true;
        }

        if (rayDistance < Parameters.skinWidth + 0.0001f)
        {
          break;
        }
      }

    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
      var isGoingUp = deltaMovement.y > 0;
      var rayDistance = Mathf.Abs(deltaMovement.y) + Parameters.skinWidth;
      var rayDirection = isGoingUp ? Vector2.up : Vector2.down;
      var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;

      // apply the delta movement to the rayOrigin since we have already calculated x movement
      rayOrigin.x += deltaMovement.x;

      var mask = platformMask;

      // if we are moving up we should ignore oneWayPlatforms
      if ((isGoingUp && !State.WasGroundedLastFrame) || IgnoreOneWayPlatformsThisFrame)
      {
        mask &= ~oneWayPlatformMask;
        mask &= ~oneWayMovingPlatformMask;
      }

      var standingOnDistance = float.MaxValue;

      for (var i = 0; i < numberOfVerticalRays; i++)
      {
        var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays), rayOrigin.y);
        var raycastHit = RFG.Utils.Physics2D.Raycast(rayVector, rayDirection, rayDistance, mask);

        if (!raycastHit)
        {
          continue;
        }

        if (!isGoingUp)
        {
          var verticalDistanceToHit = _transform.position.y - raycastHit.point.y;
          if (verticalDistanceToHit < standingOnDistance)
          {
            standingOnDistance = verticalDistanceToHit;
            StandingOn = raycastHit.collider.gameObject;
          }
        }

        // Determine whats the furtherest we can move down without hitting anything
        deltaMovement.y = raycastHit.point.y - rayVector.y;

        rayDistance = Mathf.Abs(deltaMovement.y);

        if (isGoingUp)
        {
          deltaMovement.y -= Parameters.skinWidth;
          State.IsCollidingAbove = true;
        }
        else
        {
          deltaMovement.y += Parameters.skinWidth;
          State.IsCollidingBelow = true;
        }

        if (!isGoingUp && deltaMovement.y > 0.0001f)
        {
          State.IsMovingUpSlope = true;
        }

        if (rayDistance < Parameters.skinWidth + 0.0001f)
        {
          break;
        }
      }
    }

    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
      if (Mathf.RoundToInt(angle) == 90)
      {
        return false;
      }

      if (angle < Parameters.maxSlopeAngle)
      {
        if (deltaMovement.y < 0.07f)
        {
          var slopeModifier = slopeSpeedMultiplier.Evaluate(angle);
          deltaMovement.x *= slopeModifier;

          deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);

          var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

          RaycastHit2D raycastHit = RFG.Utils.Physics2D.Raycast(rayOrigin, deltaMovement.normalized, deltaMovement.magnitude, platformMask);

          if (raycastHit)
          {
            deltaMovement = (Vector3)raycastHit.point - rayOrigin;
            deltaMovement.x += isGoingRight ? -Parameters.skinWidth : Parameters.skinWidth;
          }

          State.IsMovingUpSlope = true;
          State.IsCollidingBelow = true;
        }

      }
      else
      {
        deltaMovement.x = 0;
      }

      return true;
    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {
      var center = (_raycastBottomLeft.x + _raycastBottomRight.x) * 0.5f;
      var direction = Vector2.down;

      var slopeDistance = slopeLimitTanget * (_raycastBottomRight.x - center);
      var slopeRayVector = new Vector2(center, _raycastBottomLeft.y);

      var raycastHit = RFG.Utils.Physics2D.Raycast(slopeRayVector, direction, slopeDistance, platformMask);

      if (!raycastHit)
      {
        return;
      }

      var angle = Vector2.Angle(raycastHit.normal, Vector2.up);

      if (Mathf.Abs(angle) == 0f)
      {
        return;
      }

      var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);

      if (isMovingDownSlope)
      {
        var slopeModifier = slopeSpeedMultiplier.Evaluate(-angle);
        deltaMovement.y += raycastHit.point.y - slopeRayVector.y - Parameters.skinWidth;
        // deltaMovement.x *= slopeModifier;
        State.IsMovingDownSlope = true;
        State.SlopeAngle = angle;
      }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    private void OnTriggerExit2D(Collider2D other)
    {
    }

  }
}