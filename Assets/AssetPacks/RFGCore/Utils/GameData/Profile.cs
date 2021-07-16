using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{

  [Serializable]
  class ProfileData
  {
    public int id;
    public long createdAt;
    public long timePlayed;
  }

  public class Profile
  {
    public int id = -1;
    public long createdAt = Epoch.Current();
    public long timePlayed = 0;

    public void Save()
    {
      string fileName = $"{Application.persistentDataPath}/profile{id}.dat";
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Create(fileName);
      ProfileData data = new ProfileData();
      data.id = id;
      data.createdAt = createdAt;
      data.timePlayed = timePlayed;
      bf.Serialize(file, data);
      file.Close();
    }

    public void Load(int _id)
    {
      string fileName = $"{Application.persistentDataPath}/profile{_id}.dat";
      if (File.Exists(fileName))
      {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(fileName, FileMode.Open);
        ProfileData data = (ProfileData)bf.Deserialize(file);
        file.Close();
        id = data.id;

        createdAt = data.createdAt;
        timePlayed = data.timePlayed;
      }
      else
      {
        id = -1;
        createdAt = Epoch.Current();
        timePlayed = 0;
      }
    }

    public void Delete()
    {
      string fileName = $"{Application.persistentDataPath}/profile{id}.dat";
      if (File.Exists(fileName))
      {
        File.Delete(fileName);
      }
    }

    public static List<Profile> GetProfiles(int count)
    {
      List<Profile> profiles = new List<Profile>();
      for (int i = 0; i < count; i++)
      {
        Profile profile = new Profile();
        profile.Load(i);
        profiles.Add(profile);
      }
      return profiles;
    }

  }
}
