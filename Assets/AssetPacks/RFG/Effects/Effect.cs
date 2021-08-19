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
    private Animator _animator;
    private float _timeElapsed = 0f;

    private void Awake()
    {
      _audioSources = GetComponents<AudioSource>();
      _animator = GetComponent<Animator>();
    }

    public void OnObjectSpawn(params object[] objects)
    {
      _timeElapsed = 0;
      if (_audioSources != null)
      {
        _audioSources.PlayAll();
      }
      if (_animator != null && EffectData.AnimationClip != null)
      {
        _animator.Play(EffectData.AnimationClip);
      }
      transform.SpawnFromPool("Effects", EffectData.SpawnEffects, objects);

      if (EffectData.CameraShakeIntensity > 0)
      {
        CinemachineShake.Instance.ShakeCamera(EffectData.CameraShakeIntensity, EffectData.CameraShakeTime, EffectData.CameraShakeFade);
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
      foreach (SoundData sound in EffectData.SoundEffects)
      {
        sound.GenerateAudioSource(gameObject);
      }
      gameObject.tag = "Effect";
    }
#endif

  }
}