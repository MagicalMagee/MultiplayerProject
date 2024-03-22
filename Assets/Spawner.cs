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

    // Controls and Adjust the delay in spawn and its frequencies
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
        // Random position within the certain range of the "arena"
        float randomX = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, randomZ);

        // Instantiate the spawnees randomly at the specified range
        Instantiate(spawnee, spawnPosition, transform.rotation);
    }
}
