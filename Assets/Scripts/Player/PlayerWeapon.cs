using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

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
    private InputAction reloadAction;

    public GameObject muzzleEffect;
    private Animator animator;

    // Weapon Loading
    public float reloadTime;
    public int magazineSize;
    public int bulletsLeft; // Made this public for debugging purposes (can make private later, though unneccesary)
    public bool isReloading;

    

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    void Awake()
    {
        // Initialize PlayerInput and actions
        playerInput = new PlayerInput();
        fireAction = playerInput.OnFoot.Fire;
        reloadAction = playerInput.OnFoot.Reload;

        // Register reload event
        reloadAction.performed += ctx => AttemptReload();

        readyToShoot = true;
        currentBurst = bulletsPerBurst;

        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSound.Play(); //Play empty magazine sound when no bullets and player tries clicking mouse
        }

        // Read the Fire action state
        bool firePressed = fireAction.ReadValue<float>() > 0; // InputSystem uses float values (0 or 1) for actions

        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = firePressed; // Holding down button
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = fireAction.triggered; // Single press
        }

        // Auto-reload when magazine is empty
        if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
        {
            Reload();
        }

        // Handle shooting logic
        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            currentBurst = bulletsPerBurst;
            FireWeapon();
        }

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
        }
    }

    private void FireWeapon()
    {
        // Effects, animation, and sounds
        muzzleEffect.GetComponent<ParticleSystem>().Play(); // Muzzle effect
        animator.SetTrigger("RECOIL"); // Recoil animation
        SoundManager.Instance.shootingSoundPistol_D.Play(); // Bullet shot sound

        bulletsLeft--;

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // Create new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        // Shoot the bullet and add force
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Destroy bullet after some time
        StartCoroutine(DestroyBullet(bullet, bulletLifeTime));

        // Quick check when done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst Mode handling
        if (currentShootingMode == ShootingMode.Burst && currentBurst > 1)
        {
            currentBurst--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void AttemptReload()
    {
        if (bulletsLeft < magazineSize && !isReloading)
        {
            Reload();
        }
    }

    private void Reload()
    {
        SoundManager.Instance.reloadingSound.Play(); // Play reloading sound when reloading the gun
        animator.SetTrigger("RELOAD");

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
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
        reloadAction.Enable();
    }

    private void OnDisable()
    {
        fireAction.Disable();
        reloadAction.Disable();
    }
}
