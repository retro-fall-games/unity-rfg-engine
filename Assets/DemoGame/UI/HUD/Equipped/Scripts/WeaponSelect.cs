using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RFG;
using RFG.Platformer;

namespace Game
{
  public class WeaponSelect : MonoBehaviour
  {
    public Animator animator;
    public Image[] weaponImages;
    public bool EquipPrimary { get; set; }
    public bool EquipSecondary { get; set; }
    public Inventory Inventory;
    public EquipmentSet EquipmentSet;

    private List<WeaponItem> _weapons;

    private void Awake()
    {
      foreach (Image weaponImage in weaponImages)
      {
        weaponImage.sprite = null;
        weaponImage.transform.parent.gameObject.SetActive(false);
      }
      EquipPrimary = false;
      EquipSecondary = false;
      animator.Play("WeaponSelectHidden");
    }

    public void OpenPrimarySelect()
    {
      EquipPrimary = true;
      EquipSecondary = false;
      OpenDialog();
    }

    public void OpenSecondarySelect()
    {
      EquipPrimary = false;
      EquipSecondary = true;
      OpenDialog();
    }

    private void OpenDialog()
    {
      _weapons = Inventory.FindAll<WeaponItem>();
      // TODO - Need to filter out equipped items
      for (int i = 0; i < _weapons.Count; i++)
      {
        weaponImages[i].sprite = _weapons[i].EquipSprite;
        weaponImages[i].color = Color.white;
        weaponImages[i].transform.parent.gameObject.SetActive(true);
      }
      animator.Play("WeaponSelectOpen");
    }

    public void CloseDialog()
    {
      animator.Play("WeaponSelectClose");
    }

    public void EquipWeapon(int index)
    {
      if (EquipPrimary)
      {
        EquipmentSet.EquipPrimaryWeapon(_weapons[index]);
      }
      else if (EquipSecondary)
      {
        EquipmentSet.EquipSecondaryWeapon(_weapons[index]);
      }
      CloseDialog();
    }

  }
}