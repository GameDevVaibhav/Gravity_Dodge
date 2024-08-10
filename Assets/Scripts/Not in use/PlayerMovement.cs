using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform planet;  // Reference to the planet (center of gravity)
    public float speed = 5f;  // Movement speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculate direction towards the center of the planet
        Vector3 gravityDirection = (planet.position - transform.position).normalized;

        // Align the object's up direction to the surface normal
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = (-transform.forward * vertical + transform.right * horizontal).normalized;

        // Apply movement
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
