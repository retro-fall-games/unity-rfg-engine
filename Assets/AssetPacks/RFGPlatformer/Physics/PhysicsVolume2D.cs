using UnityEngine;

namespace RFG.Platformer
{
  [AddComponentMenu("RFG Platformer/Physics/PhysicsVolume2D")]
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
        // character.ResetVelocity();
        character.SetOverrideParameters(parameters);
      }
      JumpBehavior jumpBehavior = other.gameObject.GetComponent<JumpBehavior>();
      if (jumpBehavior != null)
      {
        jumpBehavior.jumpRestrictions = JumpBehavior.JumpRestrictions.CanJumpAnywhere;
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      CharacterController2D character = other.gameObject.GetComponent<CharacterController2D>();
      if (character != null)
      {
        // character.ResetVelocity();
        character.SetOverrideParameters(null);
      }
      JumpBehavior jumpBehavior = other.gameObject.GetComponent<JumpBehavior>();
      if (jumpBehavior != null)
      {
        jumpBehavior.jumpRestrictions = JumpBehavior.JumpRestrictions.CanJumpOnGround;
      }
    }
  }
}