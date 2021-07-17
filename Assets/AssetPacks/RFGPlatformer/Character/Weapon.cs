using UnityEngine;

namespace RFG
{
  public class Weapon : MonoBehaviour
  {
    public bool IsCharging { get; set; }
    public bool IsShooting { get; set; }
    public bool IsEquiped { get; set; }
    public bool IsEnabled { get; set; }

    private void Awake()
    {
      Unequip();
      IsEnabled = true;
    }

    public void Unequip()
    {
      IsCharging = false;
      IsShooting = false;
      IsEquiped = false;
    }
    public void Equip()
    {
      IsEquiped = true;
    }
  }

}