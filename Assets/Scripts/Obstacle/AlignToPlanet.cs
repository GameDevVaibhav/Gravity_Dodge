using UnityEngine;

public class AlignToPlanet : MonoBehaviour
{
    public Transform planetCenter;              // The center of the planet
    public GameObject objectPrefab;             // Prefab of the object to align and distribute
    public int numberOfObjectsToSpawn = 10;     // Number of objects to spawn
    public float distanceFromCenter = 3.7f;     // Distance from the center of the planet to the object's surface

    private GameObject[] spawnedObjects;        // Array to store the spawned objects

    // This method spawns objects and aligns them to the planet's surface
    public void AlignAndPositionObjects()
    {
        SpawnObjects();
        foreach (GameObject obj in spawnedObjects)
        {
            AlignObject(obj);
        }
    }

    // This method spawns objects and randomly distributes them on the planet's surface
    public void DistributeObjectsRandomly()
    {
        SpawnObjects();
        foreach (GameObject obj in spawnedObjects)
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

    // This method spawns objects and distributes them equally over the entire surface of the planet
    public void DistributeObjectsEqually()
    {
        SpawnObjects();
        int objectCount = spawnedObjects.Length;
        float phi = Mathf.PI * (3 - Mathf.Sqrt(5)); // Golden angle in radians

        for (int i = 0; i < objectCount; i++)
        {
            float y = 1 - (i / (float)(objectCount - 1)) * 2;  // y goes from 1 to -1
            float radius = Mathf.Sqrt(1 - y * y);  // radius at y

            float theta = phi * i;  // golden angle increment

            float x = Mathf.Cos(theta) * radius;
            float z = Mathf.Sin(theta) * radius;

            Vector3 direction = new Vector3(x, y, z);
            Vector3 newPosition = planetCenter.position + direction * distanceFromCenter;
            spawnedObjects[i].transform.position = newPosition;

            // Align the object to the surface of the planet
            AlignObject(spawnedObjects[i]);
        }
    }

    // This method spawns the objects and stores them in the array
    private void SpawnObjects()
    {
        // Destroy previously spawned objects if any
        if (spawnedObjects != null)
        {
            foreach (GameObject obj in spawnedObjects)
            {
                DestroyImmediate(obj);
            }
        }

        // Spawn the specified number of objects
        spawnedObjects = new GameObject[numberOfObjectsToSpawn];
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            spawnedObjects[i] = Instantiate(objectPrefab);
            spawnedObjects[i].transform.SetParent(transform); // Parent them to the current GameObject for organization
        }
    }

    // This method aligns and positions an individual object
    private void AlignObject(GameObject obj)
    {
        Vector3 directionToCenter = (planetCenter.position - obj.transform.position).normalized;
        obj.transform.rotation = Quaternion.FromToRotation(Vector3.down, directionToCenter);
        obj.transform.position = planetCenter.position - directionToCenter * distanceFromCenter;
    }

    public GameObject[] GetSpawnedObjects()
    {
        return spawnedObjects;
    }

}
