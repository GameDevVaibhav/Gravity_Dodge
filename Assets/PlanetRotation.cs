using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Rotation speed
    public Transform objectOnPlanet;  // Reference to the object on the planet

    private Vector3 currentRotationDirection = Vector3.zero;  // Current rotation direction

    void Update()
    {
        // Get input for rotation adjustments
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Update the current rotation direction based on input
        if (horizontal != 0 || vertical != 0)
        {
            currentRotationDirection = new Vector3(vertical, 0, -horizontal).normalized;
        }

        // Apply continuous rotation based on the current rotation direction
        transform.Rotate(-currentRotationDirection, rotationSpeed * Time.deltaTime, Space.World);

        // Ensure the object stays on the surface of the planet
        Vector3 direction = (objectOnPlanet.position - transform.position).normalized;
        objectOnPlanet.position = transform.position + direction * (transform.localScale.x / 2);
    }
}
