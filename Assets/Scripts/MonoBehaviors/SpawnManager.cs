using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnScript : MonoBehaviour
{
    [System.Serializable]
    public class MapPiece
    {
        public GameObject prefab; // The prefab for the map piece
        public GameObject singletonReference; // The singleton reference for this map piece
    }

    public List<MapPiece> mapPieces = new List<MapPiece>(); // List of map pieces to spawn
    public Transform[] spawnPoints; // Array of spawn points

    private List<Transform> usedSpawnPoints = new List<Transform>(); // To track used spawn points

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAllPieces());
    }

    private IEnumerator SpawnAllPieces()
    {
        foreach (var mapPiece in mapPieces)
        {
            yield return StartCoroutine(SpawnUntilSuccess(mapPiece));
        }
    }

    private IEnumerator SpawnUntilSuccess(MapPiece mapPiece)
    {
        while (mapPiece.singletonReference == null) // Loop until the map piece spawns
        {
            List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints); // Copy the list of all spawn points

            // Remove the used spawn points from the available list
            availableSpawnPoints.RemoveAll(spawnPoint => usedSpawnPoints.Contains(spawnPoint));

            // Attempt to spawn at an available spawn point
            foreach (var spawnPoint in availableSpawnPoints)
            {
                if (attemptSpawn())
                {
                    mapPiece.singletonReference = SpawnPiece(mapPiece.prefab, spawnPoint);
                    if (mapPiece.singletonReference != null)
                    {
                        usedSpawnPoints.Add(spawnPoint); // Mark this spawn point as used
                        Debug.Log($"Successfully spawned {mapPiece.prefab.name} at {spawnPoint.name}");
                        yield break; // Exit the loop once successful
                    }
                }

                yield return new WaitForSeconds(0.1f); // Small delay before the next spawn attempt
            }
        }
    }

    private GameObject SpawnPiece(GameObject prefab, Transform spawnPoint)
    {
        // Instantiate the prefab at the specified spawn point
        if (prefab != null)
        {
            GameObject newPiece = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Spawning {prefab.name} at {spawnPoint.name}");
            return newPiece;
        }

        return null; // Return null if prefab is invalid
    }

    private bool attemptSpawn()
    {
        System.Random random = new System.Random();

        // 50% chance to spawn
        return random.Next(1, 101) > 90;
    }
}

