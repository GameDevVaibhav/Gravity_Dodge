using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public Transform planet;  // Reference to the planet (center of gravity)
    public float gravity = 9.8f;  // Gravity force

    void FixedUpdate()
    {
        // Calculate direction towards the center of the planet
        Vector3 direction = (planet.position - transform.position).normalized;

        // Apply gravity force towards the center
        GetComponent<Rigidbody>().AddForce(direction * gravity);

        // Align the object to the surface normal
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, direction) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
