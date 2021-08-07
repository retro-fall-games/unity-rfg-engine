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

    private void Awake()
    {
      foreach (Image weaponImage in weaponImages)
      {
        weaponImage.sprite = null;
        weaponImage.transform.parent.gameObject.SetActive(false);
      }
      EquipPrimary = true;
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
      animator.Play("WeaponSelectOpen");
    }

    public void CloseDialog()
    {
      animator.Play("WeaponSelectClose");
    }

    public void EquipWeapon(int index)
    {
      Character character = null; // PlatformerLevelManager.Instance.PlayerCharacter;
      // WeaponBehaviour weaponBehavior = character.FindBehaviour<WeaponBehaviour>();
      // if (EquipPrimary)
      // {
      //   weaponBehavior.EquipPrimary(index);
      // }
      // else if (EquipSecondary)
      // {
      //   weaponBehavior.EquipSecondary(index);
      // }
      CloseDialog();
    }

    // public void OnEvent(WeaponPickupEvent weaponPickupEvent)
    // {
    //   for (int i = 0; i < weaponPickupEvent.weapons.Count; i++)
    //   {
    //     weaponImages[i].sprite = weaponPickupEvent.weapons[i].pickupSprite;
    //     weaponImages[i].color = Color.white;
    //     weaponImages[i].transform.parent.gameObject.SetActive(true);
    //   }
    // }


  }
}