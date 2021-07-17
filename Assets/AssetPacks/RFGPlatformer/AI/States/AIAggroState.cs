using System;

namespace RFG
{
  public class AIAggroState : TickBaseState
  {
    private Character _character;
    private Aggro _aggro;

    public AIAggroState(Character character, Aggro aggro) : base(character.gameObject)
    {
      _character = character;
      _aggro = aggro;
    }

    public override Type Tick()
    {
      if (_aggro.target2 != null)
      {
        _character.Controller.RotateTowards(_aggro.target2);
      }
      return null;
    }

    public override void OnEnter()
    {
      WeaponBehaviour weapon = _character.FindBehaviour<WeaponBehaviour>();
      if (weapon != null)
      {
        weapon.Equip(0);
      }
      return;
    }
    public override void OnExit()
    {
      WeaponBehaviour weapon = _character.FindBehaviour<WeaponBehaviour>();
      if (weapon != null)
      {
        weapon.Unequip();
      }
      return;
    }

  }
}