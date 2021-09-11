using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Pause")]
    public class PauseAbility : MonoBehaviour, IAbility
    {
      [HideInInspector]
      private Transform _transform;
      private Character _character;
      private InputActionReference _pauseInput;
      private PauseSettings _pauseSettings;

      private void Awake()
      {
        _transform = transform;
      }

      private void Start()
      {
        _character = GetComponent<Character>();
        _pauseInput = _character.Context.inputPack.PauseInput;
        _pauseSettings = _character.Context.settingsPack.PauseSettings;

        // Setup events
        OnEnable();
      }

      public void OnPausedPerformed(InputAction.CallbackContext ctx)
      {
        if (GameManager.Instance.IsPaused)
        {
          _transform.SpawnFromPool("Effects", _pauseSettings.PauseEffects);
        }
        else
        {
          _transform.SpawnFromPool("Effects", _pauseSettings.UnPauseEffects);
        }
        _pauseSettings.PauseEvent?.Raise();
      }

      private void OnEnable()
      {
        // Make sure to setup new events
        OnDisable();

        if (_pauseInput != null)
        {
          _pauseInput.action.Enable();
          _pauseInput.action.performed += OnPausedPerformed;
        }
      }

      private void OnDisable()
      {
        if (_pauseInput != null)
        {
          _pauseInput.action.Disable();
          _pauseInput.action.performed -= OnPausedPerformed;
        }
      }

    }
  }
}