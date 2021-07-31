using UnityEngine;
using RFG;

namespace Game
{
  public class MainMenu : MonoBehaviour, EventListener<GameEvent>
  {
    public Animator mainMenuAnimator;
    public Animator optionsAnimator;
    private bool _paused = false;
    public void SlideLeftMainMenu()
    {
      if (mainMenuAnimator != null)
      {
        mainMenuAnimator.Play("SlideLeft");
      }
    }

    public void SlideRightMainMenu()
    {
      if (mainMenuAnimator != null)
      {
        mainMenuAnimator.Play("SlideRight");
      }
    }

    public void ToggleOptions()
    {
      if (optionsAnimator != null)
      {
        if (!_paused)
        {
          optionsAnimator.Play("SlideLeft");
          _paused = true;
          GameManager.Instance.Pause();
        }
        else
        {
          optionsAnimator.Play("SlideRight");
          _paused = false;
          GameManager.Instance.UnPause();
        }
      }
    }

    public void SlideLeftOptions()
    {
      if (optionsAnimator != null)
      {
        optionsAnimator.Play("SlideLeft");
      }
    }

    public void SlideRightOptions()
    {
      if (optionsAnimator != null)
      {
        optionsAnimator.Play("SlideRight");
      }
    }

    public void OnEvent(GameEvent gameEvent)
    {
      switch (gameEvent.eventType)
      {
        case GameEvent.GameEventType.Paused:
          _paused = true;
          SlideLeftOptions();
          break;
        case GameEvent.GameEventType.UnPaused:
          _paused = false;
          SlideRightOptions();
          break;
      }
    }

    private void OnEnable()
    {
      this.AddListener<GameEvent>();
    }

    private void OnDisable()
    {
      this.RemoveListener<GameEvent>();
    }

  }
}