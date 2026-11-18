using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRegular : MonoBehaviour
{
    // Calls to components character may have
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    // Just to check for layers
    public LayerMask groundLayers;

    // Movement variables
    private Vector2 moveInput;
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 5f;

    // Checks
    private bool isLadder;
    private bool isClimbing;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Continuously read movement (x = horizontal, y = vertical)
        moveInput = UserInput.instance.controls.Player.Move.ReadValue<Vector2>();

        if (UserInput.instance.controls.Player.Jump.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        // Safer sprint check that never gets stuck at double speed
        isSprinting = UserInput.instance.controls.Player.Sprint.IsPressed();

        if (isLadder && Mathf.Abs(moveInput.y) > 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        // Horizontal movement
        rb.velocity = new Vector2(moveInput.x * currentSpeed, rb.velocity.y);

        // Ladder climbing
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, moveInput.y * currentSpeed);
        }
        else
        {
            rb.gravityScale = 3.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    // Here to check if grounded 
    bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}