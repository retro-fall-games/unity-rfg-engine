using UnityEngine;
using RFGFx;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Actions/Knockback")]
  public class Knockback : MonoBehaviour
  {
    [Header("Settings")]
    public float damage;
    public Vector2 velocity;
    public LayerMask layerMask;

    [Header("Audio")]
    public string[] soundFx;
    public Vector2 GetKnockbackVelocity(Vector2 target1, Vector2 target2)
    {
      Vector2 dir = (target1 - target2).normalized;
      return dir * velocity;
    }

    public void PlayFX()
    {
      if (soundFx != null && soundFx.Length > 0)
      {
        FXAudio.Instance.Play(soundFx, false);
      }
    }
  }
}
