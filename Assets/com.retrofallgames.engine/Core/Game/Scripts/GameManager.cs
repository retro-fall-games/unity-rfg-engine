using System.Collections;
using UnityEngine;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Game/Game Manager")]
  public class GameManager : PersistentSingleton<GameManager>
  {
    public GameSettings GameSettings;

    [HideInInspector]
    public bool IsPaused { get; set; }

    private void Start()
    {
      Application.targetFrameRate = GameSettings.TargetFrameRate;
    }

    public void Pause()
    {
      IsPaused = true;
      Time.timeScale = 0f;
    }

    public void UnPause()
    {
      IsPaused = false;
      Time.timeScale = 1f;
    }

    public void TogglePause()
    {
      if (IsPaused)
      {
        UnPause();
      }
      else
      {
        Pause();
      }
    }

    public void Quit()
    {
      StartCoroutine(StartQuitProcess());
    }

    private IEnumerator StartQuitProcess()
    {
      yield return new WaitForSecondsRealtime(GameSettings.WaitForSecondsToQuit);
      Application.Quit();
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

  }
}