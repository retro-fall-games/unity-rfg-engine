using UnityEngine;

namespace RFG
{
  [CreateAssetMenu(fileName = "New Game Event", menuName = "RFG/Game/Game System Event")]
  public class GameSystemEvent : ScriptableObject
  {
    public event System.Action OnRaise;

    public void Raise()
    {
      if (OnRaise != null)
      {
        OnRaise();
      }
    }
  }
}