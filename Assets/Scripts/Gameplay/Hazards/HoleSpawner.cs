using System.Collections;
using UnityEngine;

public class HoleSpawner : MonoBehaviour
{
    // Reference to the Hole's prefab
    [SerializeField] private GameObject holePrefab;
    // Array of spawn points
    [SerializeField] private Transform[] spawnPoints;
    // Time before the holePrefab is created where a warning appears on the ground at the spawn point
    [SerializeField] private float warningTime = 2f;
    // Time in seconds between Hole spawns
    [SerializeField] private float spawnInterval = 15f;

    // Reference to the spawn point where a Hole will soon be created
    [HideInInspector] private SpriteRenderer spawnPointRenderer;

    // Sorting Layer for Holes the their warning sprites (which are attached to the spawn points)
    private const string HOLE_SORTING_LAYER = "Holes";

    // Sorting Layer for warning sprites so they aren't seen (they are under the Ground layer, so invisible to Player)
    private const string DEFAULT_SORTING_LAYER = "Default";

    private void Start()
    {
        // Start the spawning loop
        StartCoroutine(SpawnHoles());
    }

    // Spawn a Hole every spawnInterval seconds, with a warning at its soon-to-be center for warningTime seconds before
    private IEnumerator SpawnHoles()
    {
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);

            // Randomly select a spawn point
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Enable warning sprite on the spawn point
            spawnPointRenderer = selectedSpawnPoint.GetComponent<SpriteRenderer>();
            spawnPointRenderer.sortingLayerName = HOLE_SORTING_LAYER;

            // Allow Player to see warning
            yield return new WaitForSeconds(warningTime);

            // Disable warning sprite
            spawnPointRenderer.sortingLayerName = DEFAULT_SORTING_LAYER; 

            // Instantiate the power-up at the selected spawn point
            Instantiate(holePrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        }
    }
}
