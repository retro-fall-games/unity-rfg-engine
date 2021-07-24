using UnityEngine;
using RFG;

namespace Game
{
  public class MainMenu : MonoBehaviour, EventListener<GameEvent>
  {
    public Animator mainMenuAnimator;
    public Animator optionsAnimator;
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
          SlideLeftOptions();
          break;
        case GameEvent.GameEventType.UnPaused:
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