using UnityEngine;

public class ObjectEmergenceManager : MonoBehaviour
{
    public GameObject[] objectsToOscillate;   // Array of objects to be oscillated
    public float emergenceDistance = 1f;
    public float oscillationFrequency = 1f;
    public Transform planetCenter;             // Reference to the planet's center
    public int numberOfObjectsToEmerge = 6;    // Number of objects to randomly select for emergence

    private Vector3[] initialPositions;        // Array to store initial positions
    private bool[] hasCompletedOscillation;    // Array to track if oscillation is complete for each object
    private GameObject[] selectedObjects;      // Array to store selected objects for emergence
    private float startTime;                   // Time when the current oscillation cycle started

    void Start()
    {
        InitializePositionsAndStatus();
        SelectAndStartOscillation();
    }

    void Update()
    {
        UpdateOscillations();

        if (CheckOscillationCompletion())
        {
            SelectAndStartOscillation();
        }
    }

    // Initialize the positions and oscillation status for all objects
    private void InitializePositionsAndStatus()
    {
        initialPositions = new Vector3[objectsToOscillate.Length];
        hasCompletedOscillation = new bool[objectsToOscillate.Length];

        for (int i = 0; i < objectsToOscillate.Length; i++)
        {
            initialPositions[i] = objectsToOscillate[i].transform.position;
            hasCompletedOscillation[i] = true; // Initially, mark all as completed to avoid them moving until selected
        }
    }

    // Select objects and start oscillation for them
    private void SelectAndStartOscillation()
    {
        selectedObjects = SelectRandomObjects(objectsToOscillate, numberOfObjectsToEmerge);
        ResetOscillationStatus();
        startTime = Time.time; // Reset start time for new cycle
    }

    // Update oscillations for the selected objects
    private void UpdateOscillations()
    {
        foreach (GameObject selectedObject in selectedObjects)
        {
            int objIndex = System.Array.IndexOf(objectsToOscillate, selectedObject);

            if (hasCompletedOscillation[objIndex])
                continue;

            PerformOscillation(selectedObject, objIndex);
        }
    }

    // Perform the oscillation logic for a given object
    private void PerformOscillation(GameObject selectedObject, int objIndex)
    {
        Vector3 direction = (selectedObject.transform.position - planetCenter.position).normalized;
        float elapsed = Time.time - startTime;
        float offset = Mathf.Abs(Mathf.Sin(elapsed * oscillationFrequency) * emergenceDistance);
        Vector3 newPosition = initialPositions[objIndex] + direction * offset;

        if (!float.IsNaN(newPosition.x) && !float.IsNaN(newPosition.y) && !float.IsNaN(newPosition.z))
        {
            selectedObject.transform.position = newPosition;
        }

        // Check if the object has completed one oscillation cycle
        if (elapsed * oscillationFrequency > Mathf.PI)
        {
            hasCompletedOscillation[objIndex] = true;
        }
    }

    // Helper method to randomly select objects from the array
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

    // Reset oscillation status for selected objects
    private void ResetOscillationStatus()
    {
        for (int i = 0; i < hasCompletedOscillation.Length; i++)
        {
            hasCompletedOscillation[i] = true; // Reset all to completed
        }

        foreach (GameObject selectedObject in selectedObjects)
        {
            int objIndex = System.Array.IndexOf(objectsToOscillate, selectedObject);
            hasCompletedOscillation[objIndex] = false; // Only mark selected objects as not completed
        }
    }

    // Check if all selected objects have completed their oscillation
    private bool CheckOscillationCompletion()
    {
        foreach (GameObject selectedObject in selectedObjects)
        {
            int objIndex = System.Array.IndexOf(objectsToOscillate, selectedObject);
            if (!hasCompletedOscillation[objIndex])
            {
                return false;
            }
        }
        return true;
    }
}
