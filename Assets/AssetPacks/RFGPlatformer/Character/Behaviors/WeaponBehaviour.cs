using UnityEngine;

namespace RFG
{
  public class WeaponBehaviour : CharacterBehaviour
  {
    public GameObject[] weapons;

    [HideInInspector]
    private int equippedWeaponIndex;
    private Weapon equippedWeapon;

    public void Equip(int index)
    {
      if (index >= weapons.Length || index < 0)
      {
        Debug.Log("Cannot equip weapon at index: " + index);
        return;
      }
      if (equippedWeapon != null)
      {
        equippedWeapon.Unequip();
      }
      equippedWeaponIndex = index;
      GameObject currentWeapon = weapons[equippedWeaponIndex];
      if (currentWeapon != null)
      {
        equippedWeapon = currentWeapon.GetComponent<Weapon>();
        equippedWeapon.Equip();
      }
    }

    public void Unequip()
    {
      if (equippedWeapon != null)
      {
        equippedWeapon.Unequip();
      }
    }

    public Weapon GetEquipped()
    {
      return equippedWeapon;
    }

  }
}