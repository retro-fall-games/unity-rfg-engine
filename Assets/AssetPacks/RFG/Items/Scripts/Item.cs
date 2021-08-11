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
    public string[] PickUpFx;

    public virtual bool OnPickUp(Inventory inventory)
    {
      if (PickUpFx.Length > 0)
      {
        foreach (string fx in PickUpFx)
        {
          ObjectPool.Instance.SpawnFromPool(fx, inventory.transform.position, Quaternion.identity, null, false, new object[] { PickUpText });
        }
      }
      return true;
    }
  }
}