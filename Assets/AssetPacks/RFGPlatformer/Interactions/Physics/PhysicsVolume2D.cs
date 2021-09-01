using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Interactions/Physics/PhysicsVolume2D")]
    public class PhysicsVolume2D : MonoBehaviour
    {
      public enum VolumeType { Water }
      public VolumeType volumeType = VolumeType.Water;
      public CharacterControllerParameters2D parameters;

      private void OnTriggerEnter2D(Collider2D other)
      {
        CharacterController2D character = other.gameObject.GetComponent<CharacterController2D>();
        if (character != null)
        {
          character.SetForce(Vector2.zero);
          character.SetOverrideParameters(parameters);
        }
        JumpAbility jumpAbility = other.gameObject.GetComponent<JumpAbility>();
        if (jumpAbility != null)
        {
          jumpAbility.JumpSettings.Restrictions = JumpSettings.JumpRestrictions.CanJumpAnywhere;
        }
      }

      private void OnTriggerExit2D(Collider2D other)
      {
        CharacterController2D character = other.gameObject.GetComponent<CharacterController2D>();
        if (character != null)
        {
          character.SetForce(Vector2.zero);
          character.SetOverrideParameters(null);
        }
        JumpAbility jumpAbility = other.gameObject.GetComponent<JumpAbility>();
        if (jumpAbility != null)
        {
          jumpAbility.JumpSettings.Restrictions = JumpSettings.JumpRestrictions.CanJumpOnGround;
        }
      }
    }
  }
}