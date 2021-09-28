using System.Collections.Generic;
using UnityEngine;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Object Pool Manager")]
  public class ObjectPoolManager : Singleton<ObjectPoolManager>
  {
    public List<string> ObjectPoolCategories;

    private Dictionary<string, ObjectPool> _objectPools;

    protected override void Awake()
    {
      base.Awake();
      _objectPools = new Dictionary<string, ObjectPool>();
      foreach (string category in ObjectPoolCategories)
      {
        Transform child = transform.Find(category);
        ObjectPool objectPool = child.GetComponent<ObjectPool>();
        if (objectPool != null)
        {
          _objectPools.Add(category, objectPool);
        }
      }
    }

    public GameObject SpawnFromPool(string category, string tag, Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = false, params object[] objects)
    {
      if (_objectPools.ContainsKey(category))
      {
        return _objectPools[category].SpawnFromPool(tag, position, rotation, parent, worldPositionStays, objects);
      }
      return null;
    }

    public GameObject SpawnFromPool(string category, string tag, Vector3 position, Quaternion rotation, params object[] objects)
    {
      if (_objectPools.ContainsKey(category))
      {
        return _objectPools[category].SpawnFromPool(tag, position, rotation, objects);
      }
      return null;
    }

    public void DeactivateAllByTag(string category, string tag)
    {
      if (_objectPools.ContainsKey(category))
      {
        _objectPools[category].DeactivateAllByTag(tag);
      }
    }
  }

  public static class TransformEx
  {
    public static List<GameObject> SpawnFromPool(this Transform transform, string category, string[] tags, params object[] objects)
    {
      if (tags != null && tags.Length > 0)
      {
        List<GameObject> spawnedObjects = new List<GameObject>();
        foreach (string tag in tags)
        {
          GameObject spawned = ObjectPoolManager.Instance.SpawnFromPool(category, tag, transform.position, transform.rotation, objects);
          spawnedObjects.Add(spawned);
        }
        return spawnedObjects;
      }
      return null;
    }

    public static List<GameObject> SpawnFromPool(this Transform transform, string category, string[] tags, Quaternion rotation, params object[] objects)
    {
      if (tags != null && tags.Length > 0)
      {
        List<GameObject> spawnedObjects = new List<GameObject>();
        foreach (string tag in tags)
        {
          GameObject spawned = ObjectPoolManager.Instance.SpawnFromPool(category, tag, transform.position, rotation, objects);
          spawnedObjects.Add(spawned);
        }
        return spawnedObjects;
      }
      return null;
    }

    public static void DeactivatePoolByTag(this Transform transform, string category, string[] tags)
    {
      if (tags != null && tags.Length > 0)
      {
        foreach (string tag in tags)
        {
          ObjectPoolManager.Instance.DeactivateAllByTag(category, tag);
        }
      }
    }
  }
}