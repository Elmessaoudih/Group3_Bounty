using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] AudioClip explosion;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ensure this is the object that triggers the pull
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }

    }

    //function to kill all near by enemies
    private void Explode()
    {
        // Define explosion radius
        float explosionRadius = 10f;

        // Find all colliders within the radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        SFXManager.instance.PlayClip(explosion);
        foreach (Collider2D hit in hits)
        {
            // Check if the object is tagged as "Enemy"
            if (hit.CompareTag("Enemy"))
            {
                // kill the enemy
                hit.GetComponent<Enemy>().TakeDamage(999);
            }
        }

        // Destroy the current game object (explosion source)
        Destroy(this.gameObject);
    }
}
