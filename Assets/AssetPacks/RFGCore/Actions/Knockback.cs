using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Actions/Knockback")]
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

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (layerMask.Contains(col.gameObject.layer))
      {
        PlayFX();
        if (!velocity.Equals(Vector2.zero))
        {
          IMoveable moveable = col.transform.gameObject.GetComponent(typeof(IMoveable)) as IMoveable;
          if (moveable != null)
          {
            Vector2 velocity = GetKnockbackVelocity(col.transform.position, transform.position);
            moveable.SetForce(velocity);
          }
        }
        if (damage > 0f)
        {
          HealthBehaviour health = col.gameObject.GetComponent<HealthBehaviour>();
          if (health != null)
          {
            health.TakeDamage(damage);
          }
        }
      }
    }
  }
}
