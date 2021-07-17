using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Character Controller 2D")]
  public class CharacterController2D : MonoBehaviour
  {

    [Header("Parameters")]
    public CharacterControllerParameters2D defaultParameters;
    public CharacterControllerState2D State { get; private set; }
    public CharacterControllerParameters2D Parameters => _overrideParameters ?? defaultParameters;
    public bool rotateOnMouseCursor = false;

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
    public event Action<RaycastHit2D> onControllerCollidedEvent;
    public event Action<Collider2D> onTriggerEnterEvent;
    public event Action<Collider2D> onTriggerStayEvent;
    public event Action<Collider2D> onTriggerExitEvent;
    public Vector2 Velocity { get { return _velocity; } }
    public bool HandleCollisions { get; set; }
    public GameObject StandingOn { get; private set; }
    public Vector3 PlatformVelocity { get; private set; }
    public bool IgnoreOneWayPlatformsThisFrame { get; set; }
    public static readonly float slopeLimitTanget = Mathf.Tan(75f * Mathf.Deg2Rad);
    public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90f, 1.5f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));
    private Vector2 _velocity;
    private Transform _transform;
    private BoxCollider2D _boxCollider;
    private List<RaycastHit2D> _raycastHitsThisFrame = new List<RaycastHit2D>(2);
    private float _verticalDistanceBetweenRays;
    private float _horizontalDistanceBetweenRays;
    private CharacterControllerParameters2D _overrideParameters;
    private Vector3 _raycastTopLeft;
    private Vector3 _raycastBottomRight;
    private Vector3 _raycastBottomLeft;
    private Vector3 _activeLocalPlatformPoint;
    private Vector3 _activeGlobalPlatformPoint;
    private GameObject _lastStandingOn;
    private bool _ignoreStairs;
    private float _fallSlowFactor;
    private float _boundsWidth;
    private float _boundsHeight;
    private bool _gravityActive = true;
    private Vector3 _colliderBottomCenterPosition;
    private Vector3 _colliderLeftCenterPosition;
    private Vector3 _colliderRightCenterPosition;
    private Vector3 _colliderTopCenterPosition;
    private Camera _mainCamera;


    private void Awake()
    {
      _mainCamera = Camera.main;
      State = new CharacterControllerState2D();
      _transform = transform;
      _boxCollider = GetComponent<BoxCollider2D>();

      CalculateDistanceBetweenRays();

      HandleCollisions = true;

      oneWayPlatformMask |= oneWayMovingPlatformMask;
      movingPlatformMask |= oneWayMovingPlatformMask;

      platformMask |= oneWayPlatformMask;
      platformMask |= movingPlatformMask;
      platformMask |= oneWayMovingPlatformMask;

      CollisionsOnStairs(false);

    }

    private void Start()
    {
      State.IsFacingRight = transform.localScale.x > 0;
    }


    private void Update()
    {
      ApplyGravity();
      Move(_velocity * Time.deltaTime);

      if (!State.WasGroundedLastFrame && State.IsCollidingBelow)
      {
        State.JustGotGrounded = true;
      }
    }

    private void LateUpdate()
    {
      if (rotateOnMouseCursor)
      {
        RotateOnMouseCursor();
      }
    }

    private void ApplyGravity()
    {
      if (_gravityActive)
      {
        _velocity.y += Parameters.gravity * Time.deltaTime;
      }

      if (_fallSlowFactor != 0)
      {
        _velocity.y *= _fallSlowFactor;
      }
    }

    private void Move(Vector2 deltaMovement)
    {
      State.WasGroundedLastFrame = State.IsCollidingBelow;
      State.Reset();
      _raycastHitsThisFrame.Clear();

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

        if (!stairsMask.Contains(StandingOn.layer))
        {
          CollisionsOnStairs(false);
        }

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

      // send off the collision events if we have a listener
      if (onControllerCollidedEvent != null)
      {
        for (var i = 0; i < _raycastHitsThisFrame.Count; i++)
          onControllerCollidedEvent(_raycastHitsThisFrame[i]);
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
      var halfWidth = (_boxCollider.size.x * Mathf.Abs(_transform.localScale.x)) / 2;
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
        var raycastHit = RFG.Physics2D.Raycast(rayVector, rayDirection, halfWidth, movingPlatformMask, Color.yellow);

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
      var colliderUseableHeight = _boxCollider.size.y * Mathf.Abs(_transform.localScale.y) - (2f * Parameters.skinWidth);
      _verticalDistanceBetweenRays = colliderUseableHeight / (numberOfHorizontalRays - 1);

      // vertical
      var colliderUseableWidth = _boxCollider.size.x * Mathf.Abs(_transform.localScale.x) - (2f * Parameters.skinWidth);
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

      _boundsWidth = Vector2.Distance(_raycastBottomLeft, _raycastBottomRight);
      _boundsHeight = Vector2.Distance(_raycastBottomLeft, _raycastTopLeft);

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
          raycastHit = RFG.Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask, Color.red);
        }
        else
        {
          raycastHit = RFG.Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask & ~oneWayPlatformMask, Color.green);
        }

        if (!raycastHit)
        {
          continue;
        }

        if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), isGoingRight))
        {
          _raycastHitsThisFrame.Add(raycastHit);
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

        _raycastHitsThisFrame.Add(raycastHit);

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
        mask &= ~stairsMask;
      }

      var standingOnDistance = float.MaxValue;

      for (var i = 0; i < numberOfVerticalRays; i++)
      {
        var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays), rayOrigin.y);
        var raycastHit = RFG.Physics2D.Raycast(rayVector, rayDirection, rayDistance, mask, Color.blue);

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
          State.IsFalling = false;
        }

        _raycastHitsThisFrame.Add(raycastHit);

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

          RaycastHit2D raycastHit = RFG.Physics2D.Raycast(rayOrigin, deltaMovement.normalized, deltaMovement.magnitude, stairsMask, Color.magenta);

          if (raycastHit)
          {
            deltaMovement = (Vector3)raycastHit.point - rayOrigin;
            deltaMovement.x += isGoingRight ? -Parameters.skinWidth : Parameters.skinWidth;
          }

          State.IsMovingUpSlope = true;
          State.IsCollidingBelow = true;

          return true;
        }

      }
      else
      {
        deltaMovement.x = 0;
      }

      return false;
    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {
      var center = (_raycastBottomLeft.x + _raycastBottomRight.x) * 0.5f;
      var direction = Vector2.down;

      var slopeDistance = slopeLimitTanget * (_raycastBottomRight.x - center);
      var slopeRayVector = new Vector2(center, _raycastBottomLeft.y);

      var raycastHit = RFG.Physics2D.Raycast(slopeRayVector, direction, slopeDistance, stairsMask, Color.red);

      if (!raycastHit)
      {
        return;
      }

      CollisionsOnStairs(true);

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
        State.IsCollidingBelow = true;
        State.SlopeAngle = angle;
      }

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (onTriggerEnterEvent != null)
        onTriggerEnterEvent(col);
    }

    public void OnTriggerStay2D(Collider2D col)
    {
      if (onTriggerStayEvent != null)
        onTriggerStayEvent(col);
    }

    public void OnTriggerExit2D(Collider2D col)
    {
      if (onTriggerExitEvent != null)
        onTriggerExitEvent(col);
    }

    public float Width()
    {
      return _boundsWidth;
    }

    public float Height()
    {
      return _boundsHeight;
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
      transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
      State.IsFacingRight = _transform.localScale.x > 0;
    }

    public void IgnoreStairsForTime(float time)
    {
      _ignoreStairs = true;
      CollisionsOnStairs(false);
      StartCoroutine(IgnoreStairsForTimeCo(time));
    }

    public IEnumerator IgnoreStairsForTimeCo(float time)
    {
      yield return new WaitForSeconds(time);
      _ignoreStairs = false;
      CollisionsOnStairs(true);
    }

    public void CollisionsOnStairs(bool turnOn)
    {
      if (turnOn && !_ignoreStairs)
      {
        platformMask |= stairsMask;
        oneWayPlatformMask |= stairsMask;
      }
      else
      {
        platformMask &= ~stairsMask;
        oneWayPlatformMask &= ~stairsMask;
      }
    }

    public void SlowFall(float factor)
    {
      _fallSlowFactor = factor;
    }

    public void GravityActive(bool state)
    {
      _gravityActive = state;
    }

    public Vector3 ColliderSize
    {
      get
      {
        return Vector3.Scale(_transform.localScale, _boxCollider.size);
      }
    }

    public Vector3 ColliderCenterPosition
    {
      get
      {
        return _boxCollider.bounds.center;
      }
    }

    public Vector3 ColliderBottomPosition
    {
      get
      {
        _colliderBottomCenterPosition.x = _boxCollider.bounds.center.x;
        _colliderBottomCenterPosition.y = _boxCollider.bounds.min.y;
        _colliderBottomCenterPosition.z = 0;
        return _colliderBottomCenterPosition;
      }
    }

    public Vector3 ColliderLeftPosition
    {
      get
      {
        _colliderLeftCenterPosition.x = _boxCollider.bounds.min.x;
        _colliderLeftCenterPosition.y = _boxCollider.bounds.center.y;
        _colliderLeftCenterPosition.z = 0;
        return _colliderLeftCenterPosition;
      }
    }

    public Vector3 ColliderTopPosition
    {
      get
      {
        _colliderTopCenterPosition.x = _boxCollider.bounds.center.x;
        _colliderTopCenterPosition.y = _boxCollider.bounds.max.y;
        _colliderTopCenterPosition.z = 0;
        return _colliderTopCenterPosition;
      }
    }

    public Vector3 ColliderRightPosition
    {
      get
      {
        _colliderRightCenterPosition.x = _boxCollider.bounds.max.x;
        _colliderRightCenterPosition.y = _boxCollider.bounds.center.y;
        _colliderRightCenterPosition.z = 0;
        return _colliderRightCenterPosition;
      }
    }

    public void RotateTowards(Transform target)
    {
      if (!State.IsFacingRight && target.position.x > _transform.position.x)
      {
        Flip();
      }
      else if (State.IsFacingRight && target.position.x < _transform.position.x)
      {
        Flip();
      }
    }

    private void RotateOnMouseCursor()
    {
      var mousePos = (Vector2)_mainCamera.ScreenToWorldPoint(Input.mousePosition);
      if (State.IsFacingRight && mousePos.x < _transform.position.x)
      {
        Flip();
      }
      else if (!State.IsFacingRight && mousePos.x > _transform.position.x)
      {
        Flip();
      }
    }

  }
}