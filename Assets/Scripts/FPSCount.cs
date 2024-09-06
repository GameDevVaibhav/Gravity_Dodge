using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCount : MonoBehaviour
{
    public Text fpsText; // Reference to a UI Text component to display the FPS count
    public float refreshRate = 0.5f; // How often (in seconds) the FPS display should be updated

    private float timer;
    private int frameCount;
    private float deltaTime;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;
        timer += Time.unscaledDeltaTime;

        if (timer >= refreshRate)
        {
            float fps = frameCount / deltaTime;
            fpsText.text = "FPS: " + Mathf.RoundToInt(fps);

            timer = 0;
            frameCount = 0;
            deltaTime = 0;
        }
    }
}
