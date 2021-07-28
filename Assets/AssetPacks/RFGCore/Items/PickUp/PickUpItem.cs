using UnityEngine;
using MyBox;

namespace RFG
{
  [CreateAssetMenu(fileName = "New Pick Up Item", menuName = "RFG Engine/Items/PickUp")]
  public class PickUpItem : ScriptableObject
  {
    public enum PickUpType { Health, Heart, Ammo, Ability }
    public Sprite pickupSprite;
    public PickUpType type = PickUpType.Health;
    [ConditionalField(nameof(type), false, PickUpType.Health)] public float healthAmount;
    [ConditionalField(nameof(type), false, PickUpType.Heart)] public float healthIncreaseAmount;
    [ConditionalField(nameof(type), false, PickUpType.Ammo)] public float ammoAmount;
    [ConditionalField(nameof(type), false, PickUpType.Ability)] public CharacterAbilities ability;
  }
}