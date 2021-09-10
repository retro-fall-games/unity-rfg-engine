using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Game/Game Manager")]
  public class GameManager : PersistentSingleton<GameManager>
  {
    [Header("Game Settings")]
    public GameSettings Settings;

    [HideInInspector]
    public bool IsPaused { get; set; }

    private void Start()
    {
      Application.targetFrameRate = Settings.TargetFrameRate;
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
      yield return new WaitForSecondsRealtime(Settings.WaitForSecondsToQuit);
      Application.Quit();
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

  }
}