using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    // Amount of exp gained when picking up this ExpOrb
    [HideInInspector] public int expGained = 1;

    // When a Player touches this ExpOrb, give Player expGained exp and remove the ExpOrb
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().GainExp(expGained);

            Destroy(gameObject);
        }
    }
}
