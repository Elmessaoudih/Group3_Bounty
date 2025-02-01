using UnityEngine;

public class SlashSpawner : MonoBehaviour
{
    // The prefab of the sword to spawn
    public GameObject slashPrefab;
    // Damage value for sword's slash
    public int dmg;
    // Interval between sword spawns
    public float fireInterval = 1f;
    // Offset distance (both positive and negative) of sword from the Player along the X axis
    public float spawnOffset = 3f;

    // Timer that counts down in seconds from fireInterval to 0 seconds (then slashes)
    private float fireTimer;
    // Alternates between true and false to toggle spawn side
    private bool spawnOnRight = true; 

    void Update()
    {
        // If the game is frozen, do nothing, otherwise continue
        if (GameManager.instance.freezeGame)
            return;

        // Count down fireTimer
        fireTimer -= Time.deltaTime;

        // If is it time to slash, spawn sword slash and reset fireTimer
        if (fireTimer <= 0f)
        {
            SpawnSlash();
            fireTimer = fireInterval;
        }
    }

    // Spawns a sword slash to the right or left side of the Player (alternating between left and right each time)
    void SpawnSlash()
    {
        // Determine the spawn position based on the current side
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += spawnOnRight ? spawnOffset : -spawnOffset;

        // Instantiate the slash prefab at the calculated position
        Instantiate(slashPrefab, spawnPosition, Quaternion.identity, GameManager.instance.player).GetComponent<Slash>().dmg = dmg;

        // Alternate the side for the next spawn
        spawnOnRight = !spawnOnRight;
    }
}
