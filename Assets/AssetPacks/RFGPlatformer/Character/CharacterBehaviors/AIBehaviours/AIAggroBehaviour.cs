using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Aggro Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Aggro")]
    public class AIAggroBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float RunSpeed = 5f;

      public override void Init(CharacterBehaviourController.BehaviourContext ctx)
      {
        Aggro aggro = ctx.character.gameObject.GetComponent<Aggro>();
        aggro.OnAggroChange += OnAggroChange;
      }

      public override void Remove(CharacterBehaviourController.BehaviourContext ctx)
      {
        Aggro aggro = ctx.character.gameObject.GetComponent<Aggro>();
        aggro.OnAggroChange -= OnAggroChange;
      }

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        // if (_character.AIState.CurrentState == AIStates.Attacking)
        // {
        //   // Rotate to always face the target
        //   _character.Controller.RotateTowards(_aggro.target2);

        //   // Move towards that target
        //   float _normalizedHorizontalSpeed = 0f;

        //   if (_character.Controller.State.IsFacingRight)
        //   {
        //     _character.AIMovementState.ChangeState(AIMovementStates.RunningRight);
        //     _normalizedHorizontalSpeed = 1f;
        //   }
        //   else
        //   {
        //     _character.AIMovementState.ChangeState(AIMovementStates.RunningLeft);
        //     _normalizedHorizontalSpeed = -1f;
        //   }

        //   _character.MovementState.ChangeState(MovementStates.Running);

        //   float movementFactor = _character.Controller.State.IsGrounded ? _character.Controller.Parameters.GroundSpeedFactor : _character.Controller.Parameters.AirSpeedFactor;
        //   float movementSpeed = _normalizedHorizontalSpeed * runSpeed * _character.Controller.Parameters.SpeedFactor;
        //   float horizontalMovementForce = Mathf.Lerp(_character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        //   _character.Controller.SetHorizontalForce(horizontalMovementForce);
      }

      private void OnAggroChange(bool aggro)
      {
        // if (aggro)
        // {
        //   _character.AIState.ChangeState(AIStates.Attacking);
        // }
        // else
        // {
        //   _character.AIState.ChangeState(AIStates.Wandering);
        // }
      }
    }
  }
}