using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Character Ability Contoller")]
    public class CharacterAbilityController : MonoBehaviour
    {
      public List<CharacterAbility> Abilities;

      private Character _character;
      private CharacterInputController _input;

      private void Awake()
      {
        _character = GetComponent<Character>();
        _input = GetComponent<CharacterInputController>();
        if (Abilities == null)
        {
          Abilities = new List<CharacterAbility>();
        }
      }

      private void Start()
      {
        InitAbilities();
      }

      public void Process()
      {
        EarlyProcessAbilities();
        if (Time.timeScale != 0f)
        {
          ProcessAbilities();
          LateProcessAbilities();
        }
      }

      private void InitAbilities()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          ability.Init(_character);
        }
      }

      private void EarlyProcessAbilities()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          ability.EarlyProcess();
        }
      }

      private void ProcessAbilities()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          ability.Process();
        }
      }

      private void LateProcessAbilities()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          ability.LateProcess();
        }
      }

      private void OnEnable()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          switch (ability.Input)
          {
            case CharacterAbility.InputMethod.Movement:
              break;
            case CharacterAbility.InputMethod.Jump:
              _input.InputActions.PlayerControls.Jump.started += ability.OnButtonStarted;
              _input.InputActions.PlayerControls.Jump.canceled += ability.OnButtonCanceled;
              _input.InputActions.PlayerControls.Jump.performed += ability.OnButtonPerformed;
              break;
            case CharacterAbility.InputMethod.Pause:
              _input.InputActions.PlayerControls.Pause.started += ability.OnButtonStarted;
              _input.InputActions.PlayerControls.Pause.canceled += ability.OnButtonCanceled;
              _input.InputActions.PlayerControls.Pause.performed += ability.OnButtonPerformed;
              break;
            case CharacterAbility.InputMethod.Dash:
              _input.InputActions.PlayerControls.Dash.started += ability.OnButtonStarted;
              _input.InputActions.PlayerControls.Dash.canceled += ability.OnButtonCanceled;
              _input.InputActions.PlayerControls.Dash.performed += ability.OnButtonPerformed;
              break;
          }
        }
      }

      private void OnDisable()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          switch (ability.Input)
          {
            case CharacterAbility.InputMethod.Movement:
              break;
            case CharacterAbility.InputMethod.Jump:
              _input.InputActions.PlayerControls.Jump.started -= ability.OnButtonStarted;
              _input.InputActions.PlayerControls.Jump.canceled -= ability.OnButtonCanceled;
              _input.InputActions.PlayerControls.Jump.performed -= ability.OnButtonPerformed;
              break;
            case CharacterAbility.InputMethod.Pause:
              _input.InputActions.PlayerControls.Pause.started -= ability.OnButtonStarted;
              _input.InputActions.PlayerControls.Pause.canceled -= ability.OnButtonCanceled;
              _input.InputActions.PlayerControls.Pause.performed -= ability.OnButtonPerformed;
              break;
            case CharacterAbility.InputMethod.Dash:
              _input.InputActions.PlayerControls.Dash.started -= ability.OnButtonStarted;
              _input.InputActions.PlayerControls.Dash.canceled -= ability.OnButtonCanceled;
              _input.InputActions.PlayerControls.Dash.performed -= ability.OnButtonPerformed;
              break;
          }
        }
      }

    }
  }
}