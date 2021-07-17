using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Actions/Knockback")]
  public class Knockback : MonoBehaviour
  {
    [Header("Settings")]
    public float damage;
    public Vector2 velocity;
    public Vector2 GetKnockbackVelocity(Vector2 target1, Vector2 target2)
    {
      Vector2 dir = (target1 - target2).normalized;
      return dir * velocity;
    }
  }
}
