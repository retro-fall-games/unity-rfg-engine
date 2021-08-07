using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class CharacterBehaviour : MonoBehaviour
    {
      public bool authorized = true;
      protected Transform _transform;
      protected Character _character;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
      }

      public virtual void InitBehaviour()
      {
      }

      public virtual void EarlyProcessBehaviour()
      {
      }

      public virtual void ProcessBehaviour()
      {
      }

      public virtual void LateProcessBehaviour()
      {
      }

      protected virtual void HandleInput()
      {
      }
    }
  }
}