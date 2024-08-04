using UnityEngine;

public class ObjectEmergenceManager : MonoBehaviour
{
    public GameObject[] objectsToOscillate;   // Array of objects to be oscillated
    public float emergenceDistance = 1f;      // Distance to emerge from the surface
    public float moveSpeed = 2f;              // Speed of movement for emerging and retracting
    public float pauseDuration = 2f;          // Duration to pause at the target position
    public Transform planetCenter;            // Reference to the planet's center
    public int numberOfObjectsToEmerge = 6;   // Number of objects to randomly select for emergence

    private Vector3[] initialLocalPositions;  // Array to store initial local positions
    private Vector3[] targetPositions;        // Array to store target (emerged) positions
    private bool[] movingOutward;             // Track if the object is moving outward
    private bool[] isPausing;                 // Track if the object is currently pausing
    private float[] pauseStartTime;           // Track the start time of the pause for each object
    private GameObject[] selectedObjects;     // Array to store selected objects for emergence

    void Start()
    {
        InitializePositionsAndStatus();
        SelectAndStartMovement();
    }

    void Update()
    {
        UpdateMovements();

        if (CheckAllObjectsAtInitialPositions())
        {
            SelectAndStartMovement();
        }
    }

    // Initialize the positions and movement status for all objects
    private void InitializePositionsAndStatus()
    {
        int length = objectsToOscillate.Length;
        initialLocalPositions = new Vector3[length];
        targetPositions = new Vector3[length];
        movingOutward = new bool[length];
        isPausing = new bool[length];
        pauseStartTime = new float[length];

        for (int i = 0; i < length; i++)
        {
            // Store the initial positions in local space relative to the planet
            initialLocalPositions[i] = objectsToOscillate[i].transform.localPosition;

            // Calculate the target position (emerged position)
            Vector3 direction = (initialLocalPositions[i] - planetCenter.localPosition).normalized;
            targetPositions[i] = initialLocalPositions[i] + direction * emergenceDistance;

            // Initially, all objects are not moving outward and not pausing
            movingOutward[i] = true;
            isPausing[i] = false;
        }
    }

    // Select objects and start movement for them
    private void SelectAndStartMovement()
    {
        selectedObjects = SelectRandomObjects(objectsToOscillate, numberOfObjectsToEmerge);
        ResetMovementStatus();
    }

    // Update movements for the selected objects
    private void UpdateMovements()
    {
        foreach (GameObject selectedObject in selectedObjects)
        {
            int objIndex = System.Array.IndexOf(objectsToOscillate, selectedObject);

            if (isPausing[objIndex])
            {
                // Check if the pause duration has elapsed
                if (Time.time - pauseStartTime[objIndex] >= pauseDuration)
                {
                    isPausing[objIndex] = false;  // Resume movement back to the initial position
                    movingOutward[objIndex] = false;
                }
                else
                {
                    // Skip movement during the pause
                    continue;
                }
            }

            if (movingOutward[objIndex])
            {
                // Move the object outward (from initial position to target position)
                selectedObject.transform.localPosition = Vector3.MoveTowards(
                    selectedObject.transform.localPosition,
                    targetPositions[objIndex],
                    moveSpeed * Time.deltaTime
                );

                // Check if the object has reached the target position
                if (Vector3.Distance(selectedObject.transform.localPosition, targetPositions[objIndex]) < 0.01f)
                {
                    isPausing[objIndex] = true;  // Start pausing
                    pauseStartTime[objIndex] = Time.time;  // Record pause start time
                }
            }
            else
            {
                // Move the object back to its initial position
                selectedObject.transform.localPosition = Vector3.MoveTowards(
                    selectedObject.transform.localPosition,
                    initialLocalPositions[objIndex],
                    moveSpeed * Time.deltaTime
                );
            }
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

    // Reset movement status for selected objects
    private void ResetMovementStatus()
    {
        for (int i = 0; i < movingOutward.Length; i++)
        {
            movingOutward[i] = true;  // Set all objects to move outward initially
            isPausing[i] = false;    // Ensure no object is pausing initially
        }
    }

    // Check if all selected objects have returned to their initial positions
    private bool CheckAllObjectsAtInitialPositions()
    {
        foreach (GameObject selectedObject in selectedObjects)
        {
            int objIndex = System.Array.IndexOf(objectsToOscillate, selectedObject);
            if (Vector3.Distance(selectedObject.transform.localPosition, initialLocalPositions[objIndex]) >= 0.01f)
            {
                return false;
            }
        }
        return true;
    }
}
