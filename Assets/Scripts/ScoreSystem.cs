using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Reference to the UI text displaying the score
    public float scoreSpeed = 1f;      // Speed at which the score increases (units per second)

    public float currentScore = 0f;   // Current score
    private float scoreInterval;       // Interval at which the score increases
    private bool isGamePlaying = false;
    private bool collectiblesSpawned = false;  // To track if collectibles have already spawned

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

            // Check if the score has exceeded 50 and spawn collectibles if not already spawned
            if (currentScore > 50f && !collectiblesSpawned)
            {
                SpawnInitialCollectibles();
                collectiblesSpawned = true;
            }
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
        scoreText.text = Mathf.FloorToInt(currentScore).ToString();
    }

    public void ResetScore()
    {
        currentScore = 0f;
        scoreText.text = "";
        collectiblesSpawned = false;
    }

    void SpawnInitialCollectibles()
    {
        // Find the CollectibleSpawner in the scene
        CollectibleSpawner spawner = FindObjectOfType<CollectibleSpawner>();
        if (spawner != null)
        {
            spawner.InitialSpawn();
        }
        else
        {
            Debug.LogError("No CollectibleSpawner found in the scene.");
        }
    }

    public int GetCurrentScore()
    {
        return (int)currentScore;
    }
}
