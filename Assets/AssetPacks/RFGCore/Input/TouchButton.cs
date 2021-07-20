using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Input/Touch Button")]
  public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
  {
    public RectTransform rectTransform;
    public string buttonId;
    private Button _button;
    private Camera _cameraMain;

    private void Start()
    {
      _cameraMain = Camera.main;
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      yield return new WaitUntil(() => InputManager.Instance != null);
      yield return new WaitUntil(() => InputManager.Instance.GetButton(buttonId) != null);
      _button = InputManager.Instance.GetButton(buttonId);
      Debug.Log("Got the button");
    }

    private void Update()
    {
      if (_button != null)
      {
        CheckForTouch();
      }
    }

    private void CheckForTouch()
    {
      if (Input.touchCount > 0)
      {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
          if (DidTouchButton())
          {
            _button.State.ChangeState(ButtonStates.Down);
          }
          else
          {
            _button.State.ChangeState(ButtonStates.Up);
          }
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
          _button.State.ChangeState(ButtonStates.Up);
        }
      }
    }

    private bool DidTouchButton()
    {
      return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.GetTouch(0).position, _cameraMain);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      if (_button != null)
      {
        _button.State.ChangeState(ButtonStates.Down);
      }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (_button != null)
      {
        _button.State.ChangeState(ButtonStates.Up);
      }
    }


  }
}
