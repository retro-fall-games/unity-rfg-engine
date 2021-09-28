using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.SceneGraph
{
  using Core;

  [AddComponentMenu("RFG/Scene/Scene Manager")]
  public class SceneManager : Singleton<SceneManager>
  {
    [Header("Cutscene")]
    public Cutscene OnStart;

    [Header("Change Scene")]
    public float WaitForSeconds = 1f;

    [Header("Transition")]
    public string TransitionName;

    [Header("Event Observer")]
    public ObserverString SceneChangeEvent;

    [HideInInspector]
    public Transition Transition => _transition;

    private string _lastScene;
    private Transition _transition;

    protected override void Awake()
    {
      base.Awake();
      _transition = GetComponent<Transition>();
    }

    private void Start()
    {
      if (OnStart != null)
      {
        OnStart.Run();
      }
    }

    public void LoadFromSceneNode(SceneNode sceneNode)
    {
      LoadScene(sceneNode.SceneName);
    }

    public void LoadScene(string name)
    {
      if (Transition != null && !TransitionName.Equals(""))
      {
        Transition.Start(TransitionName);
      }
      StartCoroutine(LoadSceneCo(name));
    }

    private IEnumerator LoadSceneCo(string name)
    {
      yield return new WaitForSecondsRealtime(WaitForSeconds);
      PlayerPrefs.SetString("lastScene", GetCurrentScene());
      GameManager.Instance.UnPause();

      // The Application loads the Scene in the background as the current Scene runs.
      // This is particularly good for creating loading screens.
      AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);

      // Wait until the asynchronous scene fully loads
      while (!asyncLoad.isDone)
      {
        yield return null;
      }
    }

    public string GetCurrentScene()
    {
      return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public string GetLastScene()
    {
      return PlayerPrefs.GetString("lastScene");
    }

    private void OnEnable()
    {
      SceneChangeEvent.OnRaise += LoadScene;
    }

    private void OnDisable()
    {
      SceneChangeEvent.OnRaise -= LoadScene;
    }

    public static List<string> GetAllScenes()
    {
      var list = new List<string>();

      for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; ++i)
      {
        string name = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        list.Add(name);
      }

      return list;
    }
  }
}