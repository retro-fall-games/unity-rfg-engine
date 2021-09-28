using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace RFG.Platformer
{
  [AddComponentMenu("RFG/Platformer/Character/Ability/Attack")]
  public class AttackAbility : MonoBehaviour, IAbility
  {
    [HideInInspector]
    private Character _character;
    private EquipmentSet _equipmentSet;
    private InputActionReference _primaryAttackInput;
    private InputActionReference _secondaryAttackInput;

    private void Awake()
    {
      _character = GetComponent<Character>();
      _equipmentSet = GetComponent<EquipmentSet>();
    }

    private void Start()
    {
      _primaryAttackInput = _character.Context.inputPack.PrimaryAttackInput;
      _secondaryAttackInput = _character.Context.inputPack.SecondaryAttackInput;

      // Setup Events
      OnEnable();
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

      // Make sure to setup new events
      OnDisable();

      if (_primaryAttackInput != null)
      {
        _primaryAttackInput.action.Enable();
        _primaryAttackInput.action.started += OnPrimaryAttackStarted;
        _primaryAttackInput.action.canceled += OnPrimaryAttackCanceled;
        _primaryAttackInput.action.performed += OnPrimaryAttackPerformed;
      }

      if (_secondaryAttackInput != null)
      {
        _secondaryAttackInput.action.Enable();
        _secondaryAttackInput.action.started += OnSecondaryAttackStarted;
        _secondaryAttackInput.action.canceled += OnSecondaryAttackCanceled;
        _secondaryAttackInput.action.performed += OnSecondaryAttackPerformed;
      }
    }

    private void OnDisable()
    {
      if (_primaryAttackInput != null)
      {
        _primaryAttackInput.action.Disable();
        _primaryAttackInput.action.started -= OnPrimaryAttackStarted;
        _primaryAttackInput.action.canceled -= OnPrimaryAttackCanceled;
        _primaryAttackInput.action.performed -= OnPrimaryAttackPerformed;
      }

      if (_secondaryAttackInput != null)
      {
        _secondaryAttackInput.action.Disable();
        _secondaryAttackInput.action.started -= OnSecondaryAttackStarted;
        _secondaryAttackInput.action.canceled -= OnSecondaryAttackCanceled;
        _secondaryAttackInput.action.performed -= OnSecondaryAttackPerformed;
      }
    }

  }
}