using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RFG.Platformer;

namespace Game
{
  public class HUDEquipped : MonoBehaviour
  {
    [Header("HUD")]
    public Image PrimaryEquipped;
    public Image SecondaryEquipped;
    public TMP_Text PrimaryAmmo;
    public TMP_Text SecondaryAmmo;
    public EquipmentSet EquipmentSet;

    private void OnEquipPrimaryWeapon(WeaponItem weapon)
    {
      PrimaryEquipped.sprite = weapon.EquipSprite;
      OnPrimaryAmmoChange(weapon.Ammo);
      weapon.OnAmmoChange += OnPrimaryAmmoChange;
    }

    private void OnEquipSecondaryWeapon(WeaponItem weapon)
    {
      SecondaryEquipped.sprite = weapon.EquipSprite;
      OnSecondaryAmmoChange(weapon.Ammo);
      weapon.OnAmmoChange += OnSecondaryAmmoChange;
    }

    private void OnUnequipPrimaryWeapon(WeaponItem weapon)
    {
      weapon.OnAmmoChange -= OnPrimaryAmmoChange;
    }

    private void OnUnequipSecondaryWeapon(WeaponItem weapon)
    {
      weapon.OnAmmoChange -= OnSecondaryAmmoChange;
    }

    private void OnPrimaryAmmoChange(int ammo)
    {
      PrimaryAmmo.SetText(ammo.ToString());
    }

    private void OnSecondaryAmmoChange(int ammo)
    {
      SecondaryAmmo.SetText(ammo.ToString());
    }

    private void OnEnable()
    {
      EquipmentSet.OnEquipPrimaryWeapon += OnEquipPrimaryWeapon;
      EquipmentSet.OnEquipSecondaryWeapon += OnEquipSecondaryWeapon;
      EquipmentSet.OnUnequipPrimaryWeapon += OnUnequipPrimaryWeapon;
      EquipmentSet.OnUnequipSecondaryWeapon += OnUnequipSecondaryWeapon;
    }

    private void OnDisable()
    {
      EquipmentSet.OnEquipPrimaryWeapon -= OnEquipPrimaryWeapon;
      EquipmentSet.OnEquipSecondaryWeapon -= OnEquipSecondaryWeapon;
      EquipmentSet.OnUnequipPrimaryWeapon -= OnUnequipPrimaryWeapon;
      EquipmentSet.OnUnequipSecondaryWeapon -= OnUnequipSecondaryWeapon;
    }

  }
}