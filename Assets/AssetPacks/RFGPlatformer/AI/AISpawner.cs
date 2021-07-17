using UnityEngine;

namespace RFG
{
  public class AISpawner : MonoBehaviour
  {
    [Header("Prefabs")]
    public AI aiPrefab;

    [Header("Controls")]
    public float spawnSpeed = 0f;
    public int spawnLimit = 10;
    public bool separately = false;

    [HideInInspector]
    private float _spawnTimeElapsed = 0f;
    private Aggro _aggro;
    private int _spawnCount = 0;
    private bool _canSpawn = false;
    private AI _currentInstance = null;

    private void Awake()
    {
      _aggro = GetComponent<Aggro>();
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
      if (_aggro.HasAggro && _canSpawn && (separately && _currentInstance == null))
      {
        _canSpawn = false;
        Spawn();
      }
    }

    private void Spawn()
    {
      _currentInstance = Instantiate(aiPrefab, transform.position, Quaternion.identity);
      _spawnCount++;
    }
  }
}