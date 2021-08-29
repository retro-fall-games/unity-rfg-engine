using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Pause")]
    public class PauseAbility : MonoBehaviour, IAbility
    {
      [Header("Input")]
      /// <summary>Input Action to initiate the Pause Event</summary>
      [Tooltip("Input Action to initiate the Pause Event")]
      public InputActionReference PauseInput;

      [Header("Settings")]
      /// <summary>Pause Settings to know game events and effects</summary>
      [Tooltip("Pause Settings to know game events and effects")]
      public PauseSettings PauseSettings;

      [HideInInspector]
      private Transform _transform;

      private void Awake()
      {
        _transform = transform;
      }

      public void OnPausedPerformed(InputAction.CallbackContext ctx)
      {
        if (GameManager.Instance.IsPaused)
        {
          _transform.SpawnFromPool("Effects", PauseSettings.PauseEffects);
        }
        else
        {
          _transform.SpawnFromPool("Effects", PauseSettings.UnPauseEffects);
        }
        PauseSettings.PauseEvent?.Raise();
      }

      private void OnEnable()
      {
        PauseInput.action.Enable();
        PauseInput.action.performed += OnPausedPerformed;
      }

      private void OnDisable()
      {
        PauseInput.action.Disable();
        PauseInput.action.performed -= OnPausedPerformed;
      }

    }
  }
}