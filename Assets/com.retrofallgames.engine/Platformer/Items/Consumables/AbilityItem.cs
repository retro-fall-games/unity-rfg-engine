using System.Collections.Generic;
using UnityEngine;

namespace RFG.Platformer
{
  using StateMachine;
  using Items;

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