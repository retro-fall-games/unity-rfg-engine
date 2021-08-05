using UnityEngine;
using UnityEngine.InputSystem;
using RFG;

public class PlayerControllerInputSystem : MonoBehaviour
{
  public float movementSpeed = 3f;
  public float movementSmoothingSpeed = 1f;

  public GameSystemEvent OnPauseEvent;

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

  public void OnPause(InputAction.CallbackContext ctx)
  {
    OnPauseEvent.Raise();
  }

  private void OnEnable()
  {
    movement = inputActions.PlayerControls.Movement;
    movement.Enable();

    inputActions.PlayerControls.Attack.performed += OnAttack;
    inputActions.PlayerControls.Pause.performed += OnPause;

    inputActions.PlayerControls.Attack.Enable();
    inputActions.PlayerControls.Pause.Enable();
  }

  private void OnDisable()
  {
    movement.Disable();
    inputActions.PlayerControls.Attack.Disable();
    inputActions.PlayerControls.Pause.Disable();
  }

}
