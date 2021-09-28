using System.Collections;
using UnityEngine;

namespace RFG.Effects
{
  using Core;

  [AddComponentMenu("RFG/Effects/Effect Trigger")]
  public class EffectTrigger : MonoBehaviour
  {
    [Header("Transforms")]
    [Tooltip("Spawn effects at these transform locations")]
    public Transform[] Transforms;
    public float WaitTimeInBetween = 0f;

    [Header("Effects")]
    public string[] Effects;

    [HideInInspector]
    private Transform _transform;

    private void Awake()
    {
      _transform = transform;
    }

    public void SpawnEffects()
    {
      if (Transforms.Length > 0)
      {
        StartCoroutine(SpawnEffectsCo());
      }
    }

    private IEnumerator SpawnEffectsCo()
    {
      for (int i = 0; i < Transforms.Length; i++)
      {
        yield return new WaitForSeconds(WaitTimeInBetween);
        Transform currentTransform = Transforms[i];
        currentTransform.SpawnFromPool("Effects", Effects);
      }
    }

    public void StopEffects()
    {
      _transform.transform.DeactivatePoolByTag("Effects", Effects);
    }
  }
}

