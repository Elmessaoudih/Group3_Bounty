using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnScript : MonoBehaviour
{
    [System.Serializable]
    public class mapPiece
    {
        public GameObject prefab; // The prefab for the map piece
        public GameObject singletonReference; // The singleton reference for this map piece
    }

    public List<mapPiece> mapPieces = new List<mapPiece>(); // List of map pieces to spawn
    public Transform[] spawnPoints; // Array of spawn points

    private List<Transform> usedSpawnPoints = new List<Transform>(); // To track used spawn points

    private bool pairedPiece1spawned = false;
    private bool pairedPiece2spawned = false;
    private bool pairedPiece3spawned = false;
    private bool pairedPiece4spawned = false;

    public MapPiece map1;
    public MapPiece map2;
    public MapPiece map3;
    public MapPiece map4;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAllPieces());
    }

    private IEnumerator SpawnAllPieces()
    {
        while (!pairedPiece1spawned || !pairedPiece2spawned || !pairedPiece3spawned || !pairedPiece4spawned)
        {
            foreach (var currMapPiece in mapPieces)
            {
                yield return StartCoroutine(SpawnUntilSuccess(currMapPiece));
            }
        }
    }

    private IEnumerator SpawnUntilSuccess(mapPiece currMapPiece)
    {
            List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints); // Copy the list of all spawn points

            // Remove the used spawn points from the available list
            availableSpawnPoints.RemoveAll(spawnPoint => usedSpawnPoints.Contains(spawnPoint));

        // Attempt to spawn at an available spawn point
        foreach (var spawnPoint in availableSpawnPoints)
        {
            if ((currMapPiece.prefab.GetComponent<PickUpMap>().piece == map1 && !pairedPiece1spawned) ||
                (currMapPiece.prefab.GetComponent<PickUpMap>().piece == map2 && !pairedPiece2spawned) ||
                (currMapPiece.prefab.GetComponent<PickUpMap>().piece == map3 && !pairedPiece3spawned) ||
                (currMapPiece.prefab.GetComponent<PickUpMap>().piece == map4 && !pairedPiece4spawned))
            {
                if (attemptSpawn())
                {
                    currMapPiece.singletonReference = SpawnPiece(currMapPiece.prefab, spawnPoint);
                    if (currMapPiece.singletonReference != null)
                    {
                        if (currMapPiece.prefab.GetComponent<PickUpMap>().piece == map1)
                        {
                            pairedPiece1spawned = true;
                        }
                        if (currMapPiece.prefab.GetComponent<PickUpMap>().piece == map2)
                        {
                            pairedPiece2spawned = true;
                        }
                        if (currMapPiece.prefab.GetComponent<PickUpMap>().piece == map3)
                        {
                            pairedPiece3spawned = true;
                        }
                        if (currMapPiece.prefab.GetComponent<PickUpMap>().piece == map4)
                        {
                            pairedPiece4spawned = true;
                        }
                        usedSpawnPoints.Add(spawnPoint); // Mark this spawn point as used
                        Debug.Log($"Successfully spawned {currMapPiece.prefab.name} at {spawnPoint.name}");
                        yield break; // Exit the loop once successful
                    }
                }
            }


            yield return new WaitForSeconds(0.1f); // Small delay before the next spawn attempt
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

        // 10% chance to spawn
        return random.Next(1, 101) > 90;
    }
}

