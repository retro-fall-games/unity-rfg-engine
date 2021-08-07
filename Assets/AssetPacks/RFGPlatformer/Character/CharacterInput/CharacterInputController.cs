using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Character Input Controller")]
    public class CharacterInputController : MonoBehaviour
    {
      public PlayerInputActions InputActions
      {
        get
        {
          if (_inputActions == null)
          {
            _inputActions = new PlayerInputActions();
          }
          return _inputActions;
        }
      }
      public InputAction Movement => _movement;
      private PlayerInputActions _inputActions;
      private InputAction _movement;

      private void OnEnable()
      {
        _movement = InputActions.PlayerControls.Movement;
        _movement.Enable();
        InputActions.PlayerControls.PrimaryAttack.Enable();
        InputActions.PlayerControls.SecondaryAttack.Enable();
        InputActions.PlayerControls.Jump.Enable();
        InputActions.PlayerControls.Pause.Enable();
      }

      private void OnDisable()
      {
        _movement.Disable();
        InputActions.PlayerControls.PrimaryAttack.Disable();
        InputActions.PlayerControls.SecondaryAttack.Disable();
        InputActions.PlayerControls.Jump.Disable();
        InputActions.PlayerControls.Pause.Disable();
      }

    }
  }
}