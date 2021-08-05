using System.Collections;
using UnityEngine;
using RFG;
using TMPro;

namespace Game
{
  public class ProfilePanel : MonoBehaviour
  {
    [Header("Settings")]
    public int id;
    public TMP_Text headerText;
    public TMP_Text startText;
    public GameObject deleteButton;

    [Header("Scene Unity Events")]
    public SceneUnityEvent OnSceneChange;

    [HideInInspector]
    private Profile<Game.ProfileData> _profile;

    private void Awake()
    {
      _profile = new Profile<Game.ProfileData>();
    }

    private void Start()
    {
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      yield return new WaitUntil(() => Game.ProfileManager.Instance != null);
      LoadPanel();
    }

    private void LoadPanel()
    {
      _profile.Load(id);
      headerText.SetText($"Profile {id + 1}");
      if (_profile.data != null)
      {
        deleteButton.SetActive(true);
        startText.SetText("Continue");
      }
      else
      {
        deleteButton.SetActive(false);
        startText.SetText("Create");
      }
    }

    public void StartGame()
    {
      if (_profile.data != null)
      {
        Game.ProfileManager.Instance.SetProfile(_profile);
        PlayerPrefs.SetInt("startingCheckpoint", _profile.data.checkpoint);
      }
      else
      {
        Game.ProfileData data = new Game.ProfileData();
        data.id = id;
        _profile.Create(data);
        Game.ProfileManager.Instance.SetProfile(_profile);
        PlayerPrefs.SetInt("startingCheckpoint", 0);
      }
      OnSceneChange?.Raise(_profile.data.level);
    }

    public void DeleteProfile()
    {
      _profile.Delete();
      LoadPanel();
    }
  }
}