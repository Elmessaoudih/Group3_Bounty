/**
 * This was for the door that we didn't have time to implement it, the only reason I didn't delete it because I figured it would be unfair to George since he
 * made it and I didn't want to delete a part of his contribution. So I just commented it out since its not needed. It doesn't do anything but just to be safe 
 * I figured I would comment it out to prevent any glitches or bugs from happening - Trey
using UnityEngine;
public class DoorScript : MonoBehaviour
{
    GameObject doorOpenObject;
    GameObject doorClosedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorOpenObject = gameObject.transform.GetChild(1).gameObject;
        doorClosedObject = gameObject.transform.GetChild(0).gameObject;
        doorOpenObject.SetActive(true);
        //gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Call this to open door when bounties collected
    void OpenDoor()
    { 
        doorOpenObject.SetActive(true);
        doorClosedObject.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
**/