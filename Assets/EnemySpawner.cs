using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyPrefab; // Drag your 'Slime' prefab here
    public float spawnInterval = 2f; // How often to spawn (Seconds)
    
    [Header("Spawn Area")]
    public float minDistance = 10f; // Minimum distance from player (Just outside screen)
    public float maxDistance = 14f; // Maximum distance (Don't spawn too far)

    private Transform player;
    private float nextSpawnTime;

    void Start()
    {
        // Find player automatically so we know where to spawn enemies around
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Check Timer
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // 1. Pick a random direction (Random point on a circle of radius 1)
        Vector2 randomDir = Random.insideUnitCircle.normalized;

        // 2. Pick a random distance between Min and Max
        float distance = Random.Range(minDistance, maxDistance);

        // 3. Calculate the actual spawn position relative to the player
        Vector2 spawnPos = (Vector2)player.position + (randomDir * distance);

        // 4. Create the enemy
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}