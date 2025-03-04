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

    // Store sprites to prevent redundant instantiation
    private Sprite pistolAmmoSprite;
    private Sprite rifleAmmoSprite;
    private Sprite pistolWeaponSprite;
    private Sprite rifleWeaponSprite;

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

        // Pre-load sprites
        pistolAmmoSprite = Resources.Load<Sprite>("Pistol_Ammo");
        rifleAmmoSprite = Resources.Load<Sprite>("Rifle_Ammo");
        pistolWeaponSprite = Resources.Load<Sprite>("Pistol_D_Weapon");
        rifleWeaponSprite = Resources.Load<Sprite>("M4A1_AssaultRifle_Weapon");
    }

    private void Update()
    {
        PlayerWeapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<PlayerWeapon>();
        PlayerWeapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<PlayerWeapon>();

        if (activeWeapon)
        {
            // Update ammo UI
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";
            ammoTypeUI.sprite = GetAmmoSprite(activeWeapon.thisWeaponModel);

            // Update weapon UI
            activeWeaponUI.sprite = GetWeaponSprite(activeWeapon.thisWeaponModel);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            // Reset UI when no active weapon
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
                return pistolAmmoSprite; // Use pre-loaded sprite
            case PlayerWeapon.WeaponModel.M4A1_AssaultRifle:
                return rifleAmmoSprite; // Use pre-loaded sprite
            default:
                return null;
        }
    }

    private Sprite GetWeaponSprite(PlayerWeapon.WeaponModel model)
    {
        switch (model)
        {
            case PlayerWeapon.WeaponModel.Pistol_D:
                return pistolWeaponSprite; // Use pre-loaded sprite
            case PlayerWeapon.WeaponModel.M4A1_AssaultRifle:
                return rifleWeaponSprite; // Use pre-loaded sprite
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
