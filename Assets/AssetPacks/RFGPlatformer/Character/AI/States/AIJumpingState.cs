// using System;
// using UnityEngine;

// namespace RFG
// {
//   namespace Platformer
//   {
//     [CreateAssetMenu(fileName = "New AI Jumping State", menuName = "RFG/Platformer/Character/AI State/Jumping")]
//     public class AIJumpingState : AIState
//     {
//       public override Type Execute(AIStateContext ctx)
//       {
//         if (ctx.characterContext.settingsPack == null || ctx.characterContext.settingsPack.JumpSettings == null)
//           return null;

//         if (ctx.controller.State.JustGotGrounded)
//         {
//           JumpSettings JumpSettings = ctx.characterContext.settingsPack.JumpSettings;
//           ctx.controller.SetHorizontalForce(0);
//           ctx.transform.SpawnFromPool("Effects", JumpSettings.LandEffects);
//           ctx.aiState.RestorePreviousDecision();
//         }
//         if (ctx.controller.State.IsGrounded)
//         {
//           JumpStart(ctx);
//         }
//         return null;
//       }

//       public void JumpStart(AIStateContext ctx)
//       {
//         if (!CanJump(ctx))
//         {
//           return;
//         }

//         JumpSettings JumpSettings = ctx.characterContext.settingsPack.JumpSettings;
//         WalkingSettings WalkingSettings = ctx.characterContext.settingsPack.WalkingSettings;
//         RunningSettings RunningSettings = ctx.characterContext.settingsPack.RunningSettings;
//         ctx.transform.SpawnFromPool("Effects", JumpSettings.JumpEffects);
//         ctx.animator.Play(JumpSettings.JumpingClip);

//         // Jump
//         ctx.controller.State.IsFalling = false;
//         ctx.controller.State.IsJumping = true;
//         ctx.controller.AddVerticalForce(Mathf.Sqrt(2f * JumpSettings.JumpHeight * Mathf.Abs(ctx.controller.Parameters.Gravity)));

//         if (WalkingSettings != null)
//         {
//           // Move horizontally
//           float normalizedHorizontalSpeed = 0f;
//           if (ctx.controller.State.IsFacingRight)
//           {
//             normalizedHorizontalSpeed = 1f;
//           }
//           else
//           {
//             normalizedHorizontalSpeed = -1f;
//           }

//           float speed = WalkingSettings.WalkingSpeed;
//           if (ctx.aggro != null && ctx.aggro.HasAggro && RunningSettings != null)
//           {
//             speed = RunningSettings.RunningSpeed;
//           }

//           float movementFactor = ctx.controller.Parameters.AirSpeedFactor;
//           float movementSpeed = normalizedHorizontalSpeed * speed * ctx.controller.Parameters.SpeedFactor;
//           float horizontalMovementForce = Mathf.Lerp(ctx.controller.Speed.x, movementSpeed, Time.deltaTime * movementFactor);

//           ctx.controller.SetHorizontalForce(horizontalMovementForce);
//         }

//         JumpStop(ctx);
//       }

//       private void JumpStop(AIStateContext ctx)
//       {
//         ctx.controller.State.IsFalling = true;
//         ctx.controller.State.IsJumping = false;
//         ctx.aiState.RestorePreviousDecision();
//       }

//       private bool CanJump(AIStateContext ctx)
//       {
//         JumpSettings JumpSettings = ctx.characterContext.settingsPack.JumpSettings;
//         if (JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpAnywhere)
//         {
//           return true;
//         }
//         if (JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpOnGround && ctx.controller.State.IsGrounded)
//         {
//           return true;
//         }
//         return false;
//       }
//     }
//   }
// }