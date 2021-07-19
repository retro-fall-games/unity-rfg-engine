using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Projectiles/Projectile")]
  public class Projectile : MonoBehaviour
  {
    [Header("Settings")]
    public float speed = 5f;
    public float damage = 10f;
    public Knockback knockback;

    [Header("Target")]
    public Transform target;
    public bool targetIsPlayer = false;

    [Header("Layer Mask")]
    public LayerMask layerMask;

    [Header("Collision Effect")]
    public GameObject collisionEffect;

    [HideInInspector]
    private Rigidbody2D rb;

    private void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      if (targetIsPlayer)
      {
        StartCoroutine(WaitForPlayer());
      }
      else if (target != null)
      {
        CalculateVelocity(target.position - transform.position);
      }
      else
      {
        CalculateVelocity(transform.right);
      }
    }

    private IEnumerator WaitForPlayer()
    {
      yield return new WaitUntil(() => LevelManager.Instance != null);
      yield return new WaitUntil(() => LevelManager.Instance.PlayerCharacter != null);
      target = LevelManager.Instance.PlayerCharacter.transform;
      CalculateVelocity(target.position - transform.position);
    }

    private void CalculateVelocity(Vector3 velocity)
    {
      rb.velocity = velocity.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (layerMask.Contains(col.gameObject.layer))
      {
        HealthBehaviour health = col.gameObject.GetComponent<HealthBehaviour>();
        if (health != null)
        {
          Vector2 velocity = Vector2.zero;
          if (knockback != null)
          {
            velocity = knockback.GetKnockbackVelocity(col.transform.position, transform.position);
          }
          health.TakeDamage(damage, velocity);
        }
        if (collisionEffect != null)
        {
          Instantiate(collisionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
      }
    }

  }
}