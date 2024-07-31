using UnityEngine;

public class ObjectEmergence : MonoBehaviour
{
    public float emergenceSpeed = 2f;      // Speed at which the object emerges
    public float emergenceDistance = 1f;   // Maximum distance the object moves outward
    public float oscillationFrequency = 1f; // Speed of oscillation

    private Vector3 initialPosition;        // Store the initial position
    [SerializeField]
    private Transform planetCenter;         // Reference to the planet's center

    void Start()
    {
        initialPosition = transform.position;  // Save the initial position
       // planetCenter = GameObject.Find("PlanetCenter").transform; // Assuming there's a GameObject named "PlanetCenter"
    }

    void Update()
    {
        // Calculate the direction from the planet center to the object
        Vector3 direction = (transform.position - planetCenter.position).normalized;

        // Oscillate the object along its local Y-axis
        float offset = Mathf.Sin(Time.time * oscillationFrequency) * emergenceDistance;

        // Calculate the new position
        Vector3 newPosition = initialPosition + direction * offset;

        // Safeguard against NaN values
        if (!float.IsNaN(newPosition.x) && !float.IsNaN(newPosition.y) && !float.IsNaN(newPosition.z))
        {
            transform.position = newPosition;
        }
    }
}
