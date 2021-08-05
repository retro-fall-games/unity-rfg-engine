using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Game/Game Manager")]
  public class GameManager : PersistentSingleton<GameManager>
  {
    [Header("Game Settings")]
    public GameSettings Settings;

    [Header("Game System Events")]
    public GameSystemEvent OnPauseEvent;
    public GameSystemEvent OnQuitEvent;

    private void Start()
    {
      Application.targetFrameRate = Settings.TargetFrameRate;
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

    public void Pause()
    {
      Settings.Pause();
    }

    public void UnPause()
    {
      Settings.UnPause();
    }

    public void TogglePause()
    {
      Settings.TogglePause();
    }

    private void OnEnable()
    {
      if (OnPauseEvent != null)
      {
        OnPauseEvent.OnRaise += TogglePause;
      }
      if (OnQuitEvent != null)
      {
        OnQuitEvent.OnRaise += Quit;
      }
    }

    private void OnDisable()
    {
      if (OnPauseEvent != null)
      {
        OnPauseEvent.OnRaise -= TogglePause;
      }
      if (OnQuitEvent != null)
      {
        OnQuitEvent.OnRaise -= Quit;
      }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
      GUI.Label(new Rect(0, 0, 300, 100), "FPS: " + ((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }
#endif

  }
}