using System.Collections.Generic;
using UnityEngine;
using RFG.StateMachine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Ability Item", menuName = "RFG/Platformer/Items/Consumable/Ability")]
    public class AbilityItem : Consumable
    {
      public List<State> AbilityStates;

      public override void Consume(Inventory inventory, bool showEffects = true)
      {
        base.Consume(inventory, showEffects);
        Character character = inventory.GetComponent<Character>();
        if (character != null)
        {
          AbilityStates.ForEach(state => character.MovementState.Add(state));
        }
      }
    }
  }
}