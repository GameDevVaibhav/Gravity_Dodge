using UnityEngine;

public class RocketSpiralMovement : MonoBehaviour
{
    public float moveSpeed = 2f;       // Speed of upward movement
    public float yRotationSpeed = 30f; // Speed of gradual rotation on the Y axis
    public float fixedXAngle = 30f;    // Fixed angle on the X axis
    public float moveDistance = 10f;   // Distance to move upwards
    public float speedMultiplier = 1f;
    private Vector3 startPosition;
    private bool isMoving = true;

    private DifficultyManager difficultyManager;

    private void Awake()
    {
        difficultyManager=FindObjectOfType<DifficultyManager>();
    }

    void Start()
    {
        // Save the starting position of the rocket
        startPosition = transform.position;

        float emergenceSpeed = difficultyManager.GetObstacleMoveSpeed();

        if(emergenceSpeed == 2f ) { moveSpeed = 1f; }
        if (emergenceSpeed == 4f) { moveSpeed = 1.5f; }
        if (emergenceSpeed == 8f) { moveSpeed = 2f; }
        // Set the fixed X angle for the rocket
        // transform.rotation = Quaternion.Euler(fixedXAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void Update()
    {
        if (isMoving)
        {
            // Move the rocket forward along its local Y axis (which is tilted by the fixed X angle)
            transform.Translate(Vector3.up * moveSpeed*speedMultiplier * Time.deltaTime,Space.Self);

            // Gradually rotate the rocket on the Y axis
            //transform.Rotate(Vector3.up, yRotationSpeed*speedMultiplier * Time.deltaTime, Space.World);

            // Check if the rocket has moved the desired distance
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                isMoving = false;
                Destroy(gameObject);
            }
        }
    }
}
