using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace RFG.Core
{
  [CustomEditor(typeof(SoundManager))]
  [CanEditMultipleObjects]
  public class SoundManagerInspector : Editor
  {
    private SoundManager soundManager;
    private GameObject gameObject;
    private Transform transform;

    private void OnEnable()
    {
      soundManager = (SoundManager)target;
      gameObject = soundManager.gameObject;
      transform = gameObject.transform;
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      DrawDefaultInspector();

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Sound Manager Setup", EditorStyles.boldLabel);
      if (GUILayout.Button("Add Sub Managers"))
      {
        AddSubManagers();
      }
      if (GUILayout.Button("Add All Sounds"))
      {
        AddAllSounds();
      }
      if (GUILayout.Button("Configure All Audio Sources"))
      {
        ConfigureSubManagersAudioSources();
      }

      serializedObject.ApplyModifiedProperties();

    }

    private SoundBaseSettings[] GetSoundBaseSettings()
    {
      Dictionary<string, Transform> settings = new Dictionary<string, Transform>();
      string[] guids = AssetDatabase.FindAssets("t:" + typeof(SoundBaseSettings).Name);
      SoundBaseSettings[] soundBaseSettings = new SoundBaseSettings[guids.Length];
      for (int i = 0; i < guids.Length; i++)
      {
        string path = AssetDatabase.GUIDToAssetPath(guids[i]);
        soundBaseSettings[i] = AssetDatabase.LoadAssetAtPath<SoundBaseSettings>(path);
      }
      return soundBaseSettings;
    }

    private Dictionary<string, Transform> GetSubManagers()
    {
      Dictionary<string, Transform> settings = new Dictionary<string, Transform>();
      SoundBaseSettings[] soundBaseSettings = GetSoundBaseSettings();
      for (int i = 0; i < soundBaseSettings.Length; i++)
      {
        Transform soundSettingsTransform = transform.Find(soundBaseSettings[i].name);
        if (soundSettingsTransform != null)
        {
          settings.Add(soundBaseSettings[i].name, soundSettingsTransform);
        }
      }
      return settings;
    }

    private void AddSubManagers()
    {
      SoundBaseSettings[] soundBaseSettings = GetSoundBaseSettings();
      Dictionary<string, Transform> subManagers = GetSubManagers();
      for (int i = 0; i < soundBaseSettings.Length; i++)
      {
        if (!subManagers.ContainsKey(soundBaseSettings[i].name))
        {
          GameObject soundSubManager = new GameObject(soundBaseSettings[i].name);
          SoundBase soundBase = soundSubManager.AddComponent<SoundBase>();
          soundSubManager.gameObject.transform.SetParent(transform);
          soundBase.Settings = soundBaseSettings[i];
        }
      }
      EditorUtility.SetDirty(target);
    }

    private void AddAllSounds()
    {

      Dictionary<string, Transform> subManagers = GetSubManagers();

      string[] guids = AssetDatabase.FindAssets("t:" + typeof(SoundData).Name);
      for (int i = 0; i < guids.Length; i++)
      {
        string path = AssetDatabase.GUIDToAssetPath(guids[i]);
        SoundData soundData = AssetDatabase.LoadAssetAtPath<SoundData>(path);
        if (subManagers.ContainsKey(soundData.type.ToString()))
        {
          AddAudioSource(subManagers[soundData.type.ToString()], soundData);
        }
      }
      EditorUtility.SetDirty(target);
    }

    private void AddAudioSource(Transform parent, SoundData soundData)
    {
      if (!parent.Find(soundData.clip.name))
      {
        GameObject soundTrack = new GameObject(soundData.clip.name);
        Sound sound = soundTrack.AddComponent<Sound>();
        soundTrack.tag = soundData.type.ToString();
        soundTrack.gameObject.transform.SetParent(parent);
        sound.soundData = soundData;
        sound.GenerateAudioData();
      }
    }

    public void ConfigureSubManagersAudioSources()
    {
      foreach (Transform child in transform)
      {
        SoundBase soundBase = child.GetComponentInChildren<SoundBase>();
        soundBase.ConfigureAudioSources();
      }
      EditorUtility.SetDirty(target);
    }
  }

}