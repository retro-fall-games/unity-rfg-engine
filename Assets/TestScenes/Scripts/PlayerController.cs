using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  public PlayerInput playerInput;
  public float movementSpeed = 3f;
  public float movementSmoothingSpeed = 1f;

  private Vector2 _velocity;
  private Vector2 rawInputMovement;

  private void Update()
  {
    CalculateMovementInputSmoothing();
    UpdatePlayerMovement();
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

  public void OnMovement(InputAction.CallbackContext ctx)
  {
    Vector2 inputMovement = ctx.ReadValue<Vector2>();
    rawInputMovement = inputMovement;
  }

  public void OnAttack(InputAction.CallbackContext ctx)
  {
    if (ctx.started)
      Debug.Log("Attack!");
  }

}
