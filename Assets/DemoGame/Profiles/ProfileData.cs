using System;

namespace Game
{

  [Serializable]
  public class ProfileData : RFG.ProfileData
  {
    public string level = "Intro";
    public int checkpoint = 0;
    public int[] weapons;
    public int[] ammoCounts;
    public int[] pickups;

  }

}