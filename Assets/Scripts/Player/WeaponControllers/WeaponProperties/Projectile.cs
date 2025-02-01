using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Damage this projectile deals
    [HideInInspector] public int dmg = 0;

    // The tag of Projectile's target, defaults to "Enemy" for Player-made Projectiles, but can be "Player" for Enemy-made Projectiles
    [Tooltip("The tag of the object this projectile can damage.")]
    [SerializeField] private string targetTag = "Enemy";

    // If true, projectile is destroyed upon collision with the target, and isn't when this variable is false
    [Tooltip("Whether the projectile is destroyed upon hitting a tagrt")]
    [SerializeField] private bool destroyOnHit = true;

    private void Start()
    {
        // Clean up after 10 seconds if the projectile misses
        Destroy(this.gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If collided with a target
        if (collision.CompareTag(targetTag))
        {
            // Use method relevant to the target
            if (targetTag == "Enemy")
            {
                // Deal damage to an enemy
                var enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(dmg);
                }
            }
            else if (targetTag == "Player")
            {
                // Deal damage to a player
                var player = collision.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(dmg);
                }
            }

            // Destroy projectile upon hitting the target
            if (destroyOnHit)
            {

                Destroy(gameObject);
            }
        }
    }
}
