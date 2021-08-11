using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Primary Weapon Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Primary Weapon")]
    public class PrimaryWeaponAbility : CharacterAbility
    {
      public WeaponItem PrimaryWeapon { get; set; }

      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private InputAction _movement;
      private Vector2 _movementVector;

      public override void Init(Character character)
      {
        _character = character;
        _transform = character.transform;
        _controller = character.Controller;
        _movement = character.Input.Movement;
      }

      public override void EarlyProcess()
      {
      }

      public override void LateProcess()
      {
      }

      public override void OnButtonStarted(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonCanceled(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonPerformed(InputAction.CallbackContext ctx)
      {
      }

      public override void Process()
      {
        if (PrimaryWeapon != null)
        {
          if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Charging)
          {
            PrimaryWeapon.Charging();
          }
          else if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Charged)
          {
            PrimaryWeapon.Charged();
          }
          else if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Firing)
          {
            PrimaryWeapon.Firing();
          }
          else if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Fired)
          {
            PrimaryWeapon.Fired();
          }
        }
      }

    }

  }
}