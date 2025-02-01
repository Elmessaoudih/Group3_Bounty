using System.Collections;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the Player in units per second
    public float jumpForce = 10f;
    public float jumpDuration = 1f; // Duration of the jump, in seconds
    public float jumpCooldown = 1f;  // Cooldown between jumps, in seconds

    private Rigidbody2D rb; // Reference to Player's rigidBody2D
    private Animator animator; // Reference to Player's Animator

    private Vector2 moveDirection = new Vector2(); // A normalized vector in the direction the Player is moving

    private bool isGrounded = true;      // Track if the player is grounded
    private bool canJump = true;        // Track if the player is allowed to jump
    private string originalLayer;       // Store the player's original layer
    private Vector3 originalScale;      // Store the player's original scale
    private BoxCollider2D playerCollider; // Store Player's Box Collider

    string animationState = "AnimationState"; // Name of the integer variable controlling Player animations

    // Enumeration with constants representing the different animationStates
    enum CharStates
    {
        walkEast = 1,
        walkWest = 2,
        walkNorth = 3,
        walkSouth = 4,
        idleSouth = 5
    }

    private void Start()
    {
        // Initialize component references
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        // Save the original layer and scale
        originalLayer = LayerMask.LayerToName(gameObject.layer);
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (GameManager.instance.freezeGame)
        {
            rb.velocity = Vector2.zero;
            animator.SetInteger(animationState, (int)CharStates.idleSouth);
            return;
        }

        SetAnimState();

        // Handle jump input
        if (Input.GetButtonDown("Jump") && canJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.freezeGame)
            return;

        ProcessInputs();
        Move();
    }

    private void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void SetAnimState()
    {
        if (moveDirection.x > 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkEast);
        }
        else if (moveDirection.x < 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkWest);
        }
        else if (moveDirection.y > 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkNorth);
        }
        else if (moveDirection.y < 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkSouth);
        }
        else
        {
            animator.SetInteger(animationState, (int)CharStates.idleSouth);
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void Jump()
    {
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        // Begin jump
        canJump = false;
        isGrounded = false;

        // Grow Player, give illusion of being closer to screen
        transform.localScale = originalScale * 1.2f;

        // Change layer to Airborne
        gameObject.layer = LayerMask.NameToLayer("Airborne");

        // Apply jump force
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Wait for the jump duration
        yield return new WaitForSeconds(jumpDuration);

        // End jump
        transform.localScale = originalScale; // Restore original scale

        // Check for collision with Blocking layer and resolve it
        if (IsInsideObstacle())
        {
            MoveCloserToCenter();
        }

        gameObject.layer = LayerMask.NameToLayer(originalLayer); // Restore original Layer

        isGrounded = true;

        // Wait for cooldown before allowing another jump
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    // Checks if the player overlaps with any Obstacles, true if so, false if not
    private bool IsInsideObstacle()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, playerCollider.size.x/2);
        foreach (var collider in overlaps)
        {
            if (collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }

    // Corrects the Player towards the center TODO
    private void MoveCloserToCenter()
    {
        // Move the player 1.5 units closer to the center of the map (0, 0)
        Vector2 directionToCenter = (Vector2.zero - (Vector2)transform.position).normalized;
        transform.position += (Vector3)directionToCenter * 1.5f; // Move 1.5 units
    }
}
