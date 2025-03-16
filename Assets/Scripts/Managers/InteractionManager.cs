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

                if (interactAction.triggered) 
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

    private void OnInteract(InputAction.CallbackContext context) // New method for AmmoBox interaction
    {
        if (hoveredOverAmmoBox != null)
        {
            WeaponManager.Instance.AmmoPickup(hoveredOverAmmoBox);
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
