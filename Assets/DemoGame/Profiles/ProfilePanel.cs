using System;
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
    public TMP_Text timePlayedText;
    public TMP_Text startText;
    public GameObject deleteButton;

    private void Start()
    {
      profile.Load();
      SetUI();
    }

    private void SetUI()
    {
      headerText.SetText($"Profile {profile.Id}");
      if (profile.TimePlayed > 0)
      {
        TimeSpan time = TimeSpan.FromSeconds(profile.TimePlayed);
        timePlayedText.SetText(time.ToString(@"hh\:mm\:ss"));
        deleteButton.SetActive(true);
        startText.SetText("Continue");
      }
      else
      {
        timePlayedText.SetText("--:--");
        deleteButton.SetActive(false);
        startText.SetText("Create");
      }
    }

    public void StartGame()
    {
      if (profile.CreatedAt == 0)
      {
        profile.Create();
      }
      // TODO - instead have a cutscene end, where you can fade out music or keep it playing
      SoundManager.Instance.StopAll(true);
      ProfileManager.Instance.SelectProfile(profile);
      SceneManager.Instance.LoadScene(profile.Level);
    }

    public void DeleteProfile()
    {
      profile.Delete();
      SetUI();
    }
  }
}