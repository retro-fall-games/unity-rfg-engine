using UnityEngine;

namespace RFG
{
  public class BaseCharacterBehaviour : MonoBehaviour
  {
    public bool authorized = true;
    protected Transform _transform;
    protected BaseCharacter _baseCharacter;

    private void Awake()
    {
      _transform = transform;
      _baseCharacter = GetComponent<BaseCharacter>();
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