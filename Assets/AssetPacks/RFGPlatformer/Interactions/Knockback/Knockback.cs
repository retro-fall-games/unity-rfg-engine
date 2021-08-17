using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Interactions/Knockback")]
    public class Knockback : MonoBehaviour
    {
      public KnockbackData KnockbackData;

      public Vector2 GetKnockbackVelocity(Vector2 target1, Vector2 target2)
      {
        Vector2 dir = (target1 - target2).normalized;
        return dir * KnockbackData.Velocity;
      }

      private void OnTriggerEnter2D(Collider2D col)
      {
        if (KnockbackData.LayerMask.Contains(col.gameObject.layer))
        {
          transform.SpawnFromPool("Effects", KnockbackData.Effects);
          if (!KnockbackData.Velocity.Equals(Vector2.zero))
          {
            IMoveable moveable = col.transform.gameObject.GetComponent(typeof(IMoveable)) as IMoveable;
            if (moveable != null)
            {
              Vector2 velocity = GetKnockbackVelocity(col.transform.position, transform.position);
              moveable.SetForce(velocity);
            }
          }
          if (KnockbackData.Damage > 0f)
          {
            CharacterBehaviourController controller = col.gameObject.GetComponent<CharacterBehaviourController>();
            if (controller != null)
            {
              HealthBehaviour health = controller.FindBehavior<HealthBehaviour>();
              if (health != null)
              {
                health.TakeDamage(KnockbackData.Damage);
              }
            }
          }
        }
      }
    }
  }
}