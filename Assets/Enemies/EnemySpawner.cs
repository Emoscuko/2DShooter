using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyPrefab; 
    public float spawnInterval = 2f;

    [Header("Spawn Offset")]
    // Optional: Adds a tiny bit of randomness so slimes don't spawn inside each other
    public float scatterRadius = 0.5f; 

    private float nextSpawnTime;

    void Update()
    {
        // Check Timer
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // 1. Start at the Spawner's position
        Vector2 spawnPos = transform.position;

        // 2. Add a tiny bit of randomness (Scatter)
        // This prevents enemies from stacking perfectly on top of each other
        Vector2 randomOffset = Random.insideUnitCircle * scatterRadius;
        spawnPos += randomOffset;

        // 3. Create the enemy
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
    
    // VISUAL AID: This draws a small circle in the Editor so you can see the spawn point
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scatterRadius > 0 ? scatterRadius : 0.5f);
    }
}