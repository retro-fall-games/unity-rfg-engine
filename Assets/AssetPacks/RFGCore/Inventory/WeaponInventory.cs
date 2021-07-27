using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public class WeaponInventory : MonoBehaviour
  {
    private List<WeaponItem> _weapons;


    private void Awake()
    {
      _weapons = new List<WeaponItem>();
    }

    public List<WeaponItem> GetAll()
    {
      return _weapons;
    }

    public void Add(WeaponItem weapon)
    {
      // If the weapon isn't in the inventory then add it
      if (!_weapons.Contains(weapon))
      {
        _weapons.Add(weapon);
      }
      else
      {
        // If it is in the inventory then maybe add the ammo from the picked up one to the one in inventory
      }
    }

    public void Remove(WeaponItem weapon)
    {
      _weapons.Remove(weapon);
    }

    public int IndexOf(WeaponItem weapon)
    {
      return _weapons.IndexOf(weapon);
    }

  }
}