using UnityEngine;
using RFG;
using TMPro;

namespace Game
{
  [AddComponentMenu("Game/Profile/Profile Panel")]
  public class ProfilePanel : MonoBehaviour
  {
    [Header("Settings")]
    public Profile profile;
    public TMP_Text headerText;
    public TMP_Text startText;
    public GameObject deleteButton;

    private void Start()
    {
      if (profile.createdAt > 0)
      {
        profile.Load();
      }
      SetUI();
    }

    private void SetUI()
    {
      headerText.SetText($"Profile {profile.id + 1}");
      if (profile.timePlayed > 0)
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
      if (profile.createdAt == 0)
      {
        profile.Create();
      }
      ProfileManager.Instance.Profile = profile;
      SoundManager.Instance.StopAll(true);
      SceneManager.Instance.LoadScene(profile.level);
    }

    public void DeleteProfile()
    {
      profile.Delete();
      SetUI();
    }
  }
}