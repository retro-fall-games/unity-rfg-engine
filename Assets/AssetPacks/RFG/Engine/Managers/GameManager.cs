using UnityEngine;
using RFG.Events;
using RFG.Utils.Singletons;

namespace RFG.Engine.Managers
{
  [AddComponentMenu("RFG Engine/Managers/Game Manager")]
  public class GameManager : PersistentSingleton<GameManager>, EventListener<GameEvent>
  {
    [Header("Settings")]
    public int targetFrameRate = 300;
    public bool IsPaused { get; private set; }

    private void Start()
    {
      Application.targetFrameRate = targetFrameRate;
    }

    public void Pause()
    {
      IsPaused = true;
    }

    public void UnPause()
    {
      IsPaused = false;
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