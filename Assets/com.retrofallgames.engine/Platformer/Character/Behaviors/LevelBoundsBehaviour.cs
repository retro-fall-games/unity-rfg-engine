using UnityEngine;

namespace RFG.Platformer
{
  [AddComponentMenu("RFG/Platformer/Character/Behaviours/Level Bounds")]
  public class LevelBoundsBehaviour : MonoBehaviour
  {
    private LevelBounds _levelBounds;
    private Character _character;

    private void Awake()
    {
      _character = GetComponent<Character>();
      GameObject levelBounds = GameObject.Find("LevelBounds");
      _levelBounds = levelBounds.GetComponent<LevelBounds>();
    }

    private void LateUpdate()
    {
      if (_character.CharacterState.CurrentStateType != typeof(AliveState))
        return;
      _levelBounds.HandleLevelBounds(_character);
    }

  }
}
