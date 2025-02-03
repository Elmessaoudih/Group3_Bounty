using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates an entry in the create submenu to create instances of the object
[CreateAssetMenu(menuName = "Map")]

public class MapPiece : ScriptableObject
{
    public string objectName; 

    //reference to item's sprite
    public Sprite sprite;

    //quantity of the item
    public int pieceQuantity;

}
