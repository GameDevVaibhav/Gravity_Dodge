using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Make the object face the camera by using LookAt
        transform.LookAt(mainCamera.transform);

        // Optionally, flip the object to face the camera directly (if needed)
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
