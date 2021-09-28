using UnityEngine;
using RFG.Core;

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
        }
        else
        {
          optionsAnimator.Play("SlideRight");
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