using UnityEngine;
using RFG;
using MyBox;

namespace Game
{
  public class WeaponPickUp : MonoBehaviour
  {
    [Header("Settings")]
    public WeaponItem weapon;
    public bool autoEquipOnPickup;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    public LayerMask layerMask;
    public float respawnTime = 0f;

    [HideInInspector]
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

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (layerMask.Contains(col.gameObject.layer))
      {
        WeaponBehaviour weaponBehaviour = col.gameObject.GetComponent<WeaponBehaviour>();
        if (weaponBehaviour != null)
        {
          if (!weaponBehaviour.Inventory.Contains(weapon))
          {
            weaponBehaviour.AddWeapon(weapon);
            weapon.firePoint = col.gameObject.transform.Find("FirePoint");
            if (autoEquipOnPickup)
            {
              if (weaponBehaviour.PrimaryWeapon == null || weaponBehaviour.PrimaryWeapon.weaponState == WeaponItem.WeaponState.Unequiped)
              {
                weaponBehaviour.EquipPrimary(weapon);
              }
              else
              {
                if (weaponBehaviour.SecondaryWeapon == null || weaponBehaviour.SecondaryWeapon.weaponState == WeaponItem.WeaponState.Unequiped)
                {
                  weaponBehaviour.EquipSecondary(weapon);
                }
              }
            }
          }
          else
          {
            weaponBehaviour.RefillWeapon(weapon);
          }

          weaponBehaviour.UpdateAmmoCounts();

        }
        Kill();
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

#if UNITY_EDITOR
    [ButtonMethod]
    private void GeneratePickup()
    {
      if (gameObject.GetComponent<SpriteRenderer>() == null)
      {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.pickupSprite;
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