using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Items/Pick Ups/Heart Pick Up")]
  public class HeartPickUp : BasePickUp
  {
    public override void OnPickup(Collider2D col)
    {
      HealthBehaviour health = col.gameObject.GetComponent<HealthBehaviour>();
      if (health != null)
      {
        health.SetMaxHealth(health.maxHealth + item.healthIncreaseAmount);
        health.SetHealth(health.maxHealth);
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