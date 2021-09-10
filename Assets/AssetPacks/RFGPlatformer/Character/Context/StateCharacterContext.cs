namespace RFG
{
  namespace Platformer
  {
    public class StateCharacterContext : StateAnimatorContext
    {
      public Character character;
      public CharacterController2D controller;

      // Packs
      public InputPack inputPack;
      public SettingsPack settingsPack;

      // Behaviours
      public HealthBehaviour healthBehaviour;
    }
  }
}