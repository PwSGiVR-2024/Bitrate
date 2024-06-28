using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyVariants; // Array to hold different enemy variants
    public Transform player;
    public float initialSpawnRate = 5f; // Initial spawn rate in seconds
    public float spawnRateMultiplier = 10f; // Multiplier for the final spawn rate
    private float spawnRate;
    private float nextSpawnTime;

    private GameTimer gameTimer;

    public float spawnRadius = 20f; // Radius of the spawning ring

    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
        spawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        AdjustSpawnRate();
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void AdjustSpawnRate()
    {
        float elapsedTime = gameTimer.totalTime - gameTimer.currentTime;
        float timeFraction = elapsedTime / gameTimer.totalTime;
        spawnRate = Mathf.Lerp(initialSpawnRate, initialSpawnRate / spawnRateMultiplier, timeFraction);
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
        GameObject enemyToSpawn = SelectEnemyVariant();
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }

    GameObject SelectEnemyVariant()
    {
        // Simple selection logic for now, can be expanded for weighted probabilities
        float elapsedTime = gameTimer.totalTime - gameTimer.currentTime;
        float timeFraction = elapsedTime / gameTimer.totalTime;

        // For simplicity, higher index in the array means stronger enemy
        int index = Mathf.FloorToInt(timeFraction * (enemyVariants.Length - 1));
        index = Mathf.Clamp(index, 0, enemyVariants.Length - 1);

        return enemyVariants[index];
    }
}
