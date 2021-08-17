
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Character Ability Contoller")]
    public class CharacterAbilityController : MonoBehaviour
    {
      public class AbilityEvents
      {
        public Action<InputAction.CallbackContext> started;
        public Action<InputAction.CallbackContext> canceled;
        public Action<InputAction.CallbackContext> performed;
      }

      public class AbilityContext
      {
        public CharacterAbilityController controller;
        public Transform transform;
        public Character character;
        public CharacterInputController input;
        public EquipmentSet equipmentSet;
      }

      [Header("Settings")]
      public List<CharacterAbility> Abilities;

      [HideInInspector]
      private AbilityContext _abilityContext;
      private Character _character;
      private CharacterInputController _input;
      private Dictionary<CharacterAbility, AbilityEvents> _abilityEvents = new Dictionary<CharacterAbility, AbilityEvents>();

      private void Awake()
      {
        _abilityContext = new AbilityContext();
        _abilityContext.controller = this;
        _abilityContext.transform = transform;
        _abilityContext.character = GetComponent<Character>();
        _abilityContext.input = GetComponent<CharacterInputController>();
        _abilityContext.equipmentSet = GetComponent<EquipmentSet>();
        if (Abilities == null)
        {
          Abilities = new List<CharacterAbility>();
        }
      }

      private void Start()
      {
        InitAbilities();
      }

      public void AddAbility(CharacterAbility ability)
      {
        EnableInputEvents(ability);
        ability.Init(_abilityContext);
        Abilities.Add(ability);
      }

      public void RemoveAbility(CharacterAbility ability)
      {
        DisableInputEvents(ability);
        ability.Remove(_abilityContext);
        Abilities.Remove(ability);
      }

      public T FindAbility<T>() where T : CharacterAbility
      {
        Type AbilityType = typeof(T);

        foreach (CharacterAbility ability in Abilities)
        {
          if (ability is T characterAbility)
          {
            return characterAbility;
          }
        }
        return null;
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
          ability.Init(_abilityContext);
        }
      }

      private void EarlyProcessAbilities()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          ability.EarlyProcess(_abilityContext);
        }
      }

      private void ProcessAbilities()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          ability.Process(_abilityContext);
        }
      }

      private void LateProcessAbilities()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          ability.LateProcess(_abilityContext);
        }
      }

      private void EnableInputEvents(CharacterAbility ability)
      {
        AbilityEvents abilityEvents;
        if (!_abilityEvents.ContainsKey(ability))
        {
          abilityEvents = new AbilityEvents();
          abilityEvents.started = (InputAction.CallbackContext ctx) => ability.OnButtonStarted(ctx, _abilityContext);
          abilityEvents.canceled = (InputAction.CallbackContext ctx) => ability.OnButtonCanceled(ctx, _abilityContext);
          abilityEvents.performed = (InputAction.CallbackContext ctx) => ability.OnButtonPerformed(ctx, _abilityContext);
          _abilityEvents.Add(ability, abilityEvents);
        }
        else
        {
          abilityEvents = _abilityEvents[ability];
        }

        InputAction input = null;

        switch (ability.Input)
        {
          case CharacterAbility.InputMethod.Jump:
            input = _abilityContext.input.InputActions.PlayerControls.Jump;
            break;
          case CharacterAbility.InputMethod.Pause:
            input = _abilityContext.input.InputActions.PlayerControls.Pause;
            break;
          case CharacterAbility.InputMethod.Dash:
            input = _abilityContext.input.InputActions.PlayerControls.Dash;
            break;
          case CharacterAbility.InputMethod.PrimaryAttack:
            input = _abilityContext.input.InputActions.PlayerControls.PrimaryAttack;
            break;
          case CharacterAbility.InputMethod.SecondaryAttack:
            input = _abilityContext.input.InputActions.PlayerControls.SecondaryAttack;
            break;
        }

        if (input != null)
        {
          input.started += abilityEvents.started;
          input.canceled += abilityEvents.canceled;
          input.performed += abilityEvents.performed;
        }
      }

      private void DisableInputEvents(CharacterAbility ability)
      {
        AbilityEvents abilityEvents;
        if (!_abilityEvents.ContainsKey(ability))
        {
          return;
        }
        else
        {
          abilityEvents = _abilityEvents[ability];
        }

        InputAction input = null;

        switch (ability.Input)
        {
          case CharacterAbility.InputMethod.Jump:
            input = _abilityContext.input.InputActions.PlayerControls.Jump;
            break;
          case CharacterAbility.InputMethod.Pause:
            input = _abilityContext.input.InputActions.PlayerControls.Pause;
            break;
          case CharacterAbility.InputMethod.Dash:
            input = _abilityContext.input.InputActions.PlayerControls.Dash;
            break;
          case CharacterAbility.InputMethod.PrimaryAttack:
            input = _abilityContext.input.InputActions.PlayerControls.PrimaryAttack;
            break;
          case CharacterAbility.InputMethod.SecondaryAttack:
            input = _abilityContext.input.InputActions.PlayerControls.SecondaryAttack;
            break;
        }

        if (input != null)
        {
          input.started -= abilityEvents.started;
          input.canceled -= abilityEvents.canceled;
          input.performed -= abilityEvents.performed;
        }

        _abilityEvents.Remove(ability);
      }

      private void OnEnable()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          EnableInputEvents(ability);
        }
      }

      private void OnDisable()
      {
        foreach (CharacterAbility ability in Abilities)
        {
          DisableInputEvents(ability);
        }
      }

    }
  }
}