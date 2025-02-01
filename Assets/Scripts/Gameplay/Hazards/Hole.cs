using UnityEngine;

public class Hole : MonoBehaviour
{
    // The duration the Hole exists in the Scene, in seconds
    [SerializeField] private float lifeTime = 10f;
    
    void Start()
    {
        // Remove Hole from the Scene when it expires
        Destroy(gameObject,lifeTime);
    }

    // When the Player, an Enemy, or a PowerUp tocuches the trigger (which is inside the Hole), then they are killed/removed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collidedTag = collision.tag;

        switch (collidedTag)
        {
            case "Player":
                GameManager.instance.EndGame();
                break;
            case "Enemy":
                collision.GetComponent<Enemy>().TakeDamage(999);
                break;
            case "PowerUp":
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }
}
