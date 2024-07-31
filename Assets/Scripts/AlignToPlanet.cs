using UnityEngine;

public class AlignToPlanet : MonoBehaviour
{
    public Transform planetCenter;     // The center of the planet
    public GameObject[] objectsToAlign; // Array of objects to align to the planet's surface
    public float distanceFromCenter = 3.7f;  // Distance from the center of the planet to the object's surface

    // This method aligns all objects in the array to the planet's surface and positions them correctly
    public void AlignAndPositionObjects()
    {
        foreach (GameObject obj in objectsToAlign)
        {
            // Calculate direction from object to planet center
            Vector3 directionToCenter = (planetCenter.position - obj.transform.position).normalized;

            // Align the object's Y axis to point to the center of the planet
            obj.transform.rotation = Quaternion.FromToRotation(Vector3.down, directionToCenter);

            // Position the object at the specified distance from the center of the planet
            obj.transform.position = planetCenter.position - directionToCenter * distanceFromCenter;
        }
    }
}
