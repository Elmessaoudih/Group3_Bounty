using UnityEngine;

public class ShotgunWeapon : NearestEnemyAttackWeapon
{
    // Reference to the prefab used to create the Shotgun's projectiles
    public GameObject projectilePrefab;
    // Damage each projectile deals
    public int dmg;
    // Time between shots from the Shotgun
    public float fireInterval = 1f;
    // Speed of the projectiles fired
    public float projectileSpeed = 10f;
    // Number of projectiles per shot
    public int projectileCount = 5;
    // Spread angle in degrees for the shotgun blast
    public float spreadAngle = 30f;     

    // Timer that counts down in seconds from fireInterval to 0 seconds (then fires)
    private float fireTimer;

    void Update()
    {
        // Guard against game being paused, continue otherwise
        if (GameManager.instance.freezeGame)
            return;

        // Count down fireTimer
        fireTimer -= Time.deltaTime;

        // If is it time to fire, shoot nearest enemy and reset fireTimer
        if (fireTimer <= 0f)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                FireShotgunBlast(nearestEnemy.transform);
            }
            fireTimer = fireInterval;
        }
    }

    // Fires projectiles in an evenly distributed cone with an angle of spreadAngle towards target
    void FireShotgunBlast(Transform target)
    {
        // Initialize additional angle between projectiles (angleStep) and the angle of the first projectile (startAngle)
        float angleStep = spreadAngle / (projectileCount - 1);
        float startAngle = -spreadAngle / 2;

        // For each projectile, fire it at an certain angle with respect to the line between the shotgunner and target,
        // and give it damage and velocity values
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 direction = rotation * (target.position - transform.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().dmg = dmg;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        }
    }
}
