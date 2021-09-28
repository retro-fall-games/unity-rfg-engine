using UnityEngine;
using UnityEngine.InputSystem;
using RFG.Core;

public class PlayerControllerInputSystem : MonoBehaviour
{
  public float movementSpeed = 3f;
  public float movementSmoothingSpeed = 1f;

  private PlayerInputActions inputActions;
  private InputAction movement;

  private Vector2 _velocity;
  private Vector2 rawInputMovement;

  private void Awake()
  {
    inputActions = new PlayerInputActions();
  }

  private void Update()
  {
    CalculateMovementInputSmoothing();
    UpdatePlayerMovement();
  }

  private void FixedUpdate()
  {
    Vector2 inputMovement = movement.ReadValue<Vector2>();
    rawInputMovement = inputMovement;
  }

  private void CalculateMovementInputSmoothing()
  {
    Vector2 speed = rawInputMovement * movementSpeed;
    _velocity = Vector2.Lerp(_velocity, speed, Time.deltaTime * movementSmoothingSpeed);
  }

  private void UpdatePlayerMovement()
  {
    transform.Translate(_velocity * Time.deltaTime, Space.World);
  }

  public void OnAttack(InputAction.CallbackContext ctx)
  {
    // if (ctx.started)
    Debug.Log("Attack!");
  }

  public void OnAttackCanceled(InputAction.CallbackContext ctx)
  {
    // if (ctx.started)
    Debug.Log("Attack Canceled!");
  }

  public void OnAttackStarted(InputAction.CallbackContext ctx)
  {
    // if (ctx.started)
    Debug.Log("Attack Started!");
  }

  public void OnPause(InputAction.CallbackContext ctx)
  {
    GameManager.Instance.TogglePause();
  }

  private void OnEnable()
  {
    //inputActions.PlayerControls
    movement = inputActions.PlayerControls.Movement;
    movement.Enable();

    inputActions.PlayerControls.PrimaryAttack.performed += OnAttack;
    inputActions.PlayerControls.PrimaryAttack.started += OnAttackStarted;
    inputActions.PlayerControls.PrimaryAttack.canceled += OnAttackCanceled;
    inputActions.PlayerControls.Pause.performed += OnPause;

    inputActions.PlayerControls.PrimaryAttack.Enable();
    inputActions.PlayerControls.Pause.Enable();
  }

  private void OnDisable()
  {
    movement.Disable();
    inputActions.PlayerControls.PrimaryAttack.Disable();
    inputActions.PlayerControls.Pause.Disable();
  }

}
