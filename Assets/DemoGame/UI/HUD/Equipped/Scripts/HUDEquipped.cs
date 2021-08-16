using System.Collections;
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

    private void OnEquipPrimaryWeapon(Weapon weapon)
    {
      PrimaryEquipped.sprite = weapon.WeaponItem.EquipSprite;
      OnPrimaryAmmoChange(weapon.Ammo);
      weapon.OnAmmoChange += OnPrimaryAmmoChange;
    }

    private void OnEquipSecondaryWeapon(Weapon weapon)
    {
      SecondaryEquipped.sprite = weapon.WeaponItem.EquipSprite;
      OnSecondaryAmmoChange(weapon.Ammo);
      weapon.OnAmmoChange += OnSecondaryAmmoChange;
    }

    private void OnUnequipPrimaryWeapon(Weapon weapon)
    {
      weapon.OnAmmoChange -= OnPrimaryAmmoChange;
    }

    private void OnUnequipSecondaryWeapon(Weapon weapon)
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