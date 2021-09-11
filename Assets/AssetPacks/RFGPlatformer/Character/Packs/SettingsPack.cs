using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Settings Pack", menuName = "RFG/Platformer/Character/Packs/Settings")]
    public class SettingsPack : ScriptableObject
    {
      [Header("Settings")]
      public AttackSettings AttackSettings;
      public DanglingSettings DanglingSettings;
      public DashSettings DashSettings;
      public HealthSettings HealthSettings;
      public IdleSettings IdleSettings;
      public JumpSettings JumpSettings;
      public PauseSettings PauseSettings;
      public RunningSettings RunningSettings;
      public StairsSettings StairsSettings;
      public WalkingSettings WalkingSettings;
      public WallClingingSettings WallClingingSettings;
      public WallJumpSettings WallJumpSettings;

      [Header("Base")]
      public SettingsPack BaseSettingsPack;

      [Header("Override")]
      public SettingsPack[] SettingsPackOverrides;
    }
  }
}