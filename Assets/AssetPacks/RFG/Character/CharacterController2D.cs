using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG.Utils;

namespace RFG.Character
{
  public class CharacterController2D : MonoBehaviour
  {
    public enum UpdateModes { Update, FixedUpdate }
    public CharacterCollisionState2D CollisionState => _collisionState;

    [Header("Default Parameters")]
    public CharacterControllerParameters2D defaultParameters;
    public CharacterControllerParameters2D Parameters
    {
      get
      {
        return _overrideParameters ?? defaultParameters;
      }
    }

    [Header("Collisions")]
    public LayerMask platformMask;
    public LayerMask movingPlatformMask;
    public LayerMask oneWayPlatformMask;
    public LayerMask movingOneWayPlatformMask;
    public LayerMask midHeightOneWayPlatformMask;
    public LayerMask stairsMask;
    public enum RaycastDirections { up, down, left, right }

    [Header("Raycasting")]
    public UpdateModes UpdateMode = UpdateModes.Update;
    public int numberOfHorizontalRays = 8;
    public int numberOfVerticalRays = 8;
    public float rayOffset = 0.05f;
    public bool castRaysOnBothSides = false;
    public float distanceToTheGroundRayMaximumLength = 100f;
    public bool performSafetyBoxcast = false;

    [Header("Stickiness")]
    public bool stickToSlopes = false;
    public float stickyRaycastLength = 0f;
    public float stickToSlopesOffsetY = 0.2f;

    [Header("Safety")]
    public bool automaticGravitySettings = true;

    [HideInInspector]
    public GameObject standingOn;
    public GameObject StandingOnLastFrame { get; private set; }
    public Collider2D StandingOnCollider { get; private set; }
    public Vector2 WorldSpeed
    {
      get
      {
        return _worldSpeed;
      }
    }

    public Vector2 Speed
    {
      get
      {
        return _speed;
      }
    }

    public Vector2 ForcesApplied { get; private set; }

    public Vector2 ColliderSize
    {
      get
      {
        return Vector2.Scale(transform.localScale, _boxCollider.size);
      }
    }

    public Vector2 ColliderCenterPosition
    {
      get
      {
        return _boxCollider.bounds.center;
      }
    }

    public Vector2 ColliderBottomPosition
    {
      get
      {
        _colliderBottomCenterPosition.x = _boxCollider.bounds.center.x;
        _colliderBottomCenterPosition.y = _boxCollider.bounds.min.y;
        return _colliderBottomCenterPosition;
      }
    }

    public Vector2 ColliderLeftPosition
    {
      get
      {
        _colliderLeftCenterPosition.x = _boxCollider.bounds.min.x;
        _colliderLeftCenterPosition.y = _boxCollider.bounds.center.y;
        return _colliderLeftCenterPosition;
      }
    }
    public Vector2 ColliderRightPosition
    {
      get
      {
        _colliderRightCenterPosition.x = _boxCollider.bounds.max.x;
        _colliderRightCenterPosition.y = _boxCollider.bounds.center.y;
        return _colliderRightCenterPosition;
      }
    }
    public Vector2 ColliderTopPosition
    {
      get
      {
        _colliderTopCenterPosition.x = _boxCollider.bounds.center.x;
        _colliderTopCenterPosition.y = _boxCollider.bounds.max.y;
        return _colliderTopCenterPosition;
      }
    }

    public float Friction
    {
      get
      {
        return _friction;
      }
    }

    public float Width()
    {
      return _boundsWidth;
    }

    public float Height()
    {
      return _boundsHeight;
    }

    public Vector2 Bounds
    {
      get
      {
        _bounds.x = _boundsWidth;
        _bounds.y = _boundsHeight;
        return _bounds;
      }
    }

    public Vector2 BoundsTopLeftCorner
    {
      get
      {
        return _boundsTopLeftCorner;
      }
    }
    public Vector2 BoundsBottomLeftCorner
    {
      get
      {
        return _boundsBottomLeftCorner;
      }
    }
    public Vector2 BoundsTopRightCorner
    {
      get
      {
        return _boundsTopRightCorner;
      }
    }
    public Vector2 BoundsBottomRightCorner
    {
      get
      {
        return _boundsBottomRightCorner;
      }
    }

    public Vector2 BoundsTop
    {
      get
      {
        return (_boundsTopLeftCorner + _boundsTopRightCorner) / 2;
      }
    }
    public Vector2 BoundsBottom
    {
      get
      {
        return (_boundsBottomLeftCorner + _boundsBottomRightCorner) / 2;
      }
    }
    public Vector2 BoundsRight
    {
      get
      {
        return (_boundsTopRightCorner + _boundsBottomRightCorner) / 2;
      }
    }
    public Vector2 BoundsLeft
    {
      get
      {
        return (_boundsTopLeftCorner + _boundsBottomLeftCorner) / 2;
      }
    }
    public Vector2 BoundsCenter
    {
      get
      {
        return _boundsCenter;
      }
    }

    public float DistanceToTheGround
    {
      get
      {
        return _distanceToTheGround;
      }
    }

    private CharacterControllerParameters2D _overrideParameters;
    private CharacterCollisionState2D _collisionState;
    private Transform _transform;
    private BoxCollider2D _boxCollider;
    private Vector2 _speed;
    private float _friction = 0f;
    private float _fallSlowFactor;
    private float _currentGravity = 0f;
    private bool _gravityActive = true;
    private const float _smallValue = 0.0001f;
    private const float _obstacleHeightTolerance = 0.05f;
    private Collider2D _ignoredCollider = null;
    private int _savedBelowLayer;
    private float _movingPlatformCurrentGravity;
    private Vector2 _externalForce;
    private Vector2 _newPosition;
    private Vector2 _colliderBottomCenterPosition;
    private Vector2 _colliderLeftCenterPosition;
    private Vector2 _colliderRightCenterPosition;
    private Vector2 _colliderTopCenterPosition;
    private Vector2 _boundsTopLeftCorner;
    private Vector2 _boundsBottomLeftCorner;
    private Vector2 _boundsTopRightCorner;
    private Vector2 _boundsBottomRightCorner;
    private Vector2 _boundsCenter;
    private Vector2 _bounds;
    private Vector3 _crossBelowSlopeAngle;
    private float _boundsWidth;
    private float _boundsHeight;
    private float _distanceToTheGround;
    private Vector2 _worldSpeed;
    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;
    private float _movementDirection;
    private float _storedMovementDirection = 1f;
    private const float _movementDirectionThreshold = 0.0001f;
    private RaycastHit2D _stickRaycast;
    private RaycastHit2D _distanceToTheGroundRaycast;
    private RaycastHit2D[] _belowHitsStorage;
    private RaycastHit2D[] _aboveHitsStorage;
    private RaycastHit2D[] _sideHitsStorage;
    private Vector2 _raycastOrigin = Vector2.zero;
    private Vector2 _horizontalRayCastFromBottom = Vector2.zero;
    private Vector2 _horizontalRayCastToTop = Vector2.zero;
    private Vector2 _verticalRayCastFromLeft = Vector2.zero;
    private Vector2 _verticalRayCastToRight = Vector2.zero;
    private LayerMask _raysBelowLayerMaskPlatforms;
    private LayerMask _raysBelowLayerMaskPlatformsWithoutOneWay;
    private LayerMask _raysBelowLayerMaskPlatformsWithoutMidHeight;
    private List<RaycastHit2D> _contactList;

    private void Awake()
    {
      _transform = transform;
      _boxCollider = GetComponent<BoxCollider2D>();
      _originalColliderSize = _boxCollider.size;
      _originalColliderOffset = _boxCollider.offset;
      _contactList = new List<RaycastHit2D>();
      _collisionState = new CharacterCollisionState2D();
      _sideHitsStorage = new RaycastHit2D[numberOfHorizontalRays];
      _belowHitsStorage = new RaycastHit2D[numberOfVerticalRays];
    }

    public void AddForce(Vector2 force)
    {
      _speed += force;
      _externalForce += force;
    }
    public void AddHorizontalForce(float x)
    {
      _speed.x += x;
      _externalForce.x += x;
    }
    public void AddVerticalForce(float y)
    {
      _speed.y += y;
      _externalForce.y += y;
    }
    public void SetForce(Vector2 force)
    {
      _speed = force;
      _externalForce = force;
    }
    public void SetHorizontalForce(float x)
    {
      _speed.x = x;
      _externalForce.x = x;
    }
    public void SetVerticalForce(float y)
    {
      _speed.y = y;
      _externalForce.y = y;
    }

    private void Update()
    {
      if (Time.timeScale == 0f)
      {
        return;
      }

      ApplyGravity();
      FrameInitialization();
      SetBounds();
      // HandleMovingPlatforms();
      DetermineMovementDirection();
      CastRays();
      MoveTransform();

      //SetBounds();
      ComputeNewSpeed();
      //SetStates();
      //ComputeDistanceToTheGround();

      //_externalForce.x = 0;
      //_externalForce.y = 0;

      //FrameExit();

      //_worldSpeed = _speed;

    }

    private void ApplyGravity()
    {
      _currentGravity = defaultParameters.gravity;
      if (_speed.y > 0f)
      {
        _currentGravity = _currentGravity / Parameters.ascentMultiplier;
      }
      else if (_speed.y < 0f)
      {
        _currentGravity = _currentGravity * Parameters.fallMultiplier;
      }
      if (_gravityActive)
      {
        _speed.y += (_currentGravity + _movingPlatformCurrentGravity) * Time.deltaTime;
      }
      if (_fallSlowFactor != 0f)
      {
        _speed.y *= _fallSlowFactor;
      }
    }

    private void FrameInitialization()
    {
      _contactList.Clear();
      _newPosition = _speed * Time.deltaTime;
      CollisionState.WasGroundedLastFrame = CollisionState.IsCollidingBelow;
      CollisionState.WasTouchingTheCeilingLastFrame = CollisionState.IsCollidingAbove;
      CollisionState.Reset();
    }

    private void SetBounds()
    {
      float top = _boxCollider.offset.y + (_boxCollider.size.y / 2f);
      float bottom = _boxCollider.offset.y - (_boxCollider.size.y / 2f);
      float left = _boxCollider.offset.x - (_boxCollider.size.x / 2f);
      float right = _boxCollider.offset.x + (_boxCollider.size.x / 2f);

      _boundsTopLeftCorner.x = left;
      _boundsTopLeftCorner.y = top;

      _boundsTopRightCorner.x = right;
      _boundsTopRightCorner.y = top;

      _boundsBottomLeftCorner.x = left;
      _boundsBottomLeftCorner.y = bottom;

      _boundsBottomRightCorner.x = right;
      _boundsBottomRightCorner.y = bottom;

      _boundsTopLeftCorner = transform.TransformPoint(_boundsTopLeftCorner);
      _boundsTopRightCorner = transform.TransformPoint(_boundsTopRightCorner);
      _boundsBottomLeftCorner = transform.TransformPoint(_boundsBottomLeftCorner);
      _boundsBottomRightCorner = transform.TransformPoint(_boundsBottomRightCorner);

      _boundsCenter = _boxCollider.bounds.center;
      _boundsWidth = Vector2.Distance(_boundsBottomLeftCorner, _boundsBottomRightCorner);
      _boundsHeight = Vector2.Distance(_boundsBottomLeftCorner, _boundsTopLeftCorner);

    }

    private void HandleMovingPlatforms()
    {
      ForcesApplied = _speed;
    }

    private void DetermineMovementDirection()
    {
      _movementDirection = _storedMovementDirection;
      if (_speed.x < -_movementDirectionThreshold)
      {
        _movementDirection = -1f;
      }
      else if (_speed.x > _movementDirectionThreshold)
      {
        _movementDirection = 1f;
      }
      else if (_externalForce.x < -_movementDirectionThreshold)
      {
        _movementDirection = -1f;
      }
      else if (_externalForce.x > _movementDirectionThreshold)
      {
        _movementDirection = 1f;
      }
      //   if (_movingPlatform != null)
      //   {

      //   }
      _storedMovementDirection = _movementDirection;
    }

    private void CastRays()
    {
      if (castRaysOnBothSides)
      {
        CastRaysToTheLeft();
        CastRaysToTheRight();
      }
      else
      {
        if (_movementDirection == -1)
        {
          CastRaysToTheLeft();
        }
        else
        {
          CastRaysToTheRight();
        }
      }

      CastRaysBelow();
      CastRaysAbove();
    }

    private void CastRaysToTheLeft()
    {
      CastRaysToTheSides(-1f);
    }

    private void CastRaysToTheRight()
    {
      CastRaysToTheSides(1f);
    }

    private void CastRaysToTheSides(float raysDirection)
    {

      _horizontalRayCastFromBottom = (_boundsBottomLeftCorner + _boundsBottomRightCorner) / 2;
      _horizontalRayCastToTop = (_boundsTopLeftCorner + _boundsTopRightCorner) / 2;
      _horizontalRayCastFromBottom = _horizontalRayCastFromBottom + (Vector2)transform.up * _obstacleHeightTolerance;
      _horizontalRayCastToTop = _horizontalRayCastToTop - (Vector2)transform.up * _obstacleHeightTolerance;

      float rayLength = Mathf.Abs(_speed.x * Time.deltaTime) + _boundsWidth / 2 + rayOffset * 2;

      for (int i = 0; i < numberOfHorizontalRays; i++)
      {
        Vector2 rayOriginPoint = Vector2.Lerp(_horizontalRayCastFromBottom, _horizontalRayCastToTop, (float)i / (float)(numberOfHorizontalRays - 1));

        if (CollisionState.WasGroundedLastFrame && i == 0)
        {
          _sideHitsStorage[i] = RFG.Utils.Physics2D.Raycast(rayOriginPoint, raysDirection * transform.right, rayLength, platformMask);
        }
        else
        {
          _sideHitsStorage[i] = RFG.Utils.Physics2D.Raycast(rayOriginPoint, raysDirection * transform.right, rayLength, platformMask & ~oneWayPlatformMask & ~movingOneWayPlatformMask);
        }

        if (_sideHitsStorage[i].distance > 0)
        {
          // Check ignore list


          float hitAngle = Mathf.Abs(Vector2.Angle(_sideHitsStorage[i].normal, transform.up));

          // if (oneWayPlatformMask.Contains(_sideHitsStorage[i].collider.gameObject))
          // {
          //   if (hitAngle > 90)
          //   {
          //     break;
          //   }
          // }

          if (_movementDirection == raysDirection)
          {
            CollisionState.LateralSlopeAngle = hitAngle;
          }

          if (hitAngle > Parameters.maxSlopeAngle)
          {

            if (raysDirection < 0)
            {
              CollisionState.IsCollidingLeft = true;
              CollisionState.DistanceToLeftCollider = _sideHitsStorage[i].distance;
            }
            else
            {
              CollisionState.IsCollidingRight = true;
              CollisionState.DistanceToRightCollider = _sideHitsStorage[i].distance;
            }

            if (_movementDirection == raysDirection || (castRaysOnBothSides && _speed.x == 0f))
            {
              // CurrentWallCollider = _sideHitsStorage[i].collider.gameObject;
              CollisionState.SlopeAngleOK = false;

              float distance = Math.DistanceBetweenPointAndLine(_sideHitsStorage[i].point, _horizontalRayCastFromBottom, _horizontalRayCastToTop);

              if (raysDirection <= 0)
              {
                _newPosition.x = -distance + _boundsWidth / 2 + rayOffset * 2;
              }
              else
              {
                _newPosition.x = distance - _boundsWidth / 2 - rayOffset * 2;
              }

              // if we're in the air, we prevent the character from being pushed back
              if (!CollisionState.IsGrounded && _speed.y != 0 && !Mathf.Approximately(hitAngle, 90f))
              {
                _newPosition.x = 0;
              }

              _contactList.Add(_sideHitsStorage[i]);
              _speed.x = 0;

              break;

            }
          }

        }

      }
    }

    private void CastRaysBelow()
    {
      _friction = 0;

      CollisionState.IsFalling = _newPosition.y < -_smallValue;

      if (Parameters.gravity > 0f && !CollisionState.IsFalling)
      {
        CollisionState.IsCollidingBelow = false;
        return;
      }

      float rayLength = (_boundsHeight / 2) + rayOffset;

      //   if (CollisionState.OnAMovingPlatform)
      //   {
      //     rayLength *= 2;
      //   }

      //   if (_newPosition.y < 0f)
      //   {
      //     rayLength += Mathf.Abs(_newPosition.y);
      //   }

      _verticalRayCastFromLeft = (_boundsBottomLeftCorner + _boundsTopLeftCorner) / 2;
      _verticalRayCastToRight = (_boundsBottomRightCorner + _boundsTopRightCorner) / 2;
      _verticalRayCastFromLeft += (Vector2)transform.up * rayOffset;
      _verticalRayCastToRight += (Vector2)transform.up * rayOffset;
      _verticalRayCastFromLeft += (Vector2)transform.right * _newPosition.x;
      _verticalRayCastToRight += (Vector2)transform.right * _newPosition.x;

      _raysBelowLayerMaskPlatforms = platformMask;

      // _raysBelowLayerMaskPlatformsWithoutOneWay;
      // _raysBelowLayerMaskPlatformsWithoutMidHeight;

      // If we are standing on a mid height one way playform, turn it into a regular platform
      if (StandingOnLastFrame)
      {
        _savedBelowLayer = StandingOnLastFrame.layer;
        if (midHeightOneWayPlatformMask.Contains(StandingOnLastFrame.layer))
        {
          StandingOnLastFrame.layer = LayerMask.NameToLayer("Platforms");
        }
      }

      // if we were grounded last frame and not on a one way platform we ignore any one way platform in our path
      //   if (CollisionState.WasGroundedLastFrame)
      //   {
      //     if (StandingOnLastFrame != null)
      //     {
      //       if (!midHeightOneWayPlatformMask.Contains(StandingOnLastFrame.layer))
      //       {
      //         _raysBelowLayerMaskPlatforms = _raysBelowLayerMaskPlatformsWithoutMidHeight;
      //       }
      //     }
      //   }

      // stairs

      // Moving platform

      float smallestDistance = float.MaxValue;
      int smallestDistanceIndex = 0;
      bool hitConnected = false;

      for (int i = 0; i < numberOfVerticalRays; i++)
      {
        Vector2 rayOriginPoint = Vector2.Lerp(_verticalRayCastFromLeft, _verticalRayCastToRight, (float)i / (float)(numberOfVerticalRays - 1));

        // if (_newPosition.y > 0 && !CollisionState.WasGroundedLastFrame)
        // {
        //   _belowHitsStorage[i] = RFG.Utils.Physics2D.Raycast(rayOriginPoint, -transform.up, rayLength, _raysBelowLayerMaskPlatformsWithoutOneWay);
        // }
        // else
        // {
        _belowHitsStorage[i] = RFG.Utils.Physics2D.Raycast(rayOriginPoint, -transform.up, rayLength, _raysBelowLayerMaskPlatforms);
        //}

        float distance = Math.DistanceBetweenPointAndLine(_belowHitsStorage[smallestDistanceIndex].point, _verticalRayCastFromLeft, _verticalRayCastToRight);

        if (_belowHitsStorage[i])
        {
          if (_belowHitsStorage[i].collider == _ignoredCollider)
          {
            continue;
          }


          hitConnected = true;
          CollisionState.BelowSlopeAngle = Vector2.Angle(_belowHitsStorage[i].normal, transform.up);
          _crossBelowSlopeAngle = Vector3.Cross(transform.up, _belowHitsStorage[i].normal);

          if (_crossBelowSlopeAngle.z < 0)
          {
            CollisionState.BelowSlopeAngle = -CollisionState.BelowSlopeAngle;
          }

          if (_belowHitsStorage[i].distance < smallestDistance)
          {
            smallestDistanceIndex = i;
            smallestDistance = _belowHitsStorage[i].distance;
          }
        }

        if (distance < _smallValue)
        {
          break;
        }
      }

      if (hitConnected)
      {
        // StandingOnCollider = _belowHitsStorage[smallestDistanceIndex].collider;
        // standingOn = StandingOnCollider.gameObject;

        CollisionState.IsFalling = false;
        CollisionState.IsCollidingBelow = true;

        float distance = Math.DistanceBetweenPointAndLine(_belowHitsStorage[smallestDistanceIndex].point, _verticalRayCastFromLeft, _verticalRayCastToRight);
        _newPosition.y = -distance + _boundsHeight / 2 + rayOffset;

        if (!CollisionState.WasGroundedLastFrame && _speed.y > 0)
        {
          _newPosition.y += _speed.y * Time.deltaTime;
        }

        if (Mathf.Abs(_newPosition.y) < _smallValue)
        {
          _newPosition.y = 0f;
        }

        // Friction

        // Moving Platform

      }
      else
      {
        CollisionState.IsCollidingBelow = false;
      }
    }
    private void CastRaysAbove()
    {

    }

    private void MoveTransform()
    {
      //   if (performSafetyBoxcast)
      //   {
      //     _stickRaycast = Physics2D.BoxCast(_boundsCenter, Bounds, Vector2.Angle(transform.up, Vector2.up), _newPosition.normalized, _newPosition.magnitude, platformMask);
      //     if (_stickRaycast)
      //     {
      //       if (Mathf.Abs(_stickRaycast.distance - _newPosition.magnitude) < -0.0002f)
      //       {
      //         _newPosition = Vector2.zero;
      //       }
      //     }
      //   }
      _transform.Translate(_newPosition, Space.Self);
    }

    private void ComputeNewSpeed()
    {
      if (Time.deltaTime > 0)
      {
        _speed = _newPosition / Time.deltaTime;
      }

      if (CollisionState.IsGrounded)
      {
        _speed.x *= Parameters.slopeAngleSpeedFactor.Evaluate(Mathf.Abs(CollisionState.BelowSlopeAngle) * Mathf.Sign(_speed.y));
      }

      if (!CollisionState.OnAMovingPlatform)
      {
        _speed.x = Mathf.Clamp(_speed.x, -Parameters.maxVelocity.x, Parameters.maxVelocity.x);
        _speed.y = Mathf.Clamp(_speed.y, -Parameters.maxVelocity.y, Parameters.maxVelocity.y);
      }
    }

    private void SetStates()
    {
      if (!CollisionState.WasGroundedLastFrame && CollisionState.IsCollidingBelow)
      {
        CollisionState.JustGotGrounded = true;
      }

      if (CollisionState.HasCollision)
      {
        OnColliderHit();
      }
    }

    private void ComputeDistanceToTheGround()
    {
      if (distanceToTheGroundRayMaximumLength <= 0)
      {
        return;
      }

      _raycastOrigin.x = (CollisionState.BelowSlopeAngle < 0) ? _boundsBottomLeftCorner.x : _boundsBottomRightCorner.x;
      _raycastOrigin.y = _boundsCenter.y;

      _distanceToTheGroundRaycast = RFG.Utils.Physics2D.Raycast(_raycastOrigin, -transform.up, distanceToTheGroundRayMaximumLength, _raysBelowLayerMaskPlatforms);

      if (_distanceToTheGroundRaycast)
      {
        if (_distanceToTheGroundRaycast.collider == _ignoredCollider)
        {
          _distanceToTheGround = -1f;
          return;
        }
        _distanceToTheGround = _distanceToTheGroundRaycast.distance - _boundsHeight / 2;
      }
      else
      {
        _distanceToTheGround = -1f;
      }
    }

    private void FrameExit()
    {
      if (StandingOnLastFrame != null)
      {
        StandingOnLastFrame.layer = _savedBelowLayer;
      }
    }

    private void OnColliderHit()
    {

    }

  }
}