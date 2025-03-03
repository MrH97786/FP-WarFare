using System.Collections;               
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set;}

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    private PlayerInput playerInput;
    private InputAction switchSlot1Action;
    private InputAction switchSlot2Action;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }

        // Initialize PlayerInput and actions
        playerInput = new PlayerInput();
        switchSlot1Action = playerInput.OnFoot.SwitchSlot1;
        switchSlot2Action = playerInput.OnFoot.SwitchSlot2;
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        // switch between weapons if key '1' or '2' is pressed
        if (switchSlot1Action.triggered)
        {
            SwitchActiveSlot(0);
        }
        if (switchSlot2Action.triggered)
        {
            SwitchActiveSlot(1);
        }
    }

    public void WeaponPickup(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon(pickedUpWeapon);

        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        PlayerWeapon weapon = pickedUpWeapon.GetComponent<PlayerWeapon>();

        pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isWeaponActive = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<PlayerWeapon>().isWeaponActive = false;
            weaponToDrop.GetComponent<PlayerWeapon>().animator.enabled = false;


            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            PlayerWeapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<PlayerWeapon>();
            currentWeapon.isWeaponActive = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            PlayerWeapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<PlayerWeapon>();
            newWeapon.isWeaponActive = true;
        }
    }

    // Enabling and Disabling key binds for weapon switching
    private void OnEnable()
    {
        switchSlot1Action.Enable();
        switchSlot2Action.Enable();
    }

    private void OnDisable()
    {
        switchSlot1Action.Disable();
        switchSlot2Action.Disable();
    }
}
