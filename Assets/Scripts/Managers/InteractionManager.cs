using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    private PlayerInput playerInput;
    private InputAction pickupAction;

    public PlayerWeapon hoveredOverWeapon = null;
    public AmmoBox hoveredOverAmmoBox = null;

    private bool justPickedUp = false; // ✅ Prevents immediate re-pickup

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

        // Initialize PlayerInput
        playerInput = new PlayerInput();
        pickupAction = playerInput.OnFoot.Pickup;
    }

    private void OnEnable()
    {
        pickupAction.Enable();
        pickupAction.performed += OnPickup;
    }

    private void OnDisable()
    {
        pickupAction.Disable();
        pickupAction.performed -= OnPickup;
    }

    private float interactionRange = 2f; // range for raycast

    private void Update()
    {
        if (hoveredOverWeapon != null && hoveredOverWeapon.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            ResetWeaponHighlight();
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // Weapon
            if (objectHitByRaycast.TryGetComponent(out PlayerWeapon weapon) && !weapon.isWeaponActive)
            {
                if (hoveredOverWeapon != weapon)
                {
                    ResetWeaponHighlight();
                    hoveredOverWeapon = weapon;
                    hoveredOverWeapon.GetComponent<Outline>().enabled = true;
                    Debug.Log("Ray hit: " + hit.transform.name);
                }

                if (pickupAction.triggered && !justPickedUp)
                {
                    justPickedUp = true;  // ✅ Set flag to prevent instant re-pickup
                    WeaponManager.Instance.WeaponPickup(hoveredOverWeapon.gameObject);
                    Invoke(nameof(ResetPickupFlag), 0.2f); // ✅ Reset flag after delay
                }
            }
            else if (hoveredOverWeapon)
            {
                ResetWeaponHighlight();
            }

            // Ammo box
            if (objectHitByRaycast.TryGetComponent(out AmmoBox ammoBox))
            {
                if (hoveredOverAmmoBox != ammoBox)
                {
                    if (hoveredOverAmmoBox)
                    {
                        hoveredOverAmmoBox.GetComponent<Outline>().enabled = false;
                    }

                    hoveredOverAmmoBox = ammoBox;
                    hoveredOverAmmoBox.GetComponent<Outline>().enabled = true;
                }


                if (pickupAction.triggered)
                {
                    WeaponManager.Instance.AmmoPickup(hoveredOverAmmoBox);
                }
            }
            else if (hoveredOverAmmoBox)
            {
                hoveredOverAmmoBox.GetComponent<Outline>().enabled = false;
                hoveredOverAmmoBox = null;
            }
        }
        else
        {
            // Reset highlights if looking away
            ResetWeaponHighlight();
            if (hoveredOverAmmoBox)
            {
                hoveredOverAmmoBox.GetComponent<Outline>().enabled = false;
                hoveredOverAmmoBox = null;
            }
        }
    }

    private void OnPickup(InputAction.CallbackContext context)
    {
        if (hoveredOverWeapon != null && !justPickedUp)
        {
            justPickedUp = true;
            WeaponManager.Instance.WeaponPickup(hoveredOverWeapon.gameObject);
            Invoke(nameof(ResetPickupFlag), 0.2f);
        }
    }

    private void ResetWeaponHighlight()
    {
        if (hoveredOverWeapon != null)
        {
            hoveredOverWeapon.GetComponent<Outline>().enabled = false;
            hoveredOverWeapon = null;
        }
    }

    private void ResetPickupFlag()
    {
        justPickedUp = false; // ✅ Allows switching weapons after a delay
    }
}
