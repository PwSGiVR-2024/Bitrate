using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyVariants; // Array of enemy prefabs, ordered by increasing strength
    public Transform player;
    public float initialSpawnRate = 1f; // Initial number of enemies per 0.1 seconds
    public float currentSpawnRate = 1f;
    public float spawnRateMultiplier = 10f; // Maximum number of enemies per 0.1 seconds
    public float spawnInterval = 0.1f; // Interval between spawn cycles in seconds

    private float elapsedTime;
    private float fractionalSpawnCount = 0f;

    private GameTimer gameTimer;

    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            AdjustSpawnRate();
            float enemiesToSpawn = currentSpawnRate;

            // Spawn the integer part of the enemies
            int integerEnemiesToSpawn = Mathf.FloorToInt(enemiesToSpawn);
            for (int i = 0; i < integerEnemiesToSpawn; i++)
            {
                SpawnEnemy();
            }

            // Accumulate the fractional part for the next cycle
            fractionalSpawnCount += enemiesToSpawn - integerEnemiesToSpawn;

            // If fractionalSpawnCount exceeds 1, spawn an extra enemy and reduce the count
            if (fractionalSpawnCount >= 1f)
            {
                SpawnEnemy();
                fractionalSpawnCount -= 1f;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void AdjustSpawnRate()
    {
        elapsedTime = gameTimer.totalTime - gameTimer.currentTime;
        float timeFraction = elapsedTime / gameTimer.totalTime;
        // Adjust spawn rate linearly from initialSpawnRate to spawnRateMultiplier
        currentSpawnRate = Mathf.Lerp(spawnRateMultiplier, initialSpawnRate, timeFraction);
    }


    void SpawnEnemy()
    {
        Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle.normalized * 20f;
        GameObject enemyToSpawn = SelectEnemyVariant();
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }

    GameObject SelectEnemyVariant()
    {
        float elapsedTime = gameTimer.totalTime - gameTimer.currentTime;
        float timeFraction = elapsedTime / gameTimer.totalTime;

        float[] weights = new float[enemyVariants.Length];
        float totalWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Mathf.Lerp(1f, 0f, (float)i / (weights.Length - 1));
            weights[i] = Mathf.Lerp(weights[i], 1f, timeFraction); // Shift probabilities towards stronger enemies over time
            totalWeight += weights[i];
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return enemyVariants[i];
            }
        }

        return enemyVariants[0]; // Fallback in case of rounding errors
    }
}
