using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemy : Enemy
{
    // The distance a SuicideEnemy must within from the Player to activate the suicide attack
    [SerializeField] private float stoppingDistance = 0.5f;

    // Reference to the Prefab used to create the lingering effects of the suicide attack
    [SerializeField] private GameObject deathObjPrefab;

    protected override void Update()
    {
        // Guards against if the game ended or Player levels up, continue otherwise
        if (GameManager.instance.freezeGame)
            return;

        // Distance from this SuicideEnemy to the Player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If this SuicideEnemy is not within stoppingDistance, then move closer to Player,
        // if it is, then dies as defined below (dropping a deathObj to damage the Player)
        if (distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            Die();
        }
    }

    // Creates a deathObj upon death, then dies as defined in the Enemy class (plays a death sound and drops an expOrb)
    protected override void Die()
    {
        if (deathObjPrefab != null)
        {
            Instantiate(deathObjPrefab, this.transform.position, this.transform.rotation);
        }

        base.Die();
    }
}
