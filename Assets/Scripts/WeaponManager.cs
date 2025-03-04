using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    private PlayerInput playerInput;
    private InputAction switchSlot1Action;
    private InputAction switchSlot2Action;

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

        pickedUpWeapon.transform.localPosition = weapon.spawnPosition;
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation);

        weapon.isWeaponActive = true;
        weapon.animator.enabled = true;

        SetLayerRecursively(pickedUpWeapon, "Ignore Raycast"); // Switch layer to "ignore raycast" when picked up
    }


    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            PlayerWeapon weaponScript = weaponToDrop.GetComponent<PlayerWeapon>();

            weaponScript.isWeaponActive = false;
            weaponScript.animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;

            SetLayerRecursively(weaponToDrop, "Default"); // Switch layer back to "default" when dropped down
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

    // Function to set a gameObject and its children to a specific layer
    private void SetLayerRecursively(GameObject obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerName);
        }
        Physics.SyncTransforms();
    }

}
