using UnityEngine;

public class AlignToPlanet : MonoBehaviour
{
    public Transform planetCenter;         // The center of the planet
    public GameObject[] objectsToAlign;    // Array of objects to align to the planet's surface
    public float distanceFromCenter = 3.7f; // Distance from the center of the planet to the object's surface

    // This method aligns all objects in the array to the planet's surface and positions them correctly
    public void AlignAndPositionObjects()
    {
        foreach (GameObject obj in objectsToAlign)
        {
            AlignObject(obj);
        }
    }

    // This method aligns and positions an individual object
    private void AlignObject(GameObject obj)
    {
        Vector3 directionToCenter = (planetCenter.position - obj.transform.position).normalized;
        obj.transform.rotation = Quaternion.FromToRotation(Vector3.down, directionToCenter);
        obj.transform.position = planetCenter.position - directionToCenter * distanceFromCenter;
    }

    // This method randomly distributes objects on the planet's surface
    public void DistributeObjectsRandomly()
    {
        foreach (GameObject obj in objectsToAlign)
        {
            // Generate a random direction vector
            Vector3 randomDirection = Random.onUnitSphere;

            // Calculate the new position based on the random direction
            Vector3 newPosition = planetCenter.position + randomDirection * distanceFromCenter;
            obj.transform.position = newPosition;

            // Align the object to the surface of the planet
            AlignObject(obj);
        }
    }
}
