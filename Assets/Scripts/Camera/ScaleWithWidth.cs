using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleWithWidth : MonoBehaviour
{
    public int width = 750;
    public float pixelsToUnits = 100;
    public float minOrthographicSize = 11f; // Minimum orthographic size

    // Start is called before the first frame update
    void Start()
    {
        UpdateCameraSize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        int height = Mathf.RoundToInt(width / (float)Screen.width * Screen.height);

        Camera cam = Camera.main;
        cam.orthographicSize = Mathf.Max(height / pixelsToUnits / 2, minOrthographicSize);
    }
}
