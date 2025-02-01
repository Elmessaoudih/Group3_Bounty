using System.Collections;
using UnityEngine;

public class RandomTossGun : MonoBehaviour
{
    // Reference to the fireSpread prefab this RandomTossGun spawns and throws
    public GameObject fireSpread;
    // Time fireSpread exists in the game, in seconds
    public float lifeTime = 5;
    // Damage fireSpread does when Enemies enter it
    public int damage = 5;
    // The units the tossed fireSpread will grow in each dimension once it spreads
    public float fireRadius = 3f;

    // Time between tosses, in seconds
    public float tossInterval = 2f;
    // Radius of the circle centered on the Player, within which the object is tossed
    public float tossRadius = 5f;
    // Time it takes to move the object to the target point, in seconds
    public float tossDuration = 1f; 

    private void Start()
    {
        // Start the tossing loop
        StartCoroutine(TossObject());
    }

    // Create the given fireSpread and toss it to a random location tossRadius units away from Player over the course of tossDuration seconds
    private IEnumerator TossObject()
    {
        while (true)
        {
            // Wait for tossInterval seconds to elapse
            yield return new WaitForSeconds(tossInterval);
            Debug.Log("tossing");

            // Generate a random position within the radius
            Vector2 randomOffset = Random.insideUnitCircle * tossRadius;
            Vector3 targetPosition = new Vector3(randomOffset.x, randomOffset.y, 0) + transform.position;

            // Instantiate the object at the player's position
            GameObject tossedObject = Instantiate(fireSpread, transform.position, Quaternion.identity);
            tossedObject.GetComponent<FireSpread>().Setup(lifeTime, damage, fireRadius, tossDuration);

            // Lerp the object to the target position over time
            float elapsedTime = 0f;
            Vector3 startPosition = tossedObject.transform.position;
            while (elapsedTime < tossDuration)
            {
                // Ensure time has passed (game is unpaused) before using Time.deltaTime and calculating position
                if (!GameManager.instance.freezeGame)
                {
                    // Increase elapsedTime, and find the fraction of tossDuration that has elapsed so far
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / tossDuration;

                    // Smoothly move the object from the start to the target position
                    tossedObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                }

                // Ensure Unity updates the frame, before while loop tries to move projectile again
                yield return null;
            }

            // Ensure the object ends exactly at the target position
            tossedObject.transform.position = targetPosition;
        }
    }
}
