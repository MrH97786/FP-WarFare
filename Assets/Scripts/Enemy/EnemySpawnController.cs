using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnController : MonoBehaviour
{
    public int initialEnemiesPerWave = 5;
    public int currentEnemiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float coolDownCounter = 0;  // This is for testing purposes

    public List<Enemy> currentEnemiesAlive;

    public GameObject Enemy; // Must be assigned in the Inspector

    public TextMeshProUGUI titleWaveOver1; // first line of the title
    public TextMeshProUGUI titleWaveOver2; // second line of the title
    public TextMeshProUGUI coolDownCounterUI; // counter in the title

    private void Start()
    {
        currentEnemiesPerWave = initialEnemiesPerWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentEnemiesAlive.Clear();

        currentWave++;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentEnemiesPerWave; i++)
        {
            // Generate a random offset within a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Creating the enemy
            var enemyObject = Instantiate(Enemy, spawnPosition, Quaternion.identity);

            // Assign the nearest EnemyPath
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            enemyScript.enemyPath = FindClosestEnemyPath(spawnPosition);

            if (enemyScript.enemyPath == null)
            {
                Debug.LogError("No EnemyPath found near the spawned enemy.");
            }

            // Track the enemy
            currentEnemiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Function to find the closest EnemyPath
    private EnemyPath FindClosestEnemyPath(Vector3 position)
    {
        EnemyPath[] allPaths = FindObjectsOfType<EnemyPath>();
        EnemyPath closestPath = null;
        float closestDistance = Mathf.Infinity;

        foreach (EnemyPath path in allPaths)
        {
            float distance = Vector3.Distance(position, path.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPath = path;
            }
        }

        return closestPath;
    }


    private void Update()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (Enemy enemy in currentEnemiesAlive)
        {
            if (enemy.isDead)
            {
                enemiesToRemove.Add(enemy);
            }
        }

        foreach (Enemy enemy in enemiesToRemove)
        {
            currentEnemiesAlive.Remove(enemy);
        }

        enemiesToRemove.Clear();

        // Starting wave cooldown once every enemy is dead
        if (currentEnemiesAlive.Count == 0 && inCooldown == false)
        {
            //Start cooldown for next wave
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            coolDownCounter -= Time.deltaTime; // Run cooldown timer
        }
        else
        {
            coolDownCounter = waveCooldown; // Reset the counter
        }

        coolDownCounterUI.text = coolDownCounter.ToString("F1");

    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        // Show the "Wave Over" UI text
        titleWaveOver1.gameObject.SetActive(true);
        titleWaveOver2.gameObject.SetActive(true);

        // Wait for the cooldown duration
        yield return new WaitForSeconds(waveCooldown);

        // Hide the title after the cooldown is over
        titleWaveOver1.gameObject.SetActive(false);
        titleWaveOver2.gameObject.SetActive(false);

        // Reset wave state and prepare for the next wave
        currentEnemiesPerWave *= 2;
        StartNextWave();
    }



}
