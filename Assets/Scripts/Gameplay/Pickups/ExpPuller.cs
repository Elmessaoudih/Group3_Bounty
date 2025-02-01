using System.Collections;
using UnityEngine;

public class ExpPuller : MonoBehaviour
{
    // Adjust the speed at which the objects move towards the player, in units per second
    public float pullSpeed = 5f;
    // Time this ExpPuller will exist (and thus, the maximum amount of time it will pull exp) 
    public float pullDuration = 2f;

    // Detect collision of Player with the pulling object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ensure this is the object that triggers the pull
        if (collision.gameObject.CompareTag("Player"))
        {
            StartPullingExp();
        }
    }

    // Function to find all "Exp" tagged objects (only the ones that currently exist) and pull them towards the player
    private void StartPullingExp()
    {
        // Turn this puller invisible and untriggerable
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;

        // Destroy the ExpPuller once its pullDuration expires
        Destroy(this.gameObject, pullDuration);

        // Find all objects tagged with "Exp"
        GameObject[] expObjects = GameObject.FindGameObjectsWithTag("Exp");

        // Start moving each "Exp" object towards the player
        foreach (GameObject exp in expObjects)
        {
            StartCoroutine(PullExpTowardsPlayer(exp));
        }
    }

    // Coroutine to move an object towards the player
    private IEnumerator PullExpTowardsPlayer(GameObject exp)
    {
        // While exp exists and isn't close enough to the Player to stop pulling
        while (exp != null && !(Vector3.Distance(exp.transform.position, GameManager.instance.player.transform.position) < 0.1f))
        {
            // Ensure time has passed (game is unpaused) before using Time.deltaTime and calculating pull
            if (!GameManager.instance.freezeGame)
            {
                // Calculate the direction to the Player
                Vector3 direction = (GameManager.instance.player.transform.position - exp.transform.position).normalized;

                // Move the object towards the Player
                exp.transform.position += direction * pullSpeed * Time.deltaTime;
            }

            // Wait for the next frame, ensuring while loop waits until pulling again
            yield return null;
        }
    }
}
