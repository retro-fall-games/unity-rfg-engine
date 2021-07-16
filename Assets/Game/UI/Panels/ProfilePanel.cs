using System.Collections;
using UnityEngine;
using RFG;
using TMPro;

public class ProfilePanel : MonoBehaviour
{
  public int id;
  public TMP_Text headerText;
  public TMP_Text startText;
  public GameObject deleteButton;
  private Profile _profile;

  private void Awake()
  {
    _profile = new Profile();
  }

  private void Start()
  {
    StartCoroutine(StartCo());
  }

  private IEnumerator StartCo()
  {
    yield return new WaitUntil(() => ProfileManager.Instance != null);
    yield return new WaitUntil(() => SceneManager.Instance != null);
    LoadPanel();
  }

  private void LoadPanel()
  {
    _profile.Load(id);
    headerText.SetText($"Profile {id + 1}");
    if (_profile.id > -1)
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
    if (_profile.id > -1)
    {
      ProfileManager.Instance.SetProfile(_profile);
    }
    else
    {
      _profile.id = id;
      _profile.Save();
      ProfileManager.Instance.SetProfile(_profile);
    }
    SceneManager.Instance.LoadScene("Scene1");
  }

  public void DeleteProfile()
  {
    if (_profile.id > -1)
    {
      _profile.Delete();
      LoadPanel();
    }
  }
}
