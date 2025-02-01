using System.Collections;
using UnityEngine;

public class OrbitSpawner : MonoBehaviour
{
    // prefab of the projectile that this OrbitSpawner spawns and rotates
    public GameObject projectilePrefab;
    // Damage the orbiting body does on impact
    public int dmg;
    // Duration for the projectile to orbit,in seconds
    [SerializeField] public float orbitDuration = 5f;
    // Cooldown between spawns, in seconds
    [SerializeField] public float cooldown = 2f;
    // Distance of the projectile from the player, AKA radius of projectile's orbit
    [SerializeField] public float orbitRadius = 2f;
    // Angular veloctiy of the orbiting body, in degrees per second
    [SerializeField] public float orbitSpeed = 180f; 

    private void Start()
    {
        // Start the orbiting loop
        StartCoroutine(OrbitProjectile());
    }

    // Create the given projectilePrefab and orbit it orbitRadius units away from the Player for orbitDuration seconds
    private IEnumerator OrbitProjectile()
    {
        while (true)
        {
            // Wait for the Orbit weapon's cooldown
            Debug.Log("Awaiting cooldown...");
            yield return new WaitForSeconds(cooldown);

            // Create the orbiting body
            Debug.Log("Cooldown over, spawning projectile...");
            GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.right * orbitRadius, Quaternion.identity, this.transform);
            projectile.SetActive(true);
            projectile.GetComponent<Projectile>().dmg = dmg;

            // Make the projectile orbit around the Player over the course of orbitDuration seconds
            float elapsedTime = 0f;
            while (elapsedTime < orbitDuration)
            {
                // Ensure time has passed (game is unpaused) before using Time.deltaTime and calculating rotation
                if (!GameManager.instance.freezeGame)
                {
                    // Increase elapsedTime
                    elapsedTime += Time.deltaTime;

                    // Calculate the angle based on elapsed time and orbit speed
                    float angle = elapsedTime * orbitSpeed;
                    float radian = angle * Mathf.Deg2Rad;

                    // Update projectile position to follow a circular path around the player
                    Vector3 offset = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0) * orbitRadius;
                    projectile.transform.position = transform.position + offset;
                }

                // Ensure Unity updates the frame, before while loop tries to rotate projectile again
                yield return null; 
            }

            // Destroy the projectile after orbiting (orbitDuration elapsed)
            Debug.Log("Orbit duration finished, resetting projectile...");
            Destroy(projectile); 
        }
    }
}
