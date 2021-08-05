using UnityEngine;

namespace RFG
{
  [CreateAssetMenu(fileName = "New Game Settings", menuName = "RFG/Game/Game Settings")]
  public class GameSettings : ScriptableObject
  {
    [Header("Runtime Settings")]
    public int TargetFrameRate = 300;
    public float WaitForSecondsToQuit = 3f;

    [Header("Debug Settings")]
    public bool DrawRaycasts = true;

    [HideInInspector]
    public bool IsPaused { get; set; }

    public void Pause()
    {
      IsPaused = true;
      Time.timeScale = 0f;
      EventManager.TriggerEvent<GameEvent>(new GameEvent(GameEvent.GameEventType.Paused));
    }

    public void UnPause()
    {
      IsPaused = false;
      Time.timeScale = 1f;
      EventManager.TriggerEvent<GameEvent>(new GameEvent(GameEvent.GameEventType.UnPaused));
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

  }
}