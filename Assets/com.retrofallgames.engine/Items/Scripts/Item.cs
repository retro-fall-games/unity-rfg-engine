using System;
using UnityEngine;
using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RFG.Items
{
  using Core;

  [Serializable]
  public class ItemSave
  {
    public string Guid;
  }

  public abstract class Item : ScriptableObject, IItem
  {
    [Header("Settings")]
    public string Guid;
    public string Description;

    [Header("Pick Up")]
    public Sprite PickUpSprite;
    public string PickUpText;
    public string[] PickUpEffects;

    public Action<Inventory> OnPickUp;

    public virtual bool PickUp(Inventory inventory, bool showEffects = true)
    {
      if (showEffects)
      {
        inventory.transform.SpawnFromPool("Effects", PickUpEffects, Quaternion.identity, new object[] { PickUpText });
      }
      OnPickUp?.Invoke(inventory);
      return true;
    }

    public ItemSave GetSave()
    {
      ItemSave save = new ItemSave();
      save.Guid = Guid;
      return save;
    }

#if UNITY_EDITOR
    [ButtonMethod]
    protected void GenerateGuid()
    {
      if (Guid == null || Guid.Equals(""))
      {
        Guid = System.Guid.NewGuid().ToString();
        EditorUtility.SetDirty(this);
      }
    }
#endif
  }
}