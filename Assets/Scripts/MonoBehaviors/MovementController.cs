using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public float movementSpeed;
    // holds 2d points
    Vector2 movement = new Vector2();

    Animator animator;

    string animationState = "AnimationState";

    Rigidbody2D rb2D;

    enum CharStates { 

        walkEast = 1,
        walkSouth = 2,
        walkWest = 3,
        walkNorth = 4,
        idleSouth = 5,
        Victory = 6,
    }

    // Start is called before the first frame update
    void Start()
    {
        // get ref to game object component so it doesn't have to be grabbed each time
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void UpdateState()
    {
        if (Input.GetKey(KeyCode.V))
        {
            animator.SetInteger(animationState, (int)CharStates.Victory);
            return;
        }

        if (movement.x > 0)
            animator.SetInteger(animationState, (int)CharStates.walkEast);
        else if (movement.x < 0)
            animator.SetInteger(animationState, (int)CharStates.walkWest);
        else if (movement.y > 0)
            animator.SetInteger(animationState, (int)CharStates.walkNorth);
        else if (movement.y < 0)
            animator.SetInteger(animationState, (int)CharStates.walkSouth);
        else
            animator.SetInteger(animationState, (int)CharStates.idleSouth);

    }
    private void MoveCharacter()
    {
        //get user input
      movement.x = Input.GetAxisRaw("Horizontal");
      movement.y = Input.GetAxisRaw("Vertical");

       //keeps player moving at the same rate of speed, no matter which direction
       movement.Normalize();

        rb2D.velocity = movement * movementSpeed;
    }
}
