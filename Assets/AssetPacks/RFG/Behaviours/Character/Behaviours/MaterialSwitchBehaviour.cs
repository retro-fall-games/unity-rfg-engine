using UnityEngine;

namespace RFG
{
  public class CharacterMaterials : BaseCharacterBehaviour
  {
    public enum MaterialState { Default, Dissolve, Dash }
    public Material dissolveMaterial;
    public Material dashMaterial;

    private SpriteRenderer _sprite;
    public Material _defaultMaterial;

    private void Awake()
    {
      _sprite = GetComponent<SpriteRenderer>();
      _defaultMaterial = _sprite.material;
    }

    public Material GetCurrentMaterial()
    {
      return _sprite.material;
    }

    public void SetMaterial(MaterialState state)
    {
      switch (state)
      {
        case MaterialState.Dissolve:
          if (dissolveMaterial != null)
          {
            _sprite.material = dissolveMaterial;
          }
          else
          {
            _sprite.material = _defaultMaterial;
          }
          break;
        case MaterialState.Dash:
          if (dashMaterial != null)
          {
            _sprite.material = dashMaterial;
          }
          else
          {
            _sprite.material = _defaultMaterial;
          }
          break;
        case MaterialState.Default:
        default:
          _sprite.material = _defaultMaterial;
          break;
      }
    }
  }
}