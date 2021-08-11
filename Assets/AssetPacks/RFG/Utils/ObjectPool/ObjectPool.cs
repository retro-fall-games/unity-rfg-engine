using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Utils/Object Pool")]
  public class ObjectPool : Singleton<ObjectPool>
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

      foreach (Pool pool in pools)
      {
        Queue<QueueObject> objectPool = new Queue<QueueObject>();

        for (int i = 0; i < pool.size; i++)
        {
          GameObject obj = Instantiate(pool.prefab);
          obj.SetActive(false);
          QueueObject queueObject = new QueueObject();
          queueObject.gameObject = obj;
          queueObject.pooledObject = obj.GetComponent<IPooledObject>();
          objectPool.Enqueue(queueObject);
        }

        poolDictionary.Add(pool.tag, objectPool);
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

  }
}