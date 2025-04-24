using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnController : MonoBehaviour
{
    public int initialEnemiesPerWave = 2;
    public int currentEnemiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCoolDownTimer = 10.0f;

    public bool inCoolDown;
    public float coolDownCounter = 0;

    public List<Enemy> currentEnemiesAlive = new List<Enemy>();

    public GameObject Enemy;

    public TextMeshProUGUI titleWaveOver1;
    public TextMeshProUGUI titleWaveOver2;
    public TextMeshProUGUI coolDownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        // Load wave number from save
        if (SaveLoadManager.Instance.cachedWave != -1)
        {
            currentWave = SaveLoadManager.Instance.cachedWave;
        }

        // Determine enemies per wave based on currentWave
        if (currentWave == 0)
        {
            currentEnemiesPerWave = initialEnemiesPerWave; // Wave 1
        }
        else if (currentWave == 1)
        {
            currentEnemiesPerWave = initialEnemiesPerWave * 2; // Wave 2
        }
        else if (currentWave == 2)
        {
            currentEnemiesPerWave = initialEnemiesPerWave * 4; // Wave 3
        }
        else
        {
            currentEnemiesPerWave = 16; // Wave 4 and onwards
        }

        GlobalReferences.Instance.waveNumber = currentWave;
        StartWaveFromSave();
    }

    private void StartWaveFromSave()
    {
        currentEnemiesAlive.Clear();
        GlobalReferences.Instance.waveNumber = currentWave;

        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private void StartNextWave()
    {
        currentEnemiesAlive.Clear();
        currentWave++;
        GlobalReferences.Instance.waveNumber = currentWave;

        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentEnemiesPerWave; i++)
        {
            Transform spawnPoint = GetRandomSpawnPoint();

            var enemyObject = Instantiate(Enemy, spawnPoint.position, Quaternion.identity);

            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            enemyScript.enemyPath = FindClosestEnemyPath(spawnPoint.position);

            if (enemyScript.enemyPath == null)
            {
                Debug.LogError("No EnemyPath found near the spawned enemy.");
            }

            currentEnemiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        Transform[] spawnPoints = GetComponentsInChildren<Transform>();
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != transform)
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }

        int randomIndex = Random.Range(0, validSpawnPoints.Count);
        return validSpawnPoints[randomIndex];
    }

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
        currentEnemiesAlive.RemoveAll(enemy => enemy == null || enemy.isDead);

        foreach (Enemy enemy in currentEnemiesAlive)
        {
            if (enemy == null)
            {
                Debug.LogWarning("Found a null enemy in the list!");
            }
        }

        if (currentEnemiesAlive.Count == 0 && !inCoolDown)
        {
            StartCoroutine(WaveCoolDown());
        }

        if (inCoolDown)
        {
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            coolDownCounter = waveCoolDownTimer;
        }

        coolDownCounterUI.text = coolDownCounter.ToString("F0");
    }

    private IEnumerator WaveCoolDown()
    {
        Debug.Log("WaveCoolDown coroutine started");

        inCoolDown = true;
        titleWaveOver1.gameObject.SetActive(true);
        titleWaveOver2.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCoolDownTimer);

        inCoolDown = false;
        titleWaveOver1.gameObject.SetActive(false);
        titleWaveOver2.gameObject.SetActive(false);

        // Adjust enemies per wave based on the new wave
        if (currentWave >= 4)
        {
            currentEnemiesPerWave = 16;
        }
        else
        {
            currentEnemiesPerWave *= 2;
        }

        StartNextWave();
    }
}