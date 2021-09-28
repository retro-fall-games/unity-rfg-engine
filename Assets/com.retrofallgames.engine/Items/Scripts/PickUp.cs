using System.Collections;
using UnityEngine;
using MyBox;

namespace RFG.Items
{
  [AddComponentMenu("RFG/Items/Pick Up")]
  public class PickUp : MonoBehaviour
  {
    [Header("Settings")]
    public Item item;
    public LayerMask LayerMask;
    public float RespawnTime = 0f;

    [HideInInspector]
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private float _respawnTimeElapsed = 0f;

    private void Awake()
    {
      _spriteRenderer = GetComponent<SpriteRenderer>();
      _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void LateUpdate()
    {
      if (_spriteRenderer.enabled == false && RespawnTime > 0f)
      {
        if (_respawnTimeElapsed > RespawnTime)
        {
          _respawnTimeElapsed = 0f;
          Spawn();
        }
        _respawnTimeElapsed += Time.deltaTime;
      }
    }

    private void Spawn()
    {
      _boxCollider.enabled = true;
      _spriteRenderer.enabled = true;
    }

    private void Kill()
    {
      _boxCollider.enabled = false;
      _spriteRenderer.enabled = false;
      if (RespawnTime == 0f)
      {
        StartCoroutine(KillCo());
      }
    }

    private IEnumerator KillCo()
    {
      yield return new WaitForSeconds(3f);
      Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (LayerMask.Contains(col.gameObject.layer))
      {
        Inventory inventory = col.gameObject.GetComponent<Inventory>();
        if (inventory != null)
        {
          inventory.Add(item);
        }
        Kill();
      }
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void GeneratePickup()
    {
      if (gameObject.GetComponent<SpriteRenderer>() == null)
      {
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = item.PickUpSprite;
      }

      if (gameObject.GetComponent<BoxCollider2D>() == null)
      {
        _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
      }
      tag = "PickUp";
      LayerMask = LayerMask.GetMask("Player");
    }
#endif
  }
}