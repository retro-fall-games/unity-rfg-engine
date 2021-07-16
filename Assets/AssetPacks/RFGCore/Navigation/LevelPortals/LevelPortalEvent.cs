namespace RFG
{
  public struct LevelPortalEvent
  {
    public string toScene;
    public int toLevelPortalIndex;
    public float waitForSeconds;
    public LevelPortalEvent(string toScene, int toLevelPortalIndex, float waitForSeconds)
    {
      this.toScene = toScene;
      this.toLevelPortalIndex = toLevelPortalIndex;
      this.waitForSeconds = waitForSeconds;
    }
  }
}