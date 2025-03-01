using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    public Transform bulletSpawn;
    public float bulletVelocity = 40;
    public float bulletLifeTime = 3f;
    private PlayerInput playerInput;
    private InputAction fireAction;

    void Awake()
    {
        // Initialize the PlayerInput and get the Fire action from the OnFoot input map
        playerInput = new PlayerInput();
        fireAction = playerInput.OnFoot.Fire;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireAction.triggered)
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        // Create new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, bulletSpawn.position, Quaternion.identity);

        // Shoot the bullet and add force to the bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);

        // Destroy bullet after a couple seconds
        StartCoroutine(DestroyBullet(bullet, bulletLifeTime));
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
