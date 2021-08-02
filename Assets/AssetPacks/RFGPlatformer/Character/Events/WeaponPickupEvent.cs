using System.Collections.Generic;

namespace RFG
{
  public struct WeaponPickupEvent
  {
    public List<WeaponItem> weapons;
    public WeaponPickupEvent(List<WeaponItem> weapons)
    {
      this.weapons = weapons;
    }
  }
}