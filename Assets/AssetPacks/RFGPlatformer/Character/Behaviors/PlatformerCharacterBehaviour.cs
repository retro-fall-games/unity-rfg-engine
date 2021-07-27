namespace RFG
{
  public class PlatformerCharacterBehaviour : BaseCharacterBehaviour
  {
    protected PlatformerCharacter _character;

    private void Awake()
    {
      _transform = transform;
      _character = GetComponent<PlatformerCharacter>();
    }
  }
}