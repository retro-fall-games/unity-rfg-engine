using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using RFG;

namespace Game
{
  [Serializable]
  [CreateAssetMenu(fileName = "New Profile", menuName = "Game/Profile")]
  public class Profile : ScriptableObject
  {
    public int id = -1;
    public long createdAt = 0;
    public long timePlayed = 0;
    public string level = "Intro";

    public void Create()
    {
      var fileName = $"{Application.persistentDataPath}/profile{id}.json";
      createdAt = Epoch.Current();
      var data = JsonUtility.ToJson(this);
      File.WriteAllText(fileName, data);
    }

    public void Save()
    {
      var fileName = $"{Application.persistentDataPath}/profile{id}.json";
      var data = JsonUtility.ToJson(this);
      File.WriteAllText(fileName, data);
    }

    public void Load()
    {
      string fileName = $"{Application.persistentDataPath}/profile{id}.json";
      if (File.Exists(fileName))
      {
        var json = File.ReadAllText(fileName);
        JsonUtility.FromJsonOverwrite(json, this);
      }
    }

    public void Delete()
    {
      string fileName = $"{Application.persistentDataPath}/profile{id}.json";
      if (File.Exists(fileName))
      {
        File.Delete(fileName);
        createdAt = 0;
        timePlayed = 0;
        level = "Intro";
      }
    }

  }
}