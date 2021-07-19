using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Character Spawner")]
  public class CharacterSpawner : MonoBehaviour
  {
    [Header("Prefabs")]
    public Character character;

    [Header("Controls")]
    public float spawnSpeed = 0f;
    public int spawnLimit = 10;
    public bool separately = false;

    [Header("Aggro")]
    public Aggro aggro;

    [HideInInspector]
    private float _spawnTimeElapsed = 0f;

    private int _spawnCount = 0;
    private bool _canSpawn = false;
    private Character _currentInstance = null;

    private void Awake()
    {
      aggro = GetComponent<Aggro>();
    }

    private void Update()
    {
      if (_spawnCount >= spawnLimit)
      {
        return;
      }

      if (!_canSpawn)
      {
        _spawnTimeElapsed += Time.deltaTime;
        if (_spawnTimeElapsed >= spawnSpeed)
        {
          _spawnTimeElapsed = 0;
          _canSpawn = true;

        }
      }
      if (aggro.HasAggro && _canSpawn && (separately && _currentInstance == null))
      {
        _canSpawn = false;
        Spawn();
      }
    }

    private void Spawn()
    {
      _currentInstance = Instantiate(character, transform.position, Quaternion.identity);
      _spawnCount++;
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void AddAggro()
    {
      this.aggro = gameObject.AddComponent<Aggro>();
      this.aggro.target1 = transform;
    }
#endif
  }
}