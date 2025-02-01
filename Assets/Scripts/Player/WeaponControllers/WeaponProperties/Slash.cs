using UnityEngine;

public class Slash : MonoBehaviour
{
    // Time the Sword's Slash stays in the Scene, in seconds
    [SerializeField] float lifetime = 0.2f;
    // Damage dealt by Sword Slash on contact
    [HideInInspector] public int dmg;


    private void Start()
    {
        // Destroy this Slash once lifetime elapses
        Destroy(this.gameObject, lifetime);
    }

    // Upon an Enemy entering this Slash's trigger, they will take the Sword's dmg (if the damage is positive)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("hit enemy");
            // Deal damage to an enemy
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(dmg);
            }

        }
    }
}
