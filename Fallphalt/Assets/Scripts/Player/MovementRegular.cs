using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementRegular : MonoBehaviour
{
    // Calls to components character may have
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    // Just to check for layers
    public LayerMask groundLayers;

    // Movement variables
    private float direction = 0f;
    private bool isGrounded;
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Literally just movement/jumping
        direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetButtonDown("Sprint"))
        {
            moveSpeed *= sprintMultiplier; // Multiply the speed when sprinting
        }
        if (Input.GetButtonUp("Sprint"))
        {
            moveSpeed /= sprintMultiplier; // Divide the speed when not sprinting
        }
    }
    private void FixedUpdate()
    {
        // Physics shtuff
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // Here to check if grounded 
    bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }
}
