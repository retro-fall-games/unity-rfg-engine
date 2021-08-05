using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Scene/Scene Manager")]
  public class SceneManager : Singleton<SceneManager>
  {
    [Header("Cutscene")]
    public Cutscene OnStart;

    [Header("Change Scene")]
    public float WaitForSeconds = 1f;

    [Header("Unity Events")]
    public SceneUnityEvent OnSceneChanged;

    [HideInInspector]
    private string _lastScene;

    private void Start()
    {
      if (OnStart != null)
      {
        OnStart.Run();
      }
    }

    public void LoadScene(string name)
    {
      StartCoroutine(LoadSceneCo(name));
    }

    private IEnumerator LoadSceneCo(string name)
    {
      yield return new WaitForSecondsRealtime(WaitForSeconds);
      PlayerPrefs.SetString("lastScene", GetCurrentScene());
      OnSceneChanged?.Raise(name);
      UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public string GetCurrentScene()
    {
      return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public string GetLastScene()
    {
      return PlayerPrefs.GetString("lastScene");
    }

  }
}
