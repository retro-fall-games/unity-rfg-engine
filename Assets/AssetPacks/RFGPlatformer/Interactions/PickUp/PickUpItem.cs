using UnityEngine;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Pick Up Item", menuName = "RFG Engine/Items/PickUp")]
    public class PickUpItem : ScriptableObject
    {
      public enum PickUpType { Health, Heart, Ammo, CharacterAbility }

      [Header("Settings")]
      public Sprite pickupSprite;
      public PickUpType type = PickUpType.Health;
      [ConditionalField(nameof(type), false, PickUpType.Health)] public float healthAmount;
      [ConditionalField(nameof(type), false, PickUpType.Heart)] public float healthIncreaseAmount;
      [ConditionalField(nameof(type), false, PickUpType.Ammo)] public float ammoAmount;
      // [ConditionalField(nameof(type), false, PickUpType.CharacterAbility)] public CharacterAbilities ability;
      public string pickupText;
    }
  }
}