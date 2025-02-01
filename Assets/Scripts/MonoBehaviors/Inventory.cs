using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Reference to the slot prefab object
    public GameObject slotPrefab;

    // Number of slots the inventory bar contains
    public const int numSlots = 4;

    // Holds references to slot prefab
    GameObject[] slots = new GameObject[numSlots];

    // Array for item images
    Image[] mapImages = new Image[numSlots];

    // Holds reference to the actual scriptable objects that are picked up
    MapPiece[] piece = new MapPiece[numSlots];

    // Start is called before the first frame update
    private void Start()
    {
        CreateSlots();
    }

    public void CreateSlots()
    {
        // Make sure that the slot prefab has been set in the Unity editor
        if (slotPrefab != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                // Create a new Slot game object and give it a name
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "MapSlot_" + i;

                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);

                slots[i] = newSlot;

                mapImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    // Adds items to inventory
    public bool AddItem(MapPiece itemToAdd)
    {
        for (int i = 0; i < piece.Length; i++)
        {
            if (piece[i] == null)
            {
                // Copy item and adds to inventory
                piece[i] = Instantiate(itemToAdd);
                piece[i].pieceQuantity = 1;
                mapImages[i].sprite = itemToAdd.sprite;
                mapImages[i].enabled = true;

                return true;
            }
        }
        // When the inventory is full it returns false
        return false;
    }

    // This is a method for the Player class to call to check if the inventory is full
    public bool IsFull()
    {
        foreach (var item in piece)
        {
            // Returns false if the inventory isn't full
            if (item == null) return false;
        }
        return true; 
    }
}
