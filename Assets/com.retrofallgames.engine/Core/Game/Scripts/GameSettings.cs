using UnityEngine;

namespace RFG.Core
{
  [CreateAssetMenu(fileName = "New Game Settings", menuName = "RFG/Core/Game/Game Settings")]
  public class GameSettings : ScriptableObject
  {
    [Header("Runtime Settings")]
    public int TargetFrameRate = 300;
    public float WaitForSecondsToQuit = 3f;

    [Header("Debug Settings")]
    public bool DrawRaycasts = true;

  }
}