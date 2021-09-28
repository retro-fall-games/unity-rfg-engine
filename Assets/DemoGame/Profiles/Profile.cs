using System;
// using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using MyBox;
using RFG.Core;
using RFG.Items;
using RFG.Platformer;

namespace Game
{
  [Serializable]
  [CreateAssetMenu(fileName = "New Profile", menuName = "Game/Profile")]
  public class Profile : ScriptableObject
  {
    public int Id = -1;
    public long CreatedAt = 0;
    public long StartedAt = 0;
    public long TimePlayed = 0;
    public string Level = "Intro";
    public int CheckpointIndex = 0;
    public InventorySave Inventory;
    public EquipmentSetSave EquipmentSet;

    public void Create()
    {
      var fileName = $"{Application.persistentDataPath}/profile{Id}.json";
      Level = "Intro";
      CheckpointIndex = 0;
      CreatedAt = Epoch.Current();
      Inventory = new InventorySave();
      Inventory.Items = new ItemSave[0];
      EquipmentSet = new EquipmentSetSave();
      EquipmentSet.PrimaryWeapon = new WeaponItemSave();
      EquipmentSet.SecondaryWeapon = new WeaponItemSave();
      var data = JsonUtility.ToJson(this);
      File.WriteAllText(fileName, data);
    }

    public void Save()
    {
      var fileName = $"{Application.persistentDataPath}/profile{Id}.json";
      TimePlayed = TimePlayed + Epoch.Current() - StartedAt;
      var data = JsonUtility.ToJson(this);
      File.WriteAllText(fileName, data);
    }

    public void Load()
    {
      string fileName = $"{Application.persistentDataPath}/profile{Id}.json";
      if (File.Exists(fileName))
      {
        var json = File.ReadAllText(fileName);
        JsonUtility.FromJsonOverwrite(json, this);
      }
    }

    public void Delete()
    {
      string fileName = $"{Application.persistentDataPath}/profile{Id}.json";
      if (File.Exists(fileName))
      {
        File.Delete(fileName);
        CreatedAt = 0;
        StartedAt = 0;
        TimePlayed = 0;
        CheckpointIndex = 0;
        Level = "Intro";
      }
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void ResetProfile()
    {
      Level = "Intro";
      CheckpointIndex = 0;
      CreatedAt = 0;
      StartedAt = 0;
      TimePlayed = 0;
      Inventory = new InventorySave();
      Inventory.Items = new ItemSave[0];
      EquipmentSet = new EquipmentSetSave();
      EquipmentSet.PrimaryWeapon = new WeaponItemSave();
      EquipmentSet.SecondaryWeapon = new WeaponItemSave();
    }
#endif

  }

}