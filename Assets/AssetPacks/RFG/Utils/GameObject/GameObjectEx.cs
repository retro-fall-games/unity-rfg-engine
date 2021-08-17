using UnityEngine;

namespace RFG
{
  public static class GameObjectEx
  {
    public static bool CompareTags(this GameObject gameObject, string[] tags)
    {
      for (int i = 0; i < tags.Length; i++)
      {
        if (gameObject.CompareTag(tags[i]))
        {
          return true;
        }
      }
      return false;
    }

  }
}