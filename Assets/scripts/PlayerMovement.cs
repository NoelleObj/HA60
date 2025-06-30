using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;  // Movement speed
    private Rigidbody rb;     // Reference to the Rigidbody component
    private Vector3 movement; // Store movement input

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure the Rigidbody is set to kinematic
        rb.isKinematic = true;
    }

    void Update()
    {
        // Get input from WASD/arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
    }

    void FixedUpdate()
    {
        // Perform the movement in FixedUpdate for smooth physics movement
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 direction)
    {
        if (direction.magnitude > 0)
        {
            // Calculate new position
            Vector3 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;

            // Move the kinematic Rigidbody to the new position
            rb.MovePosition(newPosition);
        }
    }
}
