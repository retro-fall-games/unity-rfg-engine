using UnityEngine;

namespace RFG
{
  public class CharacterBehavior : MonoBehaviour
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

    public virtual void InitBehavior()
    {
    }

    public virtual void EarlyProcessBehavior()
    {
      if (_character.CharacterInput == null)
      {
        return;
      }
      _horizontalInput = _character.CharacterInput.InputManager.PrimaryMovement.x;
      _verticalInput = _character.CharacterInput.InputManager.PrimaryMovement.y;
      HandleInput();
    }

    public virtual void ProcessBehavior()
    {
    }

    public virtual void LateProcessBehavior()
    {
    }

    protected virtual void HandleInput()
    {
    }
  }
}