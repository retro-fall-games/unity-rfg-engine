using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Items/Pick Ups/Ability Pick Up")]
  public class AbilityPickUp : BasePickUp
  {

    public override void OnPickup(Collider2D col)
    {
      switch (item.ability)
      {
        case CharacterAbilities.DoubleJump:
          JumpBehaviour jumpBehaviour = col.gameObject.GetComponent<JumpBehaviour>();
          if (jumpBehaviour != null)
          {
            jumpBehaviour.numberOfJumps = 2;
          }
          break;
        case CharacterAbilities.Dash:
          DashBehaviour dashBehaviour = col.gameObject.GetComponent<DashBehaviour>();
          if (dashBehaviour != null)
          {
            dashBehaviour.authorized = true;
          }
          break;
        case CharacterAbilities.WallCling:
        case CharacterAbilities.WallJump:
          // If you get either of these then you get both
          WallClingingBehaviour wallClingBehaviour = col.gameObject.GetComponent<WallClingingBehaviour>();
          if (wallClingBehaviour != null)
          {
            wallClingBehaviour.authorized = true;
          }
          WallJumpBehaviour wallJumpBehaviour = col.gameObject.GetComponent<WallJumpBehaviour>();
          if (wallJumpBehaviour != null)
          {
            wallJumpBehaviour.authorized = true;
          }
          break;
      }
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void GeneratePickup()
    {
      if (gameObject.GetComponent<SpriteRenderer>() == null)
      {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.pickupSprite;
      }

      if (gameObject.GetComponent<BoxCollider2D>() == null)
      {
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
      }
      tag = "PickUp";
      layerMask = LayerMask.GetMask("Player");
    }
#endif

  }
}