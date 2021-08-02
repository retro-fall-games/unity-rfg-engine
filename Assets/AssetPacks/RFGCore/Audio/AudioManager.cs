namespace RFG
{
  public class AudioManager : PersistentSingleton<AudioManager>
  {

    public void StopAll(bool fade = false)
    {
      SoundTrackAudio.Instance.StopAll(fade);
      AmbienceAudio.Instance.StopAll(fade);
    }

  }
}