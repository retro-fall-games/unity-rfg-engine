using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  [AddComponentMenu("RFG/Scene/Scene Skip")]
  public class SceneSkip : MonoBehaviour
  {
    [Header("Settings")]
    public string NextScene;
    public float WaitForSeconds = 3f;
    public bool CanSkipOnAnyInput = true;

    [Header("Unity Events")]
    public SceneUnityEvent OnSceneChange;

    [HideInInspector]
    private bool _hasSkipped = false;
    private Coroutine _autoSkipCo;

    private void Start()
    {
      _autoSkipCo = StartCoroutine(AutoSkip());
    }

    private IEnumerator AutoSkip()
    {
      yield return new WaitForSecondsRealtime(WaitForSeconds);
      OnSceneChange?.Raise(NextScene);
    }

    private void FixedUpdate()
    {
      if (CanSkipOnAnyInput)
      {
        bool anyInput = false;
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.anyKey.wasPressedThisFrame)
        {
          anyInput = true;
        }
        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
        {
          anyInput = true;
        }
        // var gamepad = Gamepad.current;
        // if (gamepad != null && gamepad.wasUpdatedThisFrame)
        // {
        //   Debug.Log("Gamepad input");
        //   anyInput = true;
        // }
        var pointer = Pointer.current;
        if (pointer != null && pointer.press.isPressed)
        {
          anyInput = true;
        }

        if (!_hasSkipped && anyInput)
        {
          _hasSkipped = true;
          StopCoroutine(_autoSkipCo);
          OnSceneChange?.Raise(NextScene);
        }
      }
    }

  }
}