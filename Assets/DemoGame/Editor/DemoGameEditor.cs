using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Game
{
  public class DemoGameEditor : EditorWindow
  {
    private const string Path = "Assets/DemoGame";
    private const string ItemsPath = "Assets/DemoGame/Items";
    private const string PickUpsPath = "Assets/DemoGame/Items/PickUps";
    private const string WeaponsPickUpsPath = "Assets/DemoGame/Items/PickUps/WeaponPickUps";
    private const string HealthPickUpsPath = "Assets/DemoGame/Items/PickUps/HealthPickUps";
    private const string AbilityPickUpsPath = "Assets/DemoGame/Items/PickUps/AbilityPickUps";
    private const string LevelPortalsPath = "Assets/DemoGame/Interactions/LevelPortals";
    private const string FloorsPath = "Assets/DemoGame/Environment/Floors/Prefabs";

    [MenuItem("Window/Demo Game")]
    public static void ShowWindow()
    {
      GetWindow<DemoGameEditor>("Demo Game");
    }

    private void OnGUI()
    {
      GUILayout.Label("Demo Game");
      GUILayout.Space(10);

      GUILayout.Label("Weapon Pickups");
      GUILayout.Space(10);

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Slingshot", GUILayout.Width(150)))
      {
        CreatePrefab(WeaponsPickUpsPath, "SlingshotPickUp");
      }
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Shockwave", GUILayout.Width(150)))
      {
        CreatePrefab(WeaponsPickUpsPath, "ShockwavePickUp");
      }
      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.Space(10);

      GUILayout.Label("Health Pickups");
      GUILayout.Space(10);

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Low Health", GUILayout.Width(150)))
      {
        CreatePrefab(HealthPickUpsPath, "LowHealthPickUp");
      }
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Max Health", GUILayout.Width(150)))
      {
        CreatePrefab(HealthPickUpsPath, "MaxHealthPickUp");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.Space(10);

      GUILayout.Label("Ability Pickups");
      GUILayout.Space(10);

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Dash", GUILayout.Width(150)))
      {
        CreatePrefab(AbilityPickUpsPath, "DashPickUp");
      }
      if (GUILayout.Button("Double Jump", GUILayout.Width(150)))
      {
        CreatePrefab(AbilityPickUpsPath, "DoubleJumpPickUp");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Wall Clinging", GUILayout.Width(150)))
      {
        CreatePrefab(AbilityPickUpsPath, "WallClingingPickUp");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.Space(10);

      GUILayout.Label("Interactions");
      GUILayout.Space(10);

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Checkpoint", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/AssetPacks/RFG/Interactions/Checkpoints/Prefabs", "Checkpoint");
      }

      if (GUILayout.Button("Warp", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/DemoGame/Interactions/Warps/Prefabs", "Warp");
      }


      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Start", GUILayout.Width(150)))
      {
        CreatePrefab(LevelPortalsPath, "StartPortal");
      }

      if (GUILayout.Button("Exit", GUILayout.Width(150)))
      {
        CreatePrefab(LevelPortalsPath, "ExitPortal");
      }


      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Trigger", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/AssetPacks/RFG/Interactions/Trigger/Prefabs", "Trigger");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.Space(10);

      GUILayout.Label("Platforms");
      GUILayout.Space(10);

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Stairs", GUILayout.Width(150)))
      {
        CreatePrefab(FloorsPath, "Stairs");
      }
      if (GUILayout.Button("One Way", GUILayout.Width(150)))
      {
        CreatePrefab(FloorsPath, "OneWayPlatform");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Moving", GUILayout.Width(150)))
      {
        CreatePrefab(FloorsPath, "MovingPlatform");
      }
      if (GUILayout.Button("One Way Moving", GUILayout.Width(150)))
      {
        CreatePrefab(FloorsPath, "OneWayMovingPlatform");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.Space(10);

      GUILayout.Label("Navigation");
      GUILayout.Space(10);

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Movement Path", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/AssetPacks/RFG/Navigation/Paths/Prefabs", "MovementPath");
      }

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Path", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/AssetPacks/RFG/Navigation/Paths/Prefabs", "Path");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.Space(10);

      GUILayout.Label("Spawners");
      GUILayout.Space(10);

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Bat", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/DemoGame/Character/Characters/Bat/Prefabs", "BatSpawner");
      }
      if (GUILayout.Button("Robot", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/DemoGame/Character/Characters/Robot/Prefabs", "RobotSpawner");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

      GUILayout.BeginHorizontal();

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Rabbit Robot", GUILayout.Width(150)))
      {
        CreatePrefab("Assets/DemoGame/Character/Characters/RabbitRobot/Prefabs", "RabbitRobotSpawner");
      }

      GUILayout.FlexibleSpace();

      GUILayout.EndHorizontal();

    }

    private void CreatePrefab(string path, string objName)
    {
      UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath($"{path}/{objName}.prefab", typeof(GameObject));
      GameObject clone = null;
      clone = PrefabUtility.InstantiatePrefab(obj) as GameObject;
      Selection.activeObject = clone;
      clone.name = objName;
    }

  }
}