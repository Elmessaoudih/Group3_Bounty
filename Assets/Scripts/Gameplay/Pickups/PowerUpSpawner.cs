using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    // Array of power-up prefabs
    [SerializeField] private GameObject[] powerupPrefabs;
    // Array of spawn points
    [SerializeField] private Transform[] spawnPoints;
    // Time in seconds between spawns
    [SerializeField] private float spawnInterval = 5f; 

    private void Start()
    {
        // Start the spawning loop
        StartCoroutine(SpawnPowerUps());
    }

    // Spawn a PowerUp very spawnInterval seconds
    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);

            // Randomly select a power-up prefab
            GameObject selectedPowerUp = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];

            // Randomly select a spawn point
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the power-up at the selected spawn point
            Instantiate(selectedPowerUp, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        }
    }
}
