using UnityEngine;

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

    [HideInInspector]
    private float _spawnTimeElapsed = 0f;
    private Aggro _aggro;
    private int _spawnCount = 0;
    private bool _canSpawn = false;
    private Character _currentInstance = null;

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
      _currentInstance = Instantiate(character, transform.position, Quaternion.identity);
      _spawnCount++;
    }
  }
}