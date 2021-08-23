using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Attack")]
    public class AttackAbility : MonoBehaviour
    {
      [Header("Input")]
      /// <summary>Input Action to initiate the Primary Attack</summary>
      [Tooltip("Input Action to initiate the Primary Attack")]
      public InputActionReference PrimaryAttackInput;

      /// <summary>Input Action to initiate the Secondary Attack</summary>
      [Tooltip("Input Action to initiate the Secondary Attack")]
      public InputActionReference SecondaryAttackInput;

      [HideInInspector]
      private EquipmentSet _equipmentSet;

      private void Awake()
      {
        _equipmentSet = GetComponent<EquipmentSet>();
      }


      public void OnPrimaryAttackStarted(InputAction.CallbackContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          _equipmentSet.PrimaryWeapon?.Started();
        }
      }

      public void OnPrimaryAttackCanceled(InputAction.CallbackContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          _equipmentSet.PrimaryWeapon?.Cancel();
        }
      }

      public void OnPrimaryAttackPerformed(InputAction.CallbackContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          _equipmentSet.PrimaryWeapon?.Perform();
        }
      }

      public void OnSecondaryAttackStarted(InputAction.CallbackContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          _equipmentSet.SecondaryWeapon?.Started();
        }
      }

      public void OnSecondaryAttackCanceled(InputAction.CallbackContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          _equipmentSet.SecondaryWeapon?.Cancel();
        }
      }

      public void OnSecondaryAttackPerformed(InputAction.CallbackContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          _equipmentSet.SecondaryWeapon?.Perform();
        }
      }

      private void OnEnable()
      {
        PrimaryAttackInput.action.Enable();
        PrimaryAttackInput.action.started += OnPrimaryAttackStarted;
        PrimaryAttackInput.action.canceled += OnPrimaryAttackStarted;
        PrimaryAttackInput.action.performed += OnPrimaryAttackStarted;
        SecondaryAttackInput.action.Enable();
        SecondaryAttackInput.action.started += OnSecondaryAttackStarted;
        SecondaryAttackInput.action.canceled += OnSecondaryAttackStarted;
        SecondaryAttackInput.action.performed += OnSecondaryAttackStarted;
      }

      private void OnDisable()
      {
        PrimaryAttackInput.action.Enable();
        PrimaryAttackInput.action.started -= OnPrimaryAttackStarted;
        PrimaryAttackInput.action.canceled -= OnPrimaryAttackStarted;
        PrimaryAttackInput.action.performed -= OnPrimaryAttackStarted;
        SecondaryAttackInput.action.Enable();
        SecondaryAttackInput.action.started -= OnSecondaryAttackStarted;
        SecondaryAttackInput.action.canceled -= OnSecondaryAttackStarted;
        SecondaryAttackInput.action.performed -= OnSecondaryAttackStarted;
      }

    }
  }
}