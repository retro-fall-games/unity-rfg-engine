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

    [Header("Transition")]
    public string TransitionName;

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

  }
}
