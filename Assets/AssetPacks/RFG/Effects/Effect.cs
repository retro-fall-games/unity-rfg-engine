using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG/Effects/Effect")]
  public class Effect : MonoBehaviour, IPooledObject
  {
    public EffectData EffectData;

    [HideInInspector]
    private AudioSource[] _audioSources;
    private float _timeElapsed = 0f;

    private void Awake()
    {
      _audioSources = GetComponents<AudioSource>();
    }

    public void OnObjectSpawn(params object[] objects)
    {
      _timeElapsed = 0;
      _audioSources.PlayAll();
      if (EffectData.ObjectsToSpawn.Length > 0)
      {
        foreach (string fx in EffectData.ObjectsToSpawn)
        {
          ObjectPool.Instance.SpawnFromPool(fx, transform.position, Quaternion.identity, null, false, objects);
        }
      }
    }

    private void Update()
    {
      _timeElapsed += Time.deltaTime;
      if (_timeElapsed >= EffectData.Lifetime)
      {
        gameObject.SetActive(false);
      }
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void GenerateEffect()
    {
      foreach (SoundData sound in EffectData.SoundFx)
      {
        sound.GenerateAudioSource(gameObject);
      }
    }
#endif

  }
}