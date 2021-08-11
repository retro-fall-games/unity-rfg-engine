using System.Collections;
using UnityEngine;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Engine/Projectiles/Projectile")]
    public class Projectile : MonoBehaviour, IPooledObject
    {
      [Header("Settings")]
      public float speed = 5f;
      public float damage = 10f;

      [Header("Target")]
      public Transform target;
      public bool targetIsPlayer = false;

      [Header("Layer Mask")]
      public LayerMask layerMask;

      [Header("Effects")]
      public Animator animator;
      public GameObject collisionEffect;
      public string collisionEffectPoolTag;
      public float cameraShakeIntensity = 0f;
      public float cameraShakeTime = 0f;
      public bool cameraShakeFade = false;

      [Header("Audio")]
      public string[] spawnSoundFx;
      public string[] deathSoundFx;

      [Header("Physics")]
      public Rigidbody2D rb;
      public BoxCollider2D boxCollider;
      public bool destroyOnCollision = false;
      public Knockback knockback;

      public void OnObjectSpawn(params object[] objects)
      {
        if (animator != null)
        {
          animator.ResetCurrentClip();
        }
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
        if (spawnSoundFx != null && spawnSoundFx.Length > 0)
        {
          // FXAudio.Instance.Play(spawnSoundFx, false);
        }
      }

      private IEnumerator WaitForPlayer()
      {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
          target = player.transform;
        }
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
            health.TakeDamage(damage);
          }

          // Play Audio
          if (deathSoundFx != null && deathSoundFx.Length > 0)
          {
            // FXAudio.Instance.Play(deathSoundFx, false);
          }

          // Shake the camera
          if (cameraShakeIntensity > 0)
          {
            CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime, cameraShakeFade);
          }

          // Effects
          if (collisionEffect != null)
          {
            Instantiate(collisionEffect, transform.position, Quaternion.identity);
          }
          else if (!collisionEffectPoolTag.Equals(""))
          {
            ObjectPool.Instance.SpawnFromPool(collisionEffectPoolTag, transform.position, Quaternion.identity);
          }

          // Destroy
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

#if UNITY_EDITOR
      [ButtonMethod]
      private void AutoConfigure()
      {
        if (GetComponent<Rigidbody2D>() == null)
        {
          this.rb = gameObject.AddComponent<Rigidbody2D>();
        }
        if (GetComponent<BoxCollider2D>() == null)
        {
          this.boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        if (GetComponent<Knockback>() == null)
        {
          this.knockback = gameObject.AddComponent<Knockback>();
        }
      }
#endif

    }
  }
}