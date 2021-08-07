using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class HealthBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float health = 100f;
      public float maxHealth = 100f;

      [Header("Death Effect")]
      public GameObject deathEffect;
      public event Action<float, float> OnHealthChange;

      public override void InitBehaviour()
      {
        health = maxHealth;
      }

      public void SetHealth(float amount)
      {
        if (amount >= maxHealth)
        {
          amount = maxHealth;
        }
        health = amount;
        OnHealthChange?.Invoke(maxHealth, health);
        if (health <= 0)
        {
          // _character.Kill();
          if (deathEffect != null)
          {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
          }
        }
      }

      public void AddHealth(float amount)
      {
        SetHealth(health += amount);
      }

      public void TakeDamage(float damage)
      {
        SetHealth(health - damage);
      }

      public void Reset()
      {
        SetHealth(maxHealth);
      }

      public void SetMaxHealth(float amount)
      {
        maxHealth = amount;
      }

    }
  }
}