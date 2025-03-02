using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    public bool isShooting;
    public bool readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // For weapon Burst
    public int bulletsPerBurst = 3;
    public int currentBurst;

    // For weapon Spread
    public float spreadIntensity;

    // Bullet
    public Transform bulletSpawn;
    public float bulletVelocity = 40;
    public float bulletLifeTime = 3f;
    private PlayerInput playerInput;
    private InputAction fireAction;

    public GameObject muzzleEffect;
    private Animator animator;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    void Awake()
    {
        // Initialize the PlayerInput and get the Fire action from the OnFoot input map
        playerInput = new PlayerInput();
        fireAction = playerInput.OnFoot.Fire;

        readyToShoot = true;
        currentBurst = bulletsPerBurst;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the Fire action state
        bool firePressed = fireAction.ReadValue<float>() > 0;  // InputSystem uses float values (0 or 1) for actions

        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = firePressed;  // Holding down button
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = fireAction.triggered; // Single press
        }

        if (readyToShoot && isShooting)
        {
            currentBurst = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play(); // Muzzle effect
        animator.SetTrigger("RECOIL"); // Recoil animation being set when weapon gets fired
        SoundManager.Instance.shootingSoundPistol_D.Play();

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // Create new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        // Shoot the bullet and add force to the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Destroy bullet after a couple seconds
        StartCoroutine(DestroyBullet(bullet, bulletLifeTime));

        // Quick check when done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst Mode
        if (currentShootingMode == ShootingMode.Burst && currentBurst > 1)
        {
            currentBurst--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else 
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    private void OnEnable()
    {
        fireAction.Enable();
    }

    private void OnDisable()
    {
        fireAction.Disable();
    }
}
