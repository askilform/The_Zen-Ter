using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;     // Assign your enemy prefab here
    public int maxEnemies = 10;        // Total enemies to spawn
    public float minSpawnDelay = 2f;   // Min delay between spawns
    public float maxSpawnDelay = 5f;   // Max delay between spawns

    private int currentEnemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    System.Collections.IEnumerator SpawnEnemies()
    {
        while (currentEnemyCount < maxEnemies)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            Instantiate(enemyPrefab, transform.position, transform.rotation);
            currentEnemyCount++;
        }
    }
}
