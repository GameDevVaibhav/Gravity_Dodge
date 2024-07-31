using UnityEngine;

public class ObjectEmergence : MonoBehaviour
{
    public float emergenceSpeed = 2f;   // Speed of emergence
    public float emergenceAmplitude = 1f; // Maximum distance the object will move along the y-axis
    private Vector3 initialLocalPosition;

    void Start()
    {
        initialLocalPosition = transform.localPosition; // Store the initial local position of the object
        emergenceAmplitude = gameObject.transform.localScale.y;
    }

    void Update()
    {
        // Calculate the new y position using a sine wave
        float newY = Mathf.Sin(Time.time * emergenceSpeed) * emergenceAmplitude;

        // Calculate the translation needed from the initial local position
        float deltaY = newY - (transform.localPosition.y - initialLocalPosition.y);

        // Translate the object along its local y-axis
        transform.Translate(new Vector3(0, deltaY, 0), Space.Self);
    }
}
