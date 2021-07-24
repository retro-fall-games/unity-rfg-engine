namespace RFG
{
  public class FXAudio : BaseAudio<FXAudio>
  {
    protected override void Awake()
    {
      VolumeName = "fxVolume";
      base.Awake();
    }
  }
}