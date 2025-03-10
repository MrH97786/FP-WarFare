using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    private PlayerInput playerInput;
    private InputAction pickupAction;

    public PlayerWeapon hoveredOverWeapon = null;
    public AmmoBox hoveredOverAmmoBox = null;

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

    //private float interactionRange = 2f; // range for raycast

    private void Update()
    {
        if (hoveredOverWeapon != null && hoveredOverWeapon.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            ResetWeaponHighlight();
            hoveredOverWeapon = null;
            return; 
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) 
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // Weapon
            if (objectHitByRaycast.GetComponent<PlayerWeapon>() && objectHitByRaycast.GetComponent<PlayerWeapon>().isWeaponActive == false)
            {
                hoveredOverWeapon = objectHitByRaycast.GetComponent<PlayerWeapon>();
                hoveredOverWeapon.GetComponent<Outline>().enabled = true;
                Debug.Log("Ray hit: " + hit.transform.name);

                if (pickupAction.triggered)
                {
                    WeaponManager.Instance.WeaponPickup(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredOverWeapon)
                {
                    hoveredOverWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            // Ammo box
            if (objectHitByRaycast.GetComponent<AmmoBox>())
            {
                hoveredOverAmmoBox = objectHitByRaycast.GetComponent<AmmoBox>();
                hoveredOverAmmoBox.GetComponent<Outline>().enabled = true;

                if (pickupAction.triggered)
                {
                    WeaponManager.Instance.AmmoPickup(hoveredOverAmmoBox); // Refill ammo on any weapon
                }
            }
            else
            {
                if (hoveredOverAmmoBox)
                {
                    hoveredOverAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }

    private void OnPickup(InputAction.CallbackContext context)
    {
        if (hoveredOverWeapon != null)
        {
            WeaponManager.Instance.WeaponPickup(hoveredOverWeapon.gameObject);
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
}
