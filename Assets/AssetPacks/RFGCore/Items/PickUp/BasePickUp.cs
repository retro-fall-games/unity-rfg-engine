using UnityEngine;
using MyBox;

namespace RFG
{
  public class BasePickUp : MonoBehaviour
  {
    public PickUpItem item;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    public LayerMask layerMask;
    public float respawnTime = 0f;
    private float _respawnTimeElapsed = 0f;

    private void LateUpdate()
    {
      if (spriteRenderer.enabled == false && respawnTime > 0f)
      {
        if (_respawnTimeElapsed > respawnTime)
        {
          _respawnTimeElapsed = 0f;
          Spawn();
        }
        _respawnTimeElapsed += Time.deltaTime;
      }
    }

    private void Spawn()
    {
      boxCollider.enabled = true;
      spriteRenderer.enabled = true;
    }

    private void Kill()
    {
      if (respawnTime > 0f)
      {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
      }
      else
      {
        Destroy(gameObject);
      }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (layerMask.Contains(col.gameObject.layer))
      {
        OnPickup(col);
        Kill();
      }
    }

    public virtual void OnPickup(Collider2D col)
    {
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