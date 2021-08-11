using UnityEngine;

namespace RFG
{
  public abstract class Equipable : Consumable, IEquipable, IStorable
  {
    [Header("Equipable Settings")]
    public bool EquipOnPickUp = false;
    public Sprite EquipSprite;
    public string EquipText;
    public SoundData[] EquipSoundFx;

    public override bool OnPickUp(Inventory inventory)
    {
      bool didPickup = base.OnPickUp(inventory);
      if (EquipOnPickUp)
      {
        Equip(inventory);
      }
      return didPickup;
    }

    public virtual void Equip(Inventory inventory)
    {
    }

    public virtual void Unequip(Inventory inventory)
    {
    }

  }
}