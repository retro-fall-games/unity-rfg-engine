using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{

  [AddComponentMenu("RFG/Game/Guid Manager")]
  public class GuidManager : Singleton<GuidManager>
  {
    [Serializable]
    public class GuidItem
    {
      public string Guid;
      public ScriptableObject Item;
    }

    public List<GuidItem> Items;

    public Dictionary<string, ScriptableObject> HashTable;

    protected override void Awake()
    {
      base.Awake();
      HashTable = new Dictionary<string, ScriptableObject>();
      foreach (GuidItem item in Items)
      {
        HashTable.Add(item.Guid, item.Item);
      }
    }

  }

  public static class GuidManagerEx
  {
    public static ScriptableObject FindObject(this string guid)
    {
      foreach (KeyValuePair<string, ScriptableObject> keyValuePair in GuidManager.Instance.HashTable)
      {
        if (keyValuePair.Key.Equals(guid))
        {
          return keyValuePair.Value;
        }
      }
      return null;
    }

    public static List<T> FindObjects<T>(this string[] guids) where T : ScriptableObject
    {
      List<T> objs = new List<T>();
      foreach (string guid in guids)
      {
        T value = (T)guid.FindObject();
        if (value != null)
        {
          objs.Add(value);
        }
      }
      return objs;
    }

    public static string FindGuid(this ScriptableObject obj)
    {
      foreach (KeyValuePair<string, ScriptableObject> keyValuePair in GuidManager.Instance.HashTable)
      {
        if (keyValuePair.Value.Equals(obj))
        {
          return keyValuePair.Key;
        }
      }
      return null;
    }

    public static string[] FindGuids<T>(this List<T> objs) where T : ScriptableObject
    {
      List<string> guids = new List<string>();
      foreach (T obj in objs)
      {
        string guid = obj.FindGuid();
        if (guid != null)
        {
          guids.Add(guid);
        }
      }
      return guids.ToArray();
    }
  }
}
