using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Dead State", menuName = "RFG/Platformer/Character/State/Dead")]
    public class DeadState : State
    {
      public override void Enter(Transform transform, Animator animator)
      {
        base.Enter(transform, animator);
        Character character = transform.GetComponent<Character>();
        if (character.CharacterType == CharacterType.Player)
        {
          GameManager.Instance.StartCoroutine(character.Respawn());
        }
        character.Controller.enabled = false;
        character.DisableAllAbilities();
        transform.gameObject.SetActive(false);
      }
    }
  }
}