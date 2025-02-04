using UnityEngine;

public class Player : Character
{
    public Inventory inventoryPrefab;
    Inventory inventory;

    public UIManager uiManager;
    private GameObject burriedMapPieceInRange;

    // This is calling the CaughtImage in the Canvas
    public GameObject caughtImage;

    void Start()
    {
        inventory = Instantiate(inventoryPrefab);
        // This makes the image that says you beat the game not visible when the game starts
        caughtImage.SetActive(false);
    }

    void Update()
    {
        if (burriedMapPieceInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Dug up: " + burriedMapPieceInRange.name);
            inventory.AddItem(burriedMapPieceInRange.GetComponent<PickUpMap>().piece);
            burriedMapPieceInRange.SetActive(false);
            burriedMapPieceInRange = null;
            UIManager.instance.HideInteractionPrompt();

            if (inventory.IsFull())
            {
                StopGame();
            }
        }
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

        if (collision.CompareTag("Dig"))
        {
            burriedMapPieceInRange = collision.gameObject;
            UIManager.instance.ShowInteractionPrompt("Press E to dig up the map piece!");
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == burriedMapPieceInRange)
        {
            burriedMapPieceInRange = null;
            UIManager.instance.HideInteractionPrompt();
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