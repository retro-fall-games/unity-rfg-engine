using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public class Inventory : MonoBehaviour
  {
    public Dictionary<string, Item> Items { get; private set; }

    private void Awake()
    {
      Items = new Dictionary<string, Item>();
    }

    public void Add(Item item)
    {
      bool didPickup = item.OnPickUp(this);
      if (didPickup)
      {
        Items.Add(item.Id, item);
      }
    }

    public void Use(string id)
    {
      if (Items.ContainsKey(id))
      {
        Consumable item = (Consumable)Items[id];
        item.Use(this);
      }
    }

    public void Equip(string id)
    {
      if (Items.ContainsKey(id))
      {
        Equipable item = (Equipable)Items[id];
        item.Equip(this);
      }
    }

    public void Unequip(string id)
    {
      if (Items.ContainsKey(id))
      {
        Equipable item = (Equipable)Items[id];
        item.Unequip(this);
      }
    }

  }
}