using System.Collections;
using UnityEngine;

public class FireSpread : MonoBehaviour
{
    // Damage this fireSpread deals to Enemies when they enter it
    [HideInInspector] public int dmg = 0;

    // Initialize this FireSpread with a given duration to exist in the game in seconds (lifetime), a damage value (dmg),
    // the amount of units the fire grows when spreading (spreadRadius), and a delay in seconds before the fire spreads (timeBeforeSpread) 
    public void Setup(float lifeTime = 5, int damage=1, float spreadRadius=2, float timeBeforeSpread = 1)
    {
        // Initialize dmg and begin the spreading process
        dmg = damage;
        StartCoroutine(Spread(spreadRadius, timeBeforeSpread));

        // FireSpread dissipates after lifeTime seconds elapse
        Destroy(this.gameObject, lifeTime);
    }

    // Causes FireSpread to spread from its tiny starting size into one that is radius larger in all dimensions, after delay seconds elapses
    IEnumerator Spread(float radius, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.transform.localScale += new Vector3(radius, radius, radius);
    }

    // When an Enemy enter the FireSpread, they take its dmg
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(dmg);
        }
    }
}
