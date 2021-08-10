using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class AnimationBehaviour : CharacterBehaviour
    {
      [Header("Animation Settings")]
      public string Layer = "Base Layer";
      public string DefaultClip = "Idle";
      public string Idle = "Idle";
      public string Walking = "Walking";
      public string Running = "Running";
      public string Falling = "Falling";
      public string Jumping = "Jumping";
      public string WallJumping = "WallJumping";
      public string WallClinging = "WallClinging";
      public string Dashing = "Dashing";



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
      //   
      // }

    }
  }
}