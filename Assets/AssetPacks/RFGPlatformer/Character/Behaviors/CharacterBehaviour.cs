using UnityEngine;

namespace RFG
{
  public class CharacterBehaviour : MonoBehaviour
  {
    protected Transform _transform;
    protected Character _character;
    protected float _horizontalInput;
    protected float _verticalInput;

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
      if (_character.CharacterInput == null)
      {
        return;
      }
      _horizontalInput = _character.CharacterInput.InputManager.PrimaryMovement.x;
      _verticalInput = _character.CharacterInput.InputManager.PrimaryMovement.y;
      HandleInput();
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