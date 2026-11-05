using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target; // Drag your player here in the Inspector

    [Header("Settings")]
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Default 2D camera distance
    public float smoothTime = 0.25f; // Time taken to catch up to the player

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate where the camera wants to move
            Vector3 targetPosition = target.position + offset;

            // Smoothly move the camera from its current position to the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
    }
}
