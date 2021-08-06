using UnityEngine;
using RFG;

namespace Game
{
  public class MainMenu : MonoBehaviour
  {
    [Header("Animator Menus")]
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

    public void ToggleOptions()
    {
      if (optionsAnimator != null)
      {
        if (!GameManager.Instance.IsPaused)
        {
          optionsAnimator.Play("SlideLeft");
          GameManager.Instance.Pause();
        }
        else
        {
          optionsAnimator.Play("SlideRight");
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

  }
}