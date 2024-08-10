using UnityEngine;

public class ObjectEmergenceManager : MonoBehaviour
{
    public GameObject[] objectsToOscillate;
    public float emergenceDistance = 1f;
    public float moveSpeed = 2f;
    public float pauseDuration = 2f;
    public Transform planetCenter;
    public int numberOfObjectsToEmerge = 6;

    private GameObject[] set1Objects;
    private GameObject[] set2Objects;

    void Start()
    {
        SelectAndStartMovement();
    }

    void Update()
    {
        // Update movements for the first set
        if (CheckAllObjectsAtInitialPositions(set1Objects))
        {
            // Transition from Set 1 to Set 2
            TransitionToNextSet();
        }
    }

    public void AssignSpawnedObjectsToOscillateArray(GameObject[] spawnedObjects)
    {
        objectsToOscillate = spawnedObjects;
        Debug.Log("Objects assigned to oscillate array.");
    }

    private void SelectAndStartMovement()
    {
        set1Objects = SelectRandomObjects(objectsToOscillate, numberOfObjectsToEmerge);
        set2Objects = SelectRandomObjects(objectsToOscillate, numberOfObjectsToEmerge);

        // Notify the standby objects to activate their indicators
        foreach (GameObject obj in set2Objects)
        {
            obj.GetComponent<StandbyIndicator>().SetStandby(true);
        }

        ResetMovementStatus(set1Objects);
        DebugStandbySet();
    }

    private void TransitionToNextSet()
    {
        // Deactivate standby indicators for Set1 and Set2
        foreach (GameObject obj in set1Objects)
        {
            obj.GetComponent<StandbyIndicator>().SetStandby(false);
        }

        foreach (GameObject obj in set2Objects)
        {
            obj.GetComponent<StandbyIndicator>().SetStandby(false);
        }

        // Set2 becomes the new Set1
        set1Objects = set2Objects;

        // Select a new set for Set2 and activate their indicators
        set2Objects = SelectRandomObjects(objectsToOscillate, numberOfObjectsToEmerge);
        foreach (GameObject obj in set2Objects)
        {
            obj.GetComponent<StandbyIndicator>().SetStandby(true);
        }

        ResetMovementStatus(set1Objects);
    }

    private GameObject[] SelectRandomObjects(GameObject[] objects, int numberToSelect)
    {
        GameObject[] selectedObjects = new GameObject[numberToSelect];
        System.Random random = new System.Random();
        int count = 0;

        while (count < numberToSelect)
        {
            int randomIndex = random.Next(objects.Length);
            GameObject selectedObject = objects[randomIndex];

            if (!System.Array.Exists(selectedObjects, obj => obj == selectedObject))
            {
                selectedObjects[count] = selectedObject;
                count++;
            }
        }

        return selectedObjects;
    }

    private void ResetMovementStatus(GameObject[] selectedObjects)
    {
        foreach (GameObject selectedObject in selectedObjects)
        {
            OscillatingObject oscillatingObject = selectedObject.GetComponent<OscillatingObject>();
            oscillatingObject.Initialize(planetCenter, emergenceDistance, moveSpeed, pauseDuration);
        }
    }

    private bool CheckAllObjectsAtInitialPositions(GameObject[] selectedObjects)
    {
        foreach (GameObject selectedObject in selectedObjects)
        {
            if (!selectedObject.GetComponent<OscillatingObject>().IsAtInitialPosition())
            {
                return false;
            }
        }
        return true;
    }

    private void DebugStandbySet()
    {
        Debug.Log("Standby Set Selected:");
        foreach (GameObject obj in set2Objects)
        {
            Debug.Log(obj.name);
        }
    }
}
