using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public class Inventory : MonoBehaviour
  {
    [Header("Settings")]
    public int MaxItems = 10;
    public Dictionary<string, Item> Items { get; private set; }
    public Action<Item> OnAdd;
    public Action<Item> OnRemove;

    private void Awake()
    {
      Items = new Dictionary<string, Item>();
    }

    public void Add(Item item)
    {
      bool didPickup = item.PickUp(this);
      if (didPickup)
      {
        // TODO - this needs to work with stackables 
        // TODO - this needs to alert that the inventory is maxed out
        if (!Items.ContainsKey(item.Id) && Items.Count < MaxItems)
        {
          Items.Add(item.Id, item);
          OnAdd?.Invoke(item);
        }
      }
    }

    public void Remove(Item item)
    {
      if (Items.ContainsKey(item.Id))
      {
        Items.Remove(item.Id);
        OnRemove?.Invoke(item);
      }
    }

    public bool InInventory(string id)
    {
      return Items.ContainsKey(id);
    }

    public void Consume(string id)
    {
      if (Items.ContainsKey(id))
      {
        Consumable item = (Consumable)Items[id];
        item.Consume(this);
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

    public List<T> FindAll<T>() where T : Item
    {
      Type AbilityType = typeof(T);
      List<T> list = new List<T>();

      foreach (KeyValuePair<string, Item> keyValuePair in Items)
      {
        if (keyValuePair.Value is T item)
        {
          list.Add(item);
        }
      }
      return list;
    }

  }
}