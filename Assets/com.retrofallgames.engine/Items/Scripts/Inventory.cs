using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Items
{
  using Core;

  [Serializable]
  public class InventorySave
  {
    public ItemSave[] Items;
  }

  [AddComponentMenu("RFG/Items/Inventory")]
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

    public void Add(Item item, bool showEffects = true)
    {
      bool didPickup = item.PickUp(this, showEffects);
      if (didPickup)
      {
        // TODO - this needs to work with stackables 
        // TODO - this needs to alert that the inventory is maxed out
        if (!Items.ContainsKey(item.Guid) && Items.Count < MaxItems)
        {
          Items.Add(item.Guid, item);
          OnAdd?.Invoke(item);
        }
      }
    }

    public void Remove(Item item)
    {
      if (Items.ContainsKey(item.Guid))
      {
        Items.Remove(item.Guid);
        OnRemove?.Invoke(item);
      }
    }

    public bool InInventory(string guid)
    {
      return Items.ContainsKey(guid);
    }

    public void Consume(string guid)
    {
      if (Items.ContainsKey(guid))
      {
        Consumable item = (Consumable)Items[guid];
        item.Consume(this);
      }
    }

    public void Equip(string guid)
    {
      if (Items.ContainsKey(guid))
      {
        Equipable item = (Equipable)Items[guid];
        item.Equip(this);
      }
    }

    public void Unequip(string guid)
    {
      if (Items.ContainsKey(guid))
      {
        Equipable item = (Equipable)Items[guid];
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

    public InventorySave GetSave()
    {
      InventorySave save = new InventorySave();
      List<ItemSave> itemsSaves = new List<ItemSave>();
      foreach (KeyValuePair<string, Item> keyValuePair in Items)
      {
        itemsSaves.Add(keyValuePair.Value.GetSave());
      }
      save.Items = itemsSaves.ToArray();
      return save;
    }

    public void RestoreSave(InventorySave save)
    {
      List<string> guids = new List<string>();
      foreach (ItemSave itemSave in save.Items)
      {
        guids.Add(itemSave.Guid);
      }
      List<Item> list = guids.ToArray().FindObjects<Item>();
      foreach (Item item in list)
      {
        Add(item, false);
      }
    }

  }
}