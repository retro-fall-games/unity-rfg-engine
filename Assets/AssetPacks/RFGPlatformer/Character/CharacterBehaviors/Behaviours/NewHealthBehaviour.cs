using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Health Character Behaviour", menuName = "RFG/Platformer/Character/Character Behaviour/Health")]
    public class NewHealthBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float Health = 100f;
      public float MaxHealth = 100f;
      public event Action<float, float> OnHealthChange;
      public event Action OnHealthZero;

      public override void Init(CharacterBehaviourController.BehaviourContext ctx)
      {
        OnHealthZero += ctx.character.Kill;
      }

      public override void InitValues(CharacterBehaviour behaviour)
      {
        NewHealthBehaviour health = (NewHealthBehaviour)behaviour;
        Health = health.Health;
        MaxHealth = health.MaxHealth;
      }

      public override void Remove(CharacterBehaviourController.BehaviourContext ctx)
      {
        OnHealthZero -= ctx.character.Kill;
      }

      public void SetHealth(float amount)
      {
        if (amount >= MaxHealth)
        {
          amount = MaxHealth;
        }
        Health = amount;
        OnHealthChange?.Invoke(MaxHealth, Health);
        if (Health <= 0)
        {
          OnHealthZero?.Invoke();
        }
      }

      public void AddHealth(float amount)
      {
        SetHealth(Health += amount);
      }

      public void TakeDamage(float damage)
      {
        SetHealth(Health - damage);
      }

      public void Reset()
      {
        SetHealth(MaxHealth);
      }

      public void SetMaxHealth(float amount)
      {
        MaxHealth = amount;
      }
    }
  }
}