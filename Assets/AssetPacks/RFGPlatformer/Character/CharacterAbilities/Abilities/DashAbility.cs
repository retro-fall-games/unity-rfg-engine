using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Dash Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Dash")]
    public class DashAbility : CharacterAbility
    {
      [Header("Dash")]
      public float DashDistance = 3f;
      public float DashForce = 40f;
      public int TotalDashes = 2;
      public int NumberOfDashesLeft = 2;

      [Header("Direction")]
      public Aim aim;
      public float MinInputThreshold = 0.1f;

      [Header("Cooldown")]
      public float Cooldown = 1f;

      [Header("Effects")]
      public string[] DashEffects;

      [HideInInspector]
      private Vector2 _dashDirection;
      private float _distanceTraveled = 0f;
      private bool _shouldKeepDashing = true;
      private Vector3 _initialPosition;
      private float _cooldownTimestamp;
      private IEnumerator _dashCoroutine;
      private float _lastDashAt = 0f;

      public override void Init(CharacterAbilityController.AbilityContext ctx)
      {
        _cooldownTimestamp = 0;
        NumberOfDashesLeft = TotalDashes;
        aim.Init();
      }

      public override void Process(CharacterAbilityController.AbilityContext ctx)
      {
        if (ctx.character.CharacterMovementState.CurrentStateType == typeof(DashingState))
        {
          ctx.character.Controller.GravityActive(false);
        }
        HandleAmountOfDashesLeft(ctx);
      }

      public override void OnButtonStarted(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        StartDash(ctx);
      }

      public override void OnButtonCanceled(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
      }

      public override void OnButtonPerformed(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
      }

      private void StartDash(CharacterAbilityController.AbilityContext ctx)
      {
        if (_cooldownTimestamp > Time.time)
        {
          return;
        }

        if (NumberOfDashesLeft <= 0)
        {
          return;
        }

        ctx.character.transform.SpawnFromPool("Effects", DashEffects);

        ctx.character.Controller.CollisionsOnStairs(true);
        ctx.character.CharacterMovementState.ChangeState(typeof(DashingState));
        _cooldownTimestamp = Time.time + Cooldown;
        _distanceTraveled = 0f;
        _shouldKeepDashing = true;
        _initialPosition = ctx.transform.position;
        _lastDashAt = Time.time;

        NumberOfDashesLeft--;

        ComputerDashDirection(ctx);
        CheckFlipCharacter(ctx);

        _dashCoroutine = Dash(ctx);
        ctx.character.StartCoroutine(_dashCoroutine);
      }

      private void ComputerDashDirection(CharacterAbilityController.AbilityContext ctx)
      {
        aim.PrimaryMovement = ctx.input.PrimaryMovement;
        aim.CurrentPosition = ctx.transform.position;
        _dashDirection = aim.GetCurrentAim();

        CheckAutoCorrectTrajectory(ctx);

        if (_dashDirection.magnitude < MinInputThreshold)
        {
          _dashDirection = ctx.character.Controller.State.IsFacingRight ? Vector2.right : Vector2.left;
        }
        else
        {
          _dashDirection = _dashDirection.normalized;
        }
      }

      private void CheckAutoCorrectTrajectory(CharacterAbilityController.AbilityContext ctx)
      {
        if (ctx.character.Controller.State.IsCollidingBelow && _dashDirection.y < 0f)
        {
          _dashDirection.y = 0f;
        }
      }

      private void CheckFlipCharacter(CharacterAbilityController.AbilityContext ctx)
      {
        if (Mathf.Abs(_dashDirection.x) > 0.05f)
        {
          if (ctx.character.Controller.State.IsFacingRight != _dashDirection.x > 0f)
          {
            ctx.character.Controller.Flip();
          }
        }
      }

      private IEnumerator Dash(CharacterAbilityController.AbilityContext ctx)
      {
        while (_distanceTraveled < DashDistance && _shouldKeepDashing && ctx.character.CharacterMovementState.CurrentStateType == typeof(DashingState))
        {
          _distanceTraveled = Vector3.Distance(_initialPosition, ctx.transform.position);

          if ((ctx.character.Controller.State.IsCollidingLeft && _dashDirection.x < 0f)
            || (ctx.character.Controller.State.IsCollidingRight && _dashDirection.x > 0f)
            || (ctx.character.Controller.State.IsCollidingAbove && _dashDirection.y > 0f)
            || (ctx.character.Controller.State.IsCollidingBelow && _dashDirection.y < 0f))
          {
            _shouldKeepDashing = false;
            ctx.character.Controller.SetForce(Vector2.zero);
          }
          else
          {
            ctx.character.Controller.GravityActive(false);
            ctx.character.Controller.SetForce(_dashDirection * DashForce);
          }
          yield return null;
        }
        StopDash(ctx);
      }

      private void StopDash(CharacterAbilityController.AbilityContext ctx)
      {
        if (_dashCoroutine != null)
        {
          GameManager.Instance.StopCoroutine(_dashCoroutine);
        }
        ctx.character.Controller.GravityActive(true);
        ctx.character.Controller.SetForce(Vector2.zero);

        if (ctx.character.CharacterMovementState.CurrentStateType == typeof(DashingState))
        {
          if (ctx.character.Controller.State.IsGrounded)
          {
            ctx.character.CharacterMovementState.ChangeState(typeof(IdleState));
          }
          else
          {
            ctx.character.CharacterMovementState.RestorePreviousState();
          }
        }

      }

      public void SetNumberOfDashesLeft(int numberLeft)
      {
        NumberOfDashesLeft = numberLeft;
      }

      private void HandleAmountOfDashesLeft(CharacterAbilityController.AbilityContext ctx)
      {
        if (Time.time - _lastDashAt < Cooldown)
        {
          return;
        }

        if (ctx.character.Controller.State.IsGrounded)
        {
          SetNumberOfDashesLeft(TotalDashes);
        }
      }

    }
  }
}