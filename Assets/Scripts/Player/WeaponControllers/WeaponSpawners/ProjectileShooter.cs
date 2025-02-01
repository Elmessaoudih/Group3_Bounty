using UnityEngine;

public class ProjectileShooter : NearestEnemyAttackWeapon
{
    // Reference to the prefab used to fire the Starter Weapon's projectiles
    public GameObject projectilePrefab;
    // Damage the fired projectile will be given
    public int dmg;
    // Time between protectiles being fired, in seconds
    public float fireInterval = 1f;
    // Speed the fired projectile moves, in units per second
    public float projectileSpeed = 10f;

    // Timer that counts down in seconds from fireInterval to 0 seconds (then fires)
    private float fireTimer;

    void Update()
    {
        // Guard against the game being paused
        if (GameManager.instance.freezeGame)
            return;

        // Count down the fireTimer
        fireTimer -= Time.deltaTime;

        // If is it time to fire, shoot nearest enemy and reset fireTimer
        if (fireTimer <= 0f)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                FireProjectile(nearestEnemy.transform);
            }
            fireTimer = fireInterval;
        }
    }
    
    // Fires a projectilePrefab at the target's current position, and initializes its dmg and projectileSpeed
    void FireProjectile(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (target.position - transform.position).normalized;
        projectile.GetComponent<Projectile>().dmg = dmg;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }
}
