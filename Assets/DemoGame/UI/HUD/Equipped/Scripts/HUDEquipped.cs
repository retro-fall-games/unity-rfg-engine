using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RFG;

namespace Game
{
  public class HUDEquipped : MonoBehaviour
  {
    [Header("HUD")]
    public Image primaryEquipped;
    public Image secondaryEquipped;

    private GameObject _player;
    private WeaponBehaviour _weaponBehaviour;

    private void Start()
    {
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
      _player = GameObject.FindGameObjectWithTag("Player");
      _weaponBehaviour = _player.GetComponent<WeaponBehaviour>();
      _weaponBehaviour.OnPrimaryEquip += OnPrimaryEquip;
      _weaponBehaviour.OnSecondaryEquip += OnSecondaryEquip;
    }

    private void OnPrimaryEquip()
    {
      if (primaryEquipped != null)
      {
        primaryEquipped.sprite = _weaponBehaviour.PrimaryWeapon.pickupSprite;
      }
    }

    private void OnSecondaryEquip()
    {
      if (secondaryEquipped != null)
      {
        secondaryEquipped.sprite = _weaponBehaviour.SecondaryWeapon.pickupSprite;
      }
    }

  }
}