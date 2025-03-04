using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        PlayerWeapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<PlayerWeapon>();
        PlayerWeapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<PlayerWeapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            PlayerWeapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel); 
                
            }

        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }
    }

    private Sprite GetAmmoSprite(PlayerWeapon.WeaponModel model)
    {
        switch (model)
        {
            case PlayerWeapon.WeaponModel.Pistol_D:
                return Instantiate(Resources.Load<GameObject>("Pistol_Ammo")).GetComponent<SpriteRenderer>().sprite;
            case PlayerWeapon.WeaponModel.M4A1_AssaultRifle:
                return Instantiate(Resources.Load<GameObject>("Rifle_Ammo")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetWeaponSprite(PlayerWeapon.WeaponModel model)
    {
        switch (model)
        {
            case PlayerWeapon.WeaponModel.Pistol_D:
                return Instantiate(Resources.Load<GameObject>("Pistol_D_Weapon")).GetComponent<SpriteRenderer>().sprite;
            case PlayerWeapon.WeaponModel.M4A1_AssaultRifle:
                return Instantiate(Resources.Load<GameObject>("M4A1_AssaultRifle_Weapon")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }
}
