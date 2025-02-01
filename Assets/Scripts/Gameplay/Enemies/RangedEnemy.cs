using UnityEngine;

public class RangedEnemy : Enemy
{
    // Speed of the fired projectile in units per second
    public float projectileSpeed = 10f;
    // Maximum distance from Player to stop and attack
    [SerializeField] private float stoppingDistance = 5f; 
    // Time between ranged attacks against Player in seconds
    [SerializeField] private float attackCooldown = 2f;

    // Reference to the projectile used to fire at Player
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // TODO is firePoint necessary?

    // Reference to the Prefab used create loot dropped after death
    [SerializeField] private GameObject deathLootPrefab;

    // Current time since the last ranged attack, in seconds
    private float attackTimer;

    protected override void Update()
    {
        // Guards against if the game ended or Player levels up, continue otherwise
        if (GameManager.instance.freezeGame)
            return;

        // Distance from this RangedEnemy to the Player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If this RangedEnemy is not within stoppingDistance, then move closer to Player,
        // if it is, then attacks as defined below by StopAndAttack() (firing a ranged projectile at Player)
        if (distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopAndAttack();
        }
    }

    // Stops this RangedEnemy so they can face the Player, then attacks
    private void StopAndAttack()
    {
        // Face the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.up = direction;

        // Handle attack cooldown
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    // Attacks the Player with a projectile, if projectile's prefab still exists
    private void Attack()
    {
        if (projectilePrefab != null && firePoint != null) // TODO is firePoint necessary?
        {
            FireProjectile(GameManager.instance.player);
        }
    }

    // Fires a projectile(made from projectilePrefab) at target
    void FireProjectile(Transform target)
    {
        // Guards against no projectile to use or no target being present
        if (projectilePrefab == null || target == null)
            return;

        // Create the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate direction
        Vector2 direction = (target.position - transform.position).normalized;

        // Set projectile damage
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.dmg = damage;
        }

        // Apply velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

    // Creates a deathLoot object upon death 50% of the time, then dies as defined in the Enemy class (plays a death sound and drops an expOrb)
    protected override void Die()
    {
        // Get random 0 or 1, and see if we get a 0 and deathLootPrefab exists, drop deathLoot object if so
        int coinFlip = Random.Range(0, 2);
        if (coinFlip == 0 && deathLootPrefab != null)
        {
            Instantiate(deathLootPrefab, this.transform.position, this.transform.rotation);
        }

        base.Die();
    }

}
