using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Reference to the UI text displaying the score
    public float scoreSpeed = 1f;      // Speed at which the score increases (units per second)

    private float currentScore = 0f;   // Current score
    private float scoreInterval;       // Interval at which the score increases
    private bool isGamePlaying = false;

    void Start()
    {
        // Calculate the interval based on the speed
        scoreInterval = 1f / scoreSpeed;
        UpdateScoreText();
    }

    void Update()
    {
        if (isGamePlaying)
        {
            IncreaseScore();
        }
    }

    public void StartScore()
    {
        isGamePlaying = true;
    }

    public void StopScore()
    {
        isGamePlaying = false;
    }

    void IncreaseScore()
    {
        currentScore += Time.deltaTime * scoreSpeed;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString();
    }
}
