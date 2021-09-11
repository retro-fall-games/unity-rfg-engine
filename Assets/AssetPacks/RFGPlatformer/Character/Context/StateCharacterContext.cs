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
      public SettingsPack settingsPack
      {
        get
        {
          if (CurrentSettingsPack == null)
          {
            ResetSettingsPack();
          }
          return CurrentSettingsPack;
        }
      }

      public SettingsPack CurrentSettingsPack;
      public SettingsPack DefaultSettingsPack;

      // Behaviours
      public HealthBehaviour healthBehaviour;

      public void SetCurrentSettingsPack(int overrideIndex)
      {
        CurrentSettingsPack = DefaultSettingsPack;

        // First Override Base Settings
        OverrideSettingsPack(DefaultSettingsPack.BaseSettingsPack);

        // Then apply any overrides
        if (overrideIndex >= 0 && overrideIndex < DefaultSettingsPack.SettingsPackOverrides.Length)
        {
          OverrideSettingsPack(DefaultSettingsPack.SettingsPackOverrides[overrideIndex]);
        }
      }

      public void ResetSettingsPack()
      {
        SetCurrentSettingsPack(-1);
      }

      public void OverrideSettingsPack(SettingsPack settings)
      {
        if (settings.AttackSettings != null)
        {
          CurrentSettingsPack.AttackSettings = settings.AttackSettings;
        }

        if (settings.DanglingSettings != null)
        {
          CurrentSettingsPack.DanglingSettings = settings.DanglingSettings;
        }

        if (settings.DashSettings != null)
        {
          CurrentSettingsPack.DashSettings = settings.DashSettings;
        }

        if (settings.HealthSettings != null)
        {
          CurrentSettingsPack.HealthSettings = settings.HealthSettings;
        }

        if (settings.IdleSettings != null)
        {
          CurrentSettingsPack.IdleSettings = settings.IdleSettings;
        }

        if (settings.JumpSettings != null)
        {
          CurrentSettingsPack.JumpSettings = settings.JumpSettings;
        }

        if (settings.PauseSettings != null)
        {
          CurrentSettingsPack.PauseSettings = settings.PauseSettings;
        }

        if (settings.RunningSettings != null)
        {
          CurrentSettingsPack.RunningSettings = settings.RunningSettings;
        }

        if (settings.StairsSettings != null)
        {
          CurrentSettingsPack.StairsSettings = settings.StairsSettings;
        }

        if (settings.WalkingSettings != null)
        {
          CurrentSettingsPack.WalkingSettings = settings.WalkingSettings;
        }

        if (settings.WallClingingSettings != null)
        {
          CurrentSettingsPack.WallClingingSettings = settings.WallClingingSettings;
        }

        if (settings.WallJumpSettings != null)
        {
          CurrentSettingsPack.WallJumpSettings = settings.WallJumpSettings;
        }
      }
    }
  }
}