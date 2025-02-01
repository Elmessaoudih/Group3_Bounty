using UnityEngine;

public class Player : Character
{
    public Inventory inventoryPrefab;
    Inventory inventory;

    // This is calling the CaughtImage in the Canvas
    public GameObject caughtImage;

    void Start()
    {
        inventory = Instantiate(inventoryPrefab);
        // This makes the image that says you beat the game not visible when the game starts
        caughtImage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickUp"))
        {
            MapPiece hitMapPiece = collision.gameObject.GetComponent<PickUpMap>().piece;

            if (hitMapPiece != null)
            {
                print("hit it: " + hitMapPiece.objectName);
                inventory.AddItem(hitMapPiece);
                collision.gameObject.SetActive(false);
            }

            // This checks if the inventory is full with all the map parts
            if (inventory.IsFull())
            {
                StopGame();
            }
        }
    }

    // Stops the game and displays the image
    void StopGame()
    {
        // This pauses the game
        Time.timeScale = 0f;

        // This hides the inventory UI when the game ends
        inventory.gameObject.SetActive(false);

        // This displays the caught image after you collect all 4 pieces and "ends" the game
        caughtImage.SetActive(true);
    }
}