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

    [Header("Visual FX")]
    public GameObject[] pickupFx;
    public string[] objectPoolPickupFx;

    [Header("Audio FX")]
    public AudioSource pickupAudio;

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
            PickUpFx(weapon.pickupText);
          }
          else
          {
            weaponBehaviour.RefillWeapon(weapon);
            PickUpFx(weapon.pickupAmmoText);
          }

          weaponBehaviour.UpdateAmmoCounts();

        }

        if (pickupAudio != null)
        {
          pickupAudio.Play();
        }
        Kill();
      }
    }

    private void Spawn()
    {
      boxCollider.enabled = true;
      spriteRenderer.enabled = true;
    }

    private void PickUpFx(string text)
    {
      if (pickupFx.Length > 0)
      {
        foreach (GameObject fx in pickupFx)
        {
          GameObject instance = Instantiate(fx, transform.position, Quaternion.identity);
          PickUpFxOnInstance(instance, text);
        }
      }
      if (objectPoolPickupFx.Length > 0)
      {
        foreach (string fx in objectPoolPickupFx)
        {
          GameObject instance = ObjectPool.Instance.SpawnFromPool(fx, transform.position, Quaternion.identity);
          PickUpFxOnInstance(instance, text);
        }
      }
    }

    private void PickUpFxOnInstance(GameObject instance, string text)
    {
      FloatingText t = instance.GetComponent<FloatingText>();
      if (t != null)
      {
        t.text.SetText(text);
      }
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