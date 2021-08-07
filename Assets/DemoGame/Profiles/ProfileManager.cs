using UnityEngine;
using RFG;

namespace Game
{
  [AddComponentMenu("Game/Profile/Profile Manager")]
  public class ProfileManager : PersistentSingleton<ProfileManager>
  {
    public Profile Profile;
  }
}