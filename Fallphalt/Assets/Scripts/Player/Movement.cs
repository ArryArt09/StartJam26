using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Header("Movement")]
    public float moveSpeed = 8f;
    private float horizontalInput;
    private bool isFacingRight = true;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public float checkRadius = 0.2f;
    public float jumpCutMultiplier = 0.5f; // For Hollow Knight short hops
    private bool isGrounded;

    [Header("Hollow Knight Wall Slide")]
    public float wallSlideSpeed = 2.5f;
    private bool isWallSliding;
    private bool isTouchingWall;

    [Header("Hollow Knight Wall Jump")]
    public Vector2 wallJumpForce = new Vector2(14f, 16f);
    public float wallJumpDuration = 0.15f;
    private bool isWallJumping;
    private float wallJumpTimer;
    private float wallJumpDirection;

    void Update()
    {
        // 1. Get Inputs (Using GetAxisRaw for instant snappy response)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Physics Environment Checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);

        // 3. Handle Timers
        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0f)
            {
                isWallJumping = false;
            }
        }

        // 4. Handle Jump and Wall Jump Inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                NormalJump(); // Fixed: Defined below at line 91
            }
            else if (isWallSliding)
            {
                InitiateWallJump(); // Fixed: Defined below at line 112
            }
        }

        // 5. Button Release Mid-Air (Cuts jump height short if released early)
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f && !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        // 6. Handle Sprite Flipping (Only when not locked in a wall jump)
        if (!isWallJumping)
        {
            FlipController();
        }
    }

    void FixedUpdate()
    {
        if (isWallJumping)
        {
            // Lock horizontal control during the snappy kick-off
            rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y);
        }
        else
        {
            // Handle regular horizontal movement and wall sliding
            Move();
            HandleWallSlide();
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void NormalJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void HandleWallSlide()
    {
        bool pushingIntoWall = (isFacingRight && horizontalInput > 0) || (!isFacingRight && horizontalInput < 0);

        if (isTouchingWall && !isGrounded && pushingIntoWall)
        {
            isWallSliding = true;

            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void InitiateWallJump()
    {
        isWallJumping = true;
        wallJumpTimer = wallJumpDuration;

        // Force direction away from current wall position
        wallJumpDirection = isFacingRight ? -1f : 1f;

        // Instant visual face-away turn
        Flip();

        rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y);
    }

    private void FlipController()
    {
        if (isFacingRight && horizontalInput < 0f)
        {
            Flip();
        }
        else if (!isFacingRight && horizontalInput > 0f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}