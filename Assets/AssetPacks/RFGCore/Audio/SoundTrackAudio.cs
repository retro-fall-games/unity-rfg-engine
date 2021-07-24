namespace RFG
{
  public class SoundTrackAudio : BaseAudio<SoundTrackAudio>
  {
    protected override void Awake()
    {
      VolumeName = "soundTrackVolume";
      base.Awake();
    }
  }
}