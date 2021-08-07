using UnityEngine;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Engine/Items/Pick Ups/Health Pick Up")]
    public class HealthPickUp : BasePickUp
    {

      public override void OnPickup(Collider2D col)
      {
        HealthBehaviour health = col.gameObject.GetComponent<HealthBehaviour>();
        if (health != null)
        {
          health.AddHealth(item.healthAmount);
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
}