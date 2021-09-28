using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Sound/Sound Manager")]
  public class SoundManager : PersistentSingleton<SoundManager>
  {
    public Dictionary<string, SoundBase> SoundBases => _soundBases;
    private Dictionary<string, SoundBase> _soundBases = new Dictionary<string, SoundBase>();
    private Coroutine _playlistCoroutine;
    private bool _isPlaylistPlaying = false;

    protected override void Awake()
    {
      base.Awake();
      foreach (Transform child in transform)
      {
        SoundBase soundBase = child.GetComponent<SoundBase>();
        _soundBases.Add(child.name, soundBase);
      }
    }

    public void Play(SoundData[] sounds, bool fade = false)
    {
      foreach (SoundData sound in sounds)
      {
        _soundBases[sound.type.ToString()].Play(sound.clip.name, fade);
      }
    }

    public void StopAll(bool fade = false)
    {
      foreach (KeyValuePair<string, SoundBase> keyValuePair in _soundBases)
      {
        keyValuePair.Value.StopAll(fade);
      }
      if (_playlistCoroutine != null)
      {
        StopCoroutine(_playlistCoroutine);
      }
      PlaylistBase playlistBase = FindObjectOfType<PlaylistBase>();
      StopPlaylist(playlistBase);
    }

    private void OnEnable()
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
      if (_isPlaylistPlaying)
      {
        return;
      }
      _isPlaylistPlaying = true;
      PlaylistBase playlistBase = FindObjectOfType<PlaylistBase>();
      if (playlistBase != null)
      {
        if (_playlistCoroutine != null)
        {
          StopCoroutine(_playlistCoroutine);
        }
        _playlistCoroutine = StartCoroutine(StartPlaylist(playlistBase));
      }
    }

    public IEnumerator StartPlaylist(PlaylistBase playlistBase)
    {
      Playlist playlist = playlistBase.Playlist;
      if (playlist.Sounds.Length <= 0)
      {
        yield return null;
      }
      SoundData soundData = playlist.Sounds[playlist.CurrentIndex];
      SoundBase soundBase = _soundBases[soundData.type.ToString()];
      yield return new WaitUntil(() => soundBase.Loaded);
      while (true)
      {
        if (Application.isFocused && !_soundBases[soundData.type.ToString()].IsPlaying(soundData.name))
        {
          soundData = playlist.Sounds[playlist.CurrentIndex];
          soundBase = _soundBases[soundData.type.ToString()];
          yield return new WaitForSecondsRealtime(playlist.WaitForSeconds);
          yield return soundBase.GetAudioSource(soundData.name).FadeIn(playlist.FadeTime);
          if (++playlist.CurrentIndex == playlist.Sounds.Length)
          {
            playlist.CurrentIndex = 0;
          }
        }
        yield return null;
      }
    }

    public void StopPlaylist(PlaylistBase playlistBase)
    {
      _isPlaylistPlaying = false;
      if (playlistBase != null)
      {
        Playlist playlist = playlistBase.Playlist;
        if (playlist.Sounds.Length <= 0)
        {
          return;
        }
        SoundData[] sounds = playlist.Sounds;
        foreach (SoundData soundData in sounds)
        {
          SoundBase soundBase = _soundBases[soundData.type.ToString()];
          soundBase.GetAudioSource(soundData.name).FadeOut(playlist.FadeTime);
        }
      }
    }

  }
}