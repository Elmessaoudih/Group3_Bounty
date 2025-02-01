using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    // The sound that plays when an Enemy is killed
    [SerializeField] AudioClip deathSound;

    // Health this Enemy starts with (protected for access in derived classes)
    [SerializeField] protected int health = 5;
    // Damage this Enemy deals to Player (protected for access in derived classes)
    [SerializeField] protected int damage = 5;
    // Speed of Enemies in units per second
    [SerializeField] public float moveSpeed = 2f;
    // Amount of exp Enemies drop upon death
    [SerializeField] public int expDropped = 1;
    // Distance the Player is knocked back upon collision with this Enemy, valid values are dictated by the constants below 
    [SerializeField] protected float knockbackForce = .25f;
    // The inclusive maximum value knockbackForce can have, if greater, KNOCK_MAX is used instead of knockbackForce
    protected const float KNOCK_MAX = .5f;
    // The non-inclusive minimum value knockbackForce can have, if it is equal or lesser, knockback is ignored
    protected const float KNOCK_MIN = 0f;

    // Distance needed to move this Enemy's current position inward to bring them back in-bounds
    protected const float JUMP_DOWN_FORCE = 1f;

    // Reference to the expOrb prefab to use to create orbs dropped upon death (made protected for derived classes)
    [SerializeField] protected GameObject expOrbPrefab;
    // Reference to the Player for positioning and attacking purposes (made protected for derived classes)
    protected Transform player;
    

    protected virtual void Start()
    {
        // Initialize reference to the Player 
        player = GameManager.instance.player;
    }

    protected virtual void Update()
    {
        // Guard against the game being paused, continue otherwise
        if (GameManager.instance.freezeGame)
            return;

        // Move to Player's position
        MoveTowardsPlayer();
    }

    // By default, Enemies move towards the Player, only stopping when in the Player space or the Player is dead
    protected virtual void MoveTowardsPlayer()
    {
        // Guard against Player being dead or non-existent, continue otherwise
        if (player == null) return;

        // Move towards the player�s position
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    // By default, Enemies lose dmg amount of health when taking damage and die when health becomes or falls under 0
    public virtual void TakeDamage(int dmg)
    {
        // Decrement health
        health -= dmg;

        // Check if dead
        if (health <= 0)
        {
            Die();
        }
    }

    // By default, Enemies make a death sound and drop expOrbs upon death, their gameObject is also removed from the Scene
    protected virtual void Die()
    {
        // deathSound exists, so play it
        if (deathSound != null)
            SFXManager.instance.PlayClip(deathSound);

        // expOrb's prefab exists, so drop it and set its expGained to expDropped from this Enemy
        if (expOrbPrefab != null)
        {
            Instantiate(expOrbPrefab, transform.position, transform.rotation).GetComponent<ExpOrb>().expGained = expDropped;
        }

        // Remove Enemy from Scene
        Destroy(gameObject);
    }

    // By default, upon touching the Player, Enemies deal their damage to the Player and knock them back (if they have a knockbackForce)
    // upon touching a Wall, Enemies move away or past it (so out-of-bounds enemies can re-enter the battle)
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Attack Player with melee
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInChildren<PlayerController>().TakeDamage(damage);
            // Knockback Player if applicable
            if (knockbackForce > KNOCK_MIN)
            {
                Vector2 knockback = (player.position - transform.position).normalized * Mathf.Min(knockbackForce, KNOCK_MAX);
                player.GetComponent<Rigidbody2D>().MovePosition((Vector2)player.position + knockback);
            }
            // Optionally handle destroy/self-destruct here for specific enemies (by overriding in derived classes)
        } 
        // Otherwise, go in-bounds if touching a wall
        else if (collision.CompareTag("Wall"))
        {
            transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, JUMP_DOWN_FORCE);
        }
    }
}
