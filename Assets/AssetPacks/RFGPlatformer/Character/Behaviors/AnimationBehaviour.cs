using System.Collections;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Platformer/Character/Behaviour/Animation Behaviour")]
    public class AnimationBehaviour : CharacterBehaviour
    {

      [Header("Settings")]
      public Animator animator;
      public string layer = "Base Layer";

      [Header("Animation Behaviour Clips")]
      public string idle = "Idle";
      public string walking = "Walking";
      public string running = "Running";
      public string falling = "Falling";
      public string jumping = "Jumping";
      public string wallJumping = "WallJumping";
      public string wallClinging = "WallClinging";
      public string dashing = "Dashing";

      // public override void InitBehaviour()
      // {
      //   StartCoroutine(InitBehaviourCo());
      // }

      // private IEnumerator InitBehaviourCo()
      // {
      //   // yield return new WaitUntil(() => _character != null);
      //   // yield return new WaitUntil(() => _character.MovementState != null);
      //   // _character.MovementState.OnStateChange += MovementOnStateChanged;
      // }

      // private void MovementOnStateChanged(MovementStates state)
      // {
      //   string clip = state.ToString();
      //   int layerIndex = animator.GetLayerIndex(layer);
      //   int hash = Animator.StringToHash(clip);
      //   if (animator.HasState(layerIndex, hash))
      //   {
      //     animator.Play(state.ToString());
      //   }
      //   else
      //   {
      //     animator.Play("Idle");
      //   }
      // }

    }
  }
}