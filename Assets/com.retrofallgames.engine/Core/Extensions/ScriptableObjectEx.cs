using System;
using UnityEngine;

namespace RFG
{
  public static class ScriptableObjectEx
  {
    public static T CreateNewInstance<T>(this ScriptableObject obj) where T : ScriptableObject
    {
      Type type = obj.GetType();
      T newObj = (T)ScriptableObject.CreateInstance(type);
      newObj.name = type.ToString();
      return newObj;
    }

    public static T CloneInstance<T>(this ScriptableObject obj) where T : ScriptableObject
    {
      return ScriptableObject.Instantiate(obj) as T;
    }
  }
}