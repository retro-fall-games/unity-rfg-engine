using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace RFG
{

  [Serializable]
  public class ProfileData
  {
    public int id = -1;
    public long createdAt = Epoch.Current();
    public long timePlayed = 0;
  }

  public class Profile<T> where T : ProfileData
  {
    public T data;

    public void Create(T data)
    {
      this.data = data;
      string fileName = $"{Application.persistentDataPath}/profile{data.id}.dat";
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Create(fileName);
      bf.Serialize(file, data);
      file.Close();
    }

    public void Save()
    {
      string fileName = $"{Application.persistentDataPath}/profile{data.id}.dat";
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Create(fileName);
      bf.Serialize(file, data);
      file.Close();
    }

    public void Load(int id)
    {
      string fileName = $"{Application.persistentDataPath}/profile{id}.dat";
      if (File.Exists(fileName))
      {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(fileName, FileMode.Open);
        data = (T)bf.Deserialize(file);
        file.Close();
      }
      else
      {
        data = null;
      }
    }

    public void Delete()
    {
      if (data != null && data.id != -1)
      {
        string fileName = $"{Application.persistentDataPath}/profile{data.id}.dat";
        if (File.Exists(fileName))
        {
          File.Delete(fileName);
        }
      }
    }

  }
}
