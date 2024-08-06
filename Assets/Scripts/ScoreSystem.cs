using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;           // Reference to the UI text displaying the score
    public float scoreSpeed = 1f;    // Speed at which the score increases (units per second)

    private float currentScore = 0f; // Current score
    private float scoreInterval;     // Interval at which the score increases

    void Start()
    {
        // Calculate the interval based on the speed
        scoreInterval = 1f / scoreSpeed;
        UpdateScoreText();
        InvokeRepeating("IncreaseScore", scoreInterval, scoreInterval);
    }

    void IncreaseScore()
    {
        currentScore += 1f;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString();
    }
}
