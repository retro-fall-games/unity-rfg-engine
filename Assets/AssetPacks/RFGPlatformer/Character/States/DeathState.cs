using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Death State", menuName = "RFG/Platformer/Character/State/Death")]
    public class DeathState : State
    {
      public override Type Execute(Transform transform, Animator animator)
      {
        return typeof(DeadState);
      }

      public override void Exit(Transform transform, Animator animator)
      {
        base.Exit(transform, animator);
        Character character = transform.GetComponent<Character>();
        if (character.CharacterType == CharacterType.Player)
        {
          GameManager.Instance.StartCoroutine(character.Respawn());
        }
        character.Controller.enabled = false;
        transform.gameObject.SetActive(false);
      }

    }
  }
}