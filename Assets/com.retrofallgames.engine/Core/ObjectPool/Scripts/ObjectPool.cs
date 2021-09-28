using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace RFG.Core
{
  using Utils;

  [AddComponentMenu("RFG/Core/Object Pool")]
  public class ObjectPool : MonoBehaviour
  {
    [System.Serializable]
    public class Pool
    {
      public string tag;
      public GameObject prefab;
      public int size;
    }

    public class QueueObject
    {
      public GameObject gameObject;
      public IPooledObject pooledObject;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<QueueObject>> poolDictionary;

    private void Start()
    {
      poolDictionary = new Dictionary<string, Queue<QueueObject>>();

      // Create all the pools
      foreach (Pool pool in pools)
      {
        Queue<QueueObject> objectPool = new Queue<QueueObject>();
        poolDictionary.Add(pool.tag, objectPool);
      }

      // Go through each child and add them to the correct pool
      foreach (Transform child in transform)
      {
        QueueObject queueObject = new QueueObject();
        queueObject.gameObject = child.gameObject;
        queueObject.gameObject.SetActive(false);
        queueObject.pooledObject = child.gameObject.GetComponent<IPooledObject>();
        poolDictionary[child.gameObject.name].Enqueue(queueObject);
      }

    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = false, params object[] objects)
    {
      if (!poolDictionary.ContainsKey(tag))
      {
        LogExt.Warn<ObjectPool>($"Pool with tag {tag} does not exist");
        return null;
      }
      QueueObject queueObject = poolDictionary[tag].Dequeue();
      GameObject objectToSpawn = queueObject.gameObject;
      objectToSpawn.SetActive(true);
      objectToSpawn.transform.position = position;
      objectToSpawn.transform.rotation = rotation;
      objectToSpawn.transform.SetParent(parent, worldPositionStays);

      IPooledObject pooledObj = queueObject.pooledObject;

      if (pooledObj != null)
      {
        pooledObj.OnObjectSpawn(objects);
      }

      poolDictionary[tag].Enqueue(queueObject);

      return objectToSpawn;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, params object[] objects)
    {
      if (!poolDictionary.ContainsKey(tag))
      {
        LogExt.Warn<ObjectPool>($"Pool with tag {tag} does not exist");
        return null;
      }
      QueueObject queueObject = poolDictionary[tag].Dequeue();
      GameObject objectToSpawn = queueObject.gameObject;
      objectToSpawn.SetActive(true);
      objectToSpawn.transform.position = position;
      objectToSpawn.transform.rotation = rotation;

      IPooledObject pooledObj = queueObject.pooledObject;

      if (pooledObj != null)
      {
        pooledObj.OnObjectSpawn(objects);
      }

      poolDictionary[tag].Enqueue(queueObject);

      return objectToSpawn;
    }

    public void DeactivateAllByTag(string tag)
    {
      if (!poolDictionary.ContainsKey(tag))
      {
        LogExt.Warn<ObjectPool>($"Pool with tag {tag} does not exist");
        return;
      }
      foreach (QueueObject queueObject in poolDictionary[tag])
      {
        GameObject objectToSpawn = queueObject.gameObject;
        objectToSpawn.SetActive(false);
      }
    }

#if UNITY_EDITOR
    [ButtonMethod]
    public void GenerateObjects()
    {
      for (int i = transform.childCount - 1; i >= 0; --i)
      {
        var child = transform.GetChild(i).gameObject;
        DestroyImmediate(child);
      }

      foreach (ObjectPool.Pool pool in pools)
      {
        LogExt.Log<ObjectPool>($"Generating pool: {pool.tag}");
        for (int i = 0; i < pool.size; i++)
        {
          GameObject obj = Instantiate(pool.prefab);
          obj.name = pool.tag;
          obj.SetActive(false);
          obj.gameObject.transform.SetParent(gameObject.transform);
        }
      }
    }
#endif

  }
}