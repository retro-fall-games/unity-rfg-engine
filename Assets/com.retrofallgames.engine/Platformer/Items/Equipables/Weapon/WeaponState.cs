using System;
using UnityEngine;

namespace RFG.Platformer
{
  using Core;

  public abstract class WeaponState : ScriptableObject
  {
    public string[] EnterEffects;
    public string[] ExitEffects;

    public virtual void Enter(Weapon weapon)
    {
      if (EnterEffects.Length > 0)
      {
        foreach (string effect in EnterEffects)
        {
          ObjectPoolManager.Instance.SpawnFromPool("Effects", effect, weapon.FirePoint.position, Quaternion.identity, null, false);
        }
      }
    }

    public virtual Type Execute(Weapon weapon)
    {
      return null;
    }

    public virtual void Exit(Weapon weapon)
    {
      if (ExitEffects.Length > 0)
      {
        foreach (string effect in ExitEffects)
        {
          ObjectPoolManager.Instance.SpawnFromPool("Effects", effect, weapon.FirePoint.position, Quaternion.identity, null, false);
        }
      }
    }
  }
}