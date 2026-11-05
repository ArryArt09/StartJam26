using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRegular : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    private float horizontalInput;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    private bool isGrounded;

    [Header("Components")]
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Gather horizontal input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Check if player is on the ground
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // 3. Handle Jumping input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 4. Flip player sprite based on movement direction
        FlipSprite();
    }

    void FixedUpdate()
    {
        // Apply horizontal physics movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void FlipSprite()
    {
        if (horizontalInput > 0f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (horizontalInput < 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizes the ground check box in the Unity Editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we touched has the tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
