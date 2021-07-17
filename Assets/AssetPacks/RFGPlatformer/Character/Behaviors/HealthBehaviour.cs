using UnityEngine;
using UnityEngine.UI;

namespace RFG
{
  public class HealthBehaviour : CharacterBehaviour
  {
    [Header("Settings")]
    public float health = 100f;
    public float maxHealth = 100f;
    [Header("UI Healthbar Slider")]
    public Slider slider;
    public event System.Action OnKill;

    public override void InitBehaviour()
    {
      health = maxHealth;
      if (slider != null)
      {
        slider.maxValue = health;
        slider.value = health;
      }
    }

    public void SetHealth(float amount)
    {
      health = amount;
      if (health >= maxHealth)
      {
        health = maxHealth;
      }
      else if (health <= 0)
      {
        OnKill?.Invoke();
      }
      if (slider != null)
      {
        slider.value = health;
      }
    }

    public void AddHealth(float amount)
    {
      SetHealth(health += amount);
    }

    public void TakeDamage(float damage, Vector2 velocity)
    {
      SetHealth(health - damage);
      if (velocity != null)
      {
        _character.Controller.AddForce(velocity);
      }
    }

    public void Reset()
    {
      SetHealth(maxHealth);
    }

  }
}