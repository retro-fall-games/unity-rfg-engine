using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RFG.Platformer;

namespace Game
{
  public class HUDEquipped : MonoBehaviour
  {
    [Header("HUD")]
    public Image primaryEquipped;
    public Image secondaryEquipped;
    public TMP_Text primaryAmmo;
    public TMP_Text secondaryAmmo;

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
      // _weaponBehaviour.OnPrimaryEquip += OnPrimaryEquip;
      // _weaponBehaviour.OnSecondaryEquip += OnSecondaryEquip;
      // _weaponBehaviour.OnPrimaryFired += OnPrimaryFired;
      // _weaponBehaviour.OnSecondaryFired += OnSecondaryFired;
    }

    private void OnPrimaryEquip()
    {
      if (primaryEquipped != null)
      {
        primaryEquipped.sprite = _weaponBehaviour.PrimaryWeapon.pickupSprite;
        primaryAmmo.SetText(_weaponBehaviour.GetPrimaryAmmoCount().ToString());
      }
    }

    private void OnSecondaryEquip()
    {
      if (secondaryEquipped != null)
      {
        secondaryEquipped.sprite = _weaponBehaviour.SecondaryWeapon.pickupSprite;
        secondaryAmmo.SetText(_weaponBehaviour.GetSecondaryAmmoCount().ToString());
      }
    }

    private void OnPrimaryFired()
    {
      if (_weaponBehaviour.PrimaryWeapon != null)
      {
        primaryAmmo.SetText(_weaponBehaviour.GetPrimaryAmmoCount().ToString());
      }
    }

    private void OnSecondaryFired()
    {
      if (_weaponBehaviour.SecondaryWeapon != null)
      {
        secondaryAmmo.SetText(_weaponBehaviour.GetSecondaryAmmoCount().ToString());
      }
    }

  }
}