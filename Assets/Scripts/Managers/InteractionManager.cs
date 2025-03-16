using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    private PlayerInput playerInput;
    private InputAction pickupAction;
    private InputAction interactAction;

    public PlayerWeapon hoveredOverWeapon = null;
    public AmmoBox hoveredOverAmmoBox = null;

    private bool justPickedUp = false;


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
        interactAction = playerInput.OnFoot.Interact;
    }

    private void OnEnable()
    {
        pickupAction.Enable();
        interactAction.Enable();

        pickupAction.performed += OnPickup;
        interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        pickupAction.Disable();
        interactAction.Disable();

        pickupAction.performed -= OnPickup;
        interactAction.performed -= OnInteract;
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
                    justPickedUp = true;
                    WeaponManager.Instance.WeaponPickup(hoveredOverWeapon.gameObject);
                    Invoke(nameof(ResetPickupFlag), 0.2f);
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

                // Don't call interactAction.triggered here anymore
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

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (hoveredOverAmmoBox != null)
        {
            int cost = hoveredOverAmmoBox.ammoCost;  // Get the cost from the specific AmmoBox

            // Check if the player's gun is already full
            PlayerWeapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.transform.GetChild(0).GetComponent<PlayerWeapon>();
            bool isGunFull = false;

            switch (activeWeapon.thisWeaponModel)
            {
                case PlayerWeapon.WeaponModel.Pistol_D:
                    if (WeaponManager.Instance.totalPistolAmmo >= WeaponManager.Instance.maxPistolAmmo)
                    {
                        isGunFull = true;
                    }
                    break;

                case PlayerWeapon.WeaponModel.M4A1_AssaultRifle:
                    if (WeaponManager.Instance.totalRifleAmmo >= WeaponManager.Instance.maxRifleAmmo)
                    {
                        isGunFull = true;
                    }
                    break;
            }

            if (isGunFull)
            {
                Debug.Log("Gun is full. Cannot pick up more ammo.");
                return; // Prevent interaction with ammo box if the gun is full
            }

            // Check if the player has enough points
            if (ScoreManager.instance.HasEnoughPoints(cost))
            {
                ScoreManager.instance.DeductPoints(cost);
                WeaponManager.Instance.AmmoPickup(hoveredOverAmmoBox);
                Debug.Log("Ammo box used! -" + cost + " points deducted.");
            }
            else
            {
                Debug.Log("Not enough points to use the Ammo Box!");
            }
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
        justPickedUp = false;
    }
}
