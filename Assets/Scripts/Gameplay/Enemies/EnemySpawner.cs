using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class holding the prefab and difficulty of a general kind of Enemy
// ATTENTION: Keep EnemyTypes sorted by difficultyValue in ascending order ,
// not doing so could make some Enemies unable to spawn until higher waves
[System.Serializable]
public class EnemyType
{
    // The prefab used to create an Enemy
    public GameObject enemyPrefab;
    // The difficulty of an Enemy, where higher values makes an Enemy less likely to spawn on early waves or spawn less times in a given wave
    public int difficultyValue;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    // The EnemyTypes of the Enemies that can spawn in a given wave
    [Tooltip("Sort these in ascending order by difficultyValue (Element 0 is lowest)")]
    public List<EnemyType> enemyTypes;
    // Difficulty of the first spawn wave is baseDifficulty + waveDifficultyIncrease
    public int baseDifficulty = 5;
    // How much more difficulty this wave has than the previous wave (that is uses to spawn an increasing amount of Enemies) 
    public int waveDifficultyIncrease = 5;
    // Radius of the circle around the Player, where Enemies spawn within a random point inside
    public float spawnRadius = 10f;
    // Time after a given wave's last Enemy is spawned until the next wave begins, in seconds
    public float timeBetweenWaves = 5f;
    // Time between each individual Enemy spawning from a given wave, ensuring all Enemies in a wave don't spawn at once, in seconds
    public float spawnInterval = 0.1f;

    // Reference to the Coroutine controlling spawn waves
    private Coroutine waves;
    // The number of the wave that will be spawned next
    private int currentWave = 1;

    // Begin to spawn waves of Enemies, until StopWaves() is called
    public void StartWaves()
    {
        waves = StartCoroutine(SpawnWaves());
    }

    // Calculate wave difficulty, spawn random assortment of Enemies one-by-one, and begin next wave
    // (unless Intervals haven't elapsed or game is paused)
    private IEnumerator SpawnWaves()
    {
        while (GameManager.instance.player != null)
        {
            // Allocate Enemies within the difficultyBudget
            int difficultyBudget = baseDifficulty + (currentWave * waveDifficultyIncrease);
            List<GameObject> enemiesToSpawn = AllocateEnemies(difficultyBudget);

            // Spawn Enemies of this wave one-by-one
            foreach (GameObject enemyPrefab in enemiesToSpawn)
            {
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(spawnInterval);
            }

            // Wait until next wave
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    // Returns a List of enemyPrefabs where the sum of their corresponding difficultyValues are equal to the given budget
    private List<GameObject> AllocateEnemies(int budget)
    {
        // Create the resulting List
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        // Initialize the index of the EnemyType with the least difficultyValue that we know is out of the budget
        // (== enemy.Count means there is no such EnemyType currently known)
        int leastOutOfBudgetEnemyIndex = enemyTypes.Count;

        // Continue allocating Enemies until budget is spent or there are no more EnemyTypes that are within the budget
        while (budget > 0 && leastOutOfBudgetEnemyIndex > 0)
        {
            // Select an EnemyType at a random selectedEnemyIndex, ensuring not to select EnemyTypes that are known to be out of the budget
            int selectedEnemyIndex = Random.Range(0, leastOutOfBudgetEnemyIndex);
            EnemyType selectedEnemy = enemyTypes[selectedEnemyIndex];
            // If selection is within budget, then add that EnemyType's prefab to the List and decrease the budget
            if (selectedEnemy.difficultyValue <= budget)
            {
                enemiesToSpawn.Add(selectedEnemy.enemyPrefab);
                budget -= selectedEnemy.difficultyValue;
            }
            // If not, then we now know this selectedEnemy (and all the ones after it in the array enemyTypes[]) is out of the budget
            else 
            {
                leastOutOfBudgetEnemyIndex = selectedEnemyIndex;
            }
        }

        // Return the List of allocated Enemies
        return enemiesToSpawn;
    }

    // Spawn given enemyPrefab at a random position within a circle of radius spawnRadius around the Player
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 spawnPosition = GameManager.instance.player.position + (Vector3)(Random.insideUnitCircle.normalized * spawnRadius);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    // Stop spawn waves
    public void StopWaves()
    {
        StopCoroutine(waves);
    }
}
