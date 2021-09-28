using UnityEngine;
using UnityEditor;

namespace RFG.Core
{
  [CustomEditor(typeof(ObjectPoolManager))]
  [CanEditMultipleObjects]
  public class ObjectPoolManagerInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      DrawDefaultInspector();

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Object Pools", EditorStyles.boldLabel);
      EditorGUILayout.HelpBox("This will create children object pools", MessageType.Warning, true);
      if (GUILayout.Button("Generate Object Pools"))
      {
        GenerateObjectPools();
      }
      EditorGUILayout.HelpBox("This will create all children objects in their respective pools", MessageType.Warning, true);
      if (GUILayout.Button("Generate All Objects"))
      {
        GenerateAllObjects();
      }

      serializedObject.ApplyModifiedProperties();

    }

    private void GenerateObjectPools()
    {
      ObjectPoolManager manager = (ObjectPoolManager)target;

      foreach (string category in manager.ObjectPoolCategories)
      {
        Transform child = manager.transform.Find(category);
        if (child == null)
        {
          GameObject obj = new GameObject(category);
          obj.AddComponent<ObjectPool>();
          obj.gameObject.transform.SetParent(manager.gameObject.transform);
        }
      }
      EditorUtility.SetDirty(target);
    }

    private void GenerateAllObjects()
    {
      ObjectPoolManager manager = (ObjectPoolManager)target;

      foreach (Transform child in manager.transform)
      {
        ObjectPool pool = child.GetComponent<ObjectPool>();
        pool.GenerateObjects();
      }
      EditorUtility.SetDirty(target);
    }

  }

}