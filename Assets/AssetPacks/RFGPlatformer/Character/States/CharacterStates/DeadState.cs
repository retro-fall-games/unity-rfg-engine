using UnityEngine;
using RFG.StateMachine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Dead State", menuName = "RFG/Platformer/Character/States/Character State/Dead")]
    public class DeadState : State
    {
      public override void Enter(IStateContext context)
      {
        base.Enter(context);
        StateCharacterContext characterContext = context as StateCharacterContext;
        if (characterContext.character.CharacterType == CharacterType.Player)
        {
          GameManager.Instance.StartCoroutine(characterContext.character.Respawn());
        }
        characterContext.controller.enabled = false;
        characterContext.character.DisableAllAbilities();
        characterContext.transform.gameObject.SetActive(false);
      }
    }
  }
}