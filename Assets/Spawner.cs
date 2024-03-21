using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject spawnee;
    public bool IWantToSpawn = true;
    public float initialSpawnDelay;
    public float decreaseSpawnDelayInterval;
    public float minSpawnDelay;
    public float spawnRangeX = 5f; // Adjustable range for X coordinate
    public float spawnRangeZ = 5f; // Adjustable range for Z coordinate

    void Start()
    {
        StartCoroutine(SpawnControl());
    }

    IEnumerator SpawnControl()
    {
        float spawnDelay = initialSpawnDelay;

        while (IWantToSpawn)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(spawnDelay);

            if (spawnDelay > minSpawnDelay)
            {
                spawnDelay -= decreaseSpawnDelayInterval;
            }
        }
    }

    void SpawnEnemies()
    {
        // Calculate random position within the defined range
        float randomX = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, randomZ);

        // Instantiate the spawnee at the calculated position
        Instantiate(spawnee, spawnPosition, transform.rotation);
    }
}
