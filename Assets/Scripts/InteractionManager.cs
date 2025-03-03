using UnityEngine;
using UnityEngine.InputSystem; 

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    private PlayerInput playerInput;
    private InputAction pickupAction; 
    
    public PlayerWeapon hoveredOverWeapon = null;

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
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, interactionRange)) // Limit range
    {
        GameObject objectHitByRaycast = hit.transform.gameObject;

        if (objectHitByRaycast.TryGetComponent(out PlayerWeapon weapon))
        {
            // Highlight weapon when player is looking at it
            if (hoveredOverWeapon != weapon)
            {
                ResetWeaponHighlight();
                hoveredOverWeapon = weapon;
                hoveredOverWeapon.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            ResetWeaponHighlight();
        }
    }
    else
    {
        ResetWeaponHighlight();
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
