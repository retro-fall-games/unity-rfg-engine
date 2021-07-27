using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Character/Platformer Character")]
  public class PlatformerCharacter : BaseCharacter
  {
    [HideInInspector]
    public CharacterController2D Controller => _controller;
    private CharacterController2D _controller;

    protected override void Awake()
    {
      base.Awake();
      _controller = GetComponent<CharacterController2D>();
      OnBirth += OnActionBirth;
      OnKill += OnActionKill;
    }

    public void OnActionBirth()
    {
      _controller.ResetVelocity();
      _controller.enabled = true;
    }

    public void OnActionKill()
    {
      _controller.enabled = false;
      if (characterType == CharacterType.Player)
      {
        PlatformerLevelManager.Instance.SpawnPlayer();
      }
    }

  }
}