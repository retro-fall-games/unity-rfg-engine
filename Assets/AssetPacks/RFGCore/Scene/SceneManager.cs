using System.Collections;
using UnityEngine;
using RFGFx;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Scene/Scene Manager")]
  public class SceneManager : Singleton<SceneManager>
  {

    [Header("Scene Settings")]
    public Cutscene onStart;
    public bool fadeInOnStart = false;

    [HideInInspector]
    public bool Loaded { get; set; }
    private string _lastScene;

    private void Start()
    {
      Loaded = false;
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      // Wait until everything is loaded
      yield return new WaitUntil(() => GameManager.Instance != null);
      yield return new WaitUntil(() => Transition.Instance != null);
      if (onStart != null)
      {
        onStart.Run();
      }
      if (fadeInOnStart)
      {
        StartCoroutine(FadeInOnStart());
      }

      Loaded = true;

      yield break;
    }

    public void LoadScene(string name)
    {
      LoadScene(name, 3f);
    }

    public void LoadScene(string name, float waitForSeconds = 3f)
    {
      StartCoroutine(LoadSceneCo(name, waitForSeconds));
    }

    private IEnumerator LoadSceneCo(string name, float waitForSeconds)
    {
      Transition.Instance.Show("CrossFade", "Start");
      yield return new WaitForSeconds(waitForSeconds);
      PlayerPrefs.SetString("lastScene", GetCurrentScene());
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

    public IEnumerator FadeInOnStart()
    {
      yield return new WaitUntil(() => Transition.Instance != null);
      Transition.Instance.Show("CrossFade", "End");
    }

  }
}
