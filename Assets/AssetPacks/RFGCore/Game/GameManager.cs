using System.Collections;
using UnityEngine;
using RFGFx;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Core/Managers/Game Manager")]
  public class GameManager : PersistentSingleton<GameManager>, EventListener<GameEvent>
  {
    [Header("Settings")]
    public int targetFrameRate = 300;

    [Header("Debug")]
    public bool drawRaycasts = true;
    public bool IsPaused { get; private set; }

    private void Start()
    {
      // Application.targetFrameRate = targetFrameRate;
    }

    public void Quit()
    {
      StartCoroutine(QuitGameCo());
    }

    private IEnumerator QuitGameCo()
    {
      AudioManager.Instance.StopAll(true);
      Transition.Instance.Show("CrossFade", "Start");
      yield return new WaitForSecondsRealtime(3f);

#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
      Application.Quit();
#else
      Application.Quit();
#endif
    }

    public void Pause()
    {
      IsPaused = true;
      Time.timeScale = 0f;
      EventManager.TriggerEvent(new GameEvent(GameEvent.GameEventType.Paused));
    }

    public void UnPause()
    {
      IsPaused = false;
      Time.timeScale = 1f;
      EventManager.TriggerEvent(new GameEvent(GameEvent.GameEventType.UnPaused));
    }

    public void OnEvent(GameEvent gameEvent)
    {
      switch (gameEvent.eventType)
      {
        case GameEvent.GameEventType.Pause:
          if (!IsPaused)
          {
            Pause();
          }
          else
          {
            UnPause();
          }
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