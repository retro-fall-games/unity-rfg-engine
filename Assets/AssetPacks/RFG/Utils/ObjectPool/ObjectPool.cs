using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Utils/Object Pool")]
  public class ObjectPool : Singleton<ObjectPool>
  {
    [System.Serializable]
    public class Pool
    {
      public string tag;
      public GameObject prefab;
      public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
      poolDictionary = new Dictionary<string, Queue<GameObject>>();

      foreach (Pool pool in pools)
      {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
          GameObject obj = Instantiate(pool.prefab);
          obj.SetActive(false);
          objectPool.Enqueue(obj);
        }

        poolDictionary.Add(pool.tag, objectPool);
      }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = false)
    {
      if (!poolDictionary.ContainsKey(tag))
      {
        LogExt.Warn<ObjectPool>($"Pool with tag {tag} does not exist");
        return null;
      }
      GameObject objectToSpawn = poolDictionary[tag].Dequeue();
      objectToSpawn.SetActive(true);
      objectToSpawn.transform.position = position;
      objectToSpawn.transform.rotation = rotation;
      objectToSpawn.transform.SetParent(parent, worldPositionStays);

      IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

      if (pooledObj != null)
      {
        pooledObj.OnObjectSpawn();
      }

      poolDictionary[tag].Enqueue(objectToSpawn);

      return objectToSpawn;
    }

  }
}