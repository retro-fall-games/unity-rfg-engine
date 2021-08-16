using System;
using UnityEngine;

namespace RFG
{
  public abstract class Item : ScriptableObject, IItem
  {
    [Header("Item Settings")]
    public string Id;
    public string Description;

    [Header("Pick Up")]
    public Sprite PickUpSprite;
    public string PickUpText;
    public string[] PickUpEffects;

    public Action<Inventory> OnPickUp;

    public virtual bool PickUp(Inventory inventory)
    {
      inventory.transform.SpawnFromPool("Effects", PickUpEffects, Quaternion.identity, new object[] { PickUpText });
      OnPickUp?.Invoke(inventory);
      return true;
    }
  }
}