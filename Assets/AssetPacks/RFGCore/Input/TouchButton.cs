using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Input/Touch Button")]
  public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
  {
    public RectTransform rectTransform;
    public string buttonId;
    private Button _button;
    private Camera _cameraMain;
    private Dictionary<Touch, ButtonStates> _touchStates;

    private void Awake()
    {
      _touchStates = new Dictionary<Touch, ButtonStates>();
    }

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
        foreach (Touch touch in Input.touches)
        {
          if (touch.phase == TouchPhase.Began)
          {
            if (DidTouchButton(touch))
            {
              // Keep a dictionary of what touch happen to touch the button
              if (!_touchStates.ContainsKey(touch))
              {
                _touchStates.Add(touch, ButtonStates.Down);
              }
              else
              {
                _touchStates[touch] = ButtonStates.Down;
              }

              // The touch did touch the button
              _button.State.ChangeState(ButtonStates.Down);
            }
          }
          else if (touch.phase == TouchPhase.Moved)
          {
            // Dont allow touches to count when the move, only when they begin
            if (!DidTouchButton(touch))
            {
              // Did the touch move and now its not touching the button?
              CheckTouchUp(touch);
            }
          }
          else if (touch.phase == TouchPhase.Ended)
          {
            // Did the touch for that specific dictionary entry end?
            CheckTouchUp(touch);
          }
        }
      }
      else
      {
        ClearTouches();
      }
    }

    private bool DidTouchButton(Touch touch)
    {
      return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position, _cameraMain);
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

    private void CheckTouchUp(Touch touch)
    {
      if (_touchStates.ContainsKey(touch))
      {
        ButtonStates btnState = _touchStates[touch];
        if (btnState == ButtonStates.Down || btnState == ButtonStates.Pressed)
        {
          _button.State.ChangeState(ButtonStates.Up);
        }
      }
    }

    private void ClearTouches()
    {
      foreach (KeyValuePair<Touch, ButtonStates> item in _touchStates)
      {
        CheckTouchUp(item.Key);
      }
    }


  }
}
