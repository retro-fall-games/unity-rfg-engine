using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Character Input Controller")]
    public class CharacterInputController : MonoBehaviour
    {
      [Header("Cursor")]
      public Texture2D CustomCursor;

      [Header("Joystick")]
      public VariableJoystick VariableJoystick;

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
      public Vector2 PrimaryMovement
      {
        get
        {
          if (VariableJoystick)
          {
            return VariableJoystick.Direction;
          }
          else
          {
            return Movement.ReadValue<Vector2>();
          }
        }
      }
      private PlayerInputActions _inputActions;
      private InputAction _movement;


      private void Awake()
      {
        if (CustomCursor != null)
        {
          // Cursor.SetCursor(CustomCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
      }

      private void OnEnable()
      {
        _movement = InputActions.PlayerControls.Movement;
        _movement.Enable();
        InputActions.PlayerControls.PrimaryAttack.Enable();
        InputActions.PlayerControls.SecondaryAttack.Enable();
        InputActions.PlayerControls.Jump.Enable();
        InputActions.PlayerControls.Pause.Enable();
        InputActions.PlayerControls.Dash.Enable();
      }

      private void OnDisable()
      {
        _movement.Disable();
        InputActions.PlayerControls.PrimaryAttack.Disable();
        InputActions.PlayerControls.SecondaryAttack.Disable();
        InputActions.PlayerControls.Jump.Disable();
        InputActions.PlayerControls.Pause.Disable();
        InputActions.PlayerControls.Dash.Disable();
      }

    }
  }
}