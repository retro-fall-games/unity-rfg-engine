using System.Collections.Generic;
using UnityEngine;
using MyBox;
using RFG;
using RFG.Platformer;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
  [CreateAssetMenu(fileName = "New Resource Index", menuName = "Game/Resource Index")]
  public class ResourceIndex : ScriptableObject, IResourceIndex
  {
    public List<ResourceAsset> Assets;

    public List<ResourceAsset> GetAssets()
    {
      return Assets;
    }

#if UNITY_EDITOR
    private static Dictionary<string, System.Type> typesToIndex = new Dictionary<string, System.Type>
    {
        { "Consumables", typeof(ScriptableObject) },
        { "Equipables", typeof(ScriptableObject) },
        { "Abilities", typeof(ScriptableObject) },
        { "Behaviours", typeof(ScriptableObject) },
        { "States", typeof(ScriptableObject) },
    };

    [ButtonMethod]
    private void BuildMap()
    {
      Assets = new List<ResourceAsset>();
      foreach (var kvp in typesToIndex)
      {
        var all = Resources.LoadAll(kvp.Key, kvp.Value);
        for (int i = 0; i < all.Length; i++)
        {
          ScriptableObject o = (ScriptableObject)all[i];
          if (o is Item item)
          {
            ResourceAsset asset = new ResourceAsset();
            asset.Guid = item.Guid;
            asset.Asset = item;
            Assets.Add(asset);
          }
          // else if (o is CharacterAbility ability)
          // {
          //   ResourceAsset asset = new ResourceAsset();
          //   asset.Guid = ability.Guid;
          //   asset.Asset = ability;
          //   Assets.Add(asset);
          // }
          // else if (o is CharacterState state)
          // {
          //   ResourceAsset asset = new ResourceAsset();
          //   asset.Guid = state.Guid;
          //   asset.Asset = state;
          //   Assets.Add(asset);
          // }
        }
      }
      EditorUtility.SetDirty(this);
    }
#endif

  }

}