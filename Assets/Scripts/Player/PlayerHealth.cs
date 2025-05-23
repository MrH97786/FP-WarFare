using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health;
    private float lerpTimer;
    public GameObject gameOverUI;
    public bool isDead;  // Add a flag to check if the player is dead

    [Header("Health Bar")]
    public float maxHealth = 300;
    public float barDelay = 2f; // speed of the delay bar takes to catch up to health lost
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float damageDuration;
    public float damageFade;
    private float durationTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveLoadManager.Instance.cachedHealth > 0)
        {
            health = SaveLoadManager.Instance.cachedHealth;
            SaveLoadManager.Instance.cachedHealth = -1f; // Reset after applying
        }
        else
        {
            health = maxHealth;
        }
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
        lerpTimer = 0f;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (health <= 0 && !isDead)  // Only call PlayerDead if the player is not already dead
        {
            PlayerDead();  // Call the PlayerDead method when health is 0 or lower
            isDead = true;  // Set the isDead flag to true
        }

        if (damageOverlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if (durationTimer > damageDuration)
            {
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * damageFade;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
            }
        }
    }

    // Getter and Seter 
    public float Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, maxHealth);
    }



    public void UpdateHealthUI()
    {
        //Debug.Log(health); //console log for health 
        float fillFrontH = frontHealthBar.fillAmount;
        float fillBackH = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;
        if (fillBackH > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / barDelay;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBackH, healthFraction, percentComplete);
        }
        if (fillFrontH < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / barDelay;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFrontH, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead == false)
        {
            health -= damage;
            lerpTimer = 0f;
            durationTimer = 0;
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);

            // Play the hurt sound when taking damage
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    public void HealthRestore()
    {
        int healCost = 200; // Cost to heal

        if (health >= maxHealth) // Prevent interaction if health is already full
        {
            Debug.Log("Health is already full. Cannot restore.");
            return;
        }

        if (ScoreManager.instance.HasEnoughPoints(healCost)) // Check if the player has enough points
        {
            ScoreManager.instance.DeductPoints(healCost); // Deduct points
            health = maxHealth; // Set health to full
            lerpTimer = 0f;
            Debug.Log("Health restored! -" + healCost + " points deducted.");
        }
        else
        {
            Debug.Log("Not enough points to restore health!");
        }
    }



    private void PlayerDead()
    {
        // Play the death sound only once
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);

        GetComponent<PlayerMovement>().SetDead(true);

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInteract>().enabled = false;
        GetComponent<CharacterController>().enabled = false;

        // Debug.Log("Player has died!");

        // Dying animation
        GetComponentInChildren<Animator>().enabled = true;

        GetComponent<PlayerScreenBlackout>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);

        int waveSurvived = GlobalReferences.Instance.waveNumber;
        int highestPoints = GlobalReferences.Instance.scoreNumber;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore(currentSceneIndex))
        {
            SaveLoadManager.Instance.SaveHighScore(currentSceneIndex, waveSurvived - 1);
        }

        if (highestPoints > SaveLoadManager.Instance.LoadHighPointScore(currentSceneIndex))
        {
            SaveLoadManager.Instance.SaveHighPointScore(currentSceneIndex, highestPoints);
        }

        StartCoroutine(ReturnToMainMenu());
    }


    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(4f);

        // Clear save data
        string path = Application.persistentDataPath + "/playerdata.json";
        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

        SaveLoadManager.Instance.cachedHealth = -1f;
        SaveLoadManager.Instance.cachedWave = -1;
        SaveLoadManager.Instance.cachedScore = -1;

        GlobalReferences.Instance.waveNumber = 1;
        GlobalReferences.Instance.scoreNumber = 0;

        SceneManager.LoadScene("MainMenu");
    }

}
