namespace RFG.Interactions
{
  public struct LevelPortalEvent
  {
    public string toScene;
    public int toLevelPortalIndex;
    public bool fadeSoundtrack;
    public float waitForSeconds;
    public LevelPortalEvent(string toScene, int toLevelPortalIndex, bool fadeSoundtrack, float waitForSeconds)
    {
      this.toScene = toScene;
      this.toLevelPortalIndex = toLevelPortalIndex;
      this.fadeSoundtrack = fadeSoundtrack;
      this.waitForSeconds = waitForSeconds;
    }
  }
}