using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace RFG.Core
{

  [AddComponentMenu("RFG/Core/Input/Events/Input Action Event")]
  public class InputActionEvent : MonoBehaviour
  {
    [Header("Input")]
    /// <summary>Input Action to initiate events</summary>
    [Tooltip("Input Action to initiate events")]
    public InputActionReference InputReference;

    [Header("Events")]
    public UnityEvent OnStarted;
    public UnityEvent OnCanceled;
    public UnityEvent OnPerformed;

    private void OnStartedCallback(InputAction.CallbackContext ctx)
    {
      OnStarted?.Invoke();
    }

    private void OnCanceledCallback(InputAction.CallbackContext ctx)
    {
      OnCanceled?.Invoke();
    }

    private void OnPerformedCallback(InputAction.CallbackContext ctx)
    {
      OnPerformed?.Invoke();
    }

    private void OnEnable()
    {
      InputReference.action.Enable();
      InputReference.action.started += OnStartedCallback;
      InputReference.action.canceled += OnCanceledCallback;
      InputReference.action.performed += OnPerformedCallback;
    }

    private void OnDisable()
    {
      InputReference.action.Disable();
      InputReference.action.started -= OnStartedCallback;
      InputReference.action.canceled -= OnCanceledCallback;
      InputReference.action.performed -= OnPerformedCallback;
    }
  }
}