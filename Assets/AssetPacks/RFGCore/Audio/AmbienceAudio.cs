namespace RFG
{
  public class AmbienceAudio : BaseAudio<AmbienceAudio>
  {
    protected override void Awake()
    {
      VolumeName = "ambienceVolume";
      base.Awake();
    }
  }
}