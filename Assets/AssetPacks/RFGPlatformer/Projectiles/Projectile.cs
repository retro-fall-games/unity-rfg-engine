using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Projectiles/Projectile")]
  public class Projectile : MonoBehaviour, IPooledObject
  {
    [Header("Settings")]
    public float speed = 5f;
    public float damage = 10f;
    public bool destroyOnCollision = false;
    public Knockback knockback;

    [Header("Target")]
    public Transform target;
    public bool targetIsPlayer = false;

    [Header("Layer Mask")]
    public LayerMask layerMask;

    [Header("Effects")]
    public GameObject collisionEffect;
    public float cameraShakeIntensity = 0f;
    public float cameraShakeTime = 0f;
    public bool cameraShakeFade = false;

    [HideInInspector]
    private Rigidbody2D rb;

    private void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
    }

    public void OnObjectSpawn()
    {
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
        if (cameraShakeIntensity > 0)
        {
          CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime, cameraShakeFade);
        }

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
        if (destroyOnCollision)
        {
          Destroy(gameObject);
        }
        else
        {
          gameObject.SetActive(false);
        }
      }
    }

  }
}