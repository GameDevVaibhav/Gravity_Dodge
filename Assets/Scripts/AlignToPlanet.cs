using UnityEngine;

public class AlignToPlanet : MonoBehaviour
{
    public Transform planetCenter;    // The center of the planet
    public GameObject[] objectsToAlign; // Array of objects to align to the planet's surface

    // This method will align all objects in the array to the planet's surface
    public void AlignObjectsToPlanet()
    {
        foreach (GameObject obj in objectsToAlign)
        {
            Vector3 directionToCenter = (planetCenter.position - obj.transform.position).normalized;
            obj.transform.rotation = Quaternion.FromToRotation(Vector3.down, directionToCenter);
        }
    }
}
