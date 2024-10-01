using DG.Tweening;
using System.IO;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    public TextMeshProUGUI scoreText;  // Reference to the UI text displaying the score
    public float scoreSpeed = 1f;      // Speed at which the score increases (units per second)
    public float speedUp = 20f;
    private float orignalSpeed;
    private Color orignalTextColor;

    public float currentScore = 0f;   // Current score
    private float scoreInterval;       // Interval at which the score increases
    private bool isGamePlaying = false;
    private bool collectiblesSpawned = false;  // To track if collectibles have already spawned


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        orignalSpeed = scoreSpeed;
        orignalTextColor=scoreText.color;
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

    public void ScoreSpeedup(bool isSpeedBoost)
    {
        if (isSpeedBoost)
        {
            scoreSpeed = speedUp;

            scoreText.color = Color.yellow;

            // Create the pop-up effect
            scoreText.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f) // Scale up
                .OnComplete(() => scoreText.transform.DOScale(Vector3.one, 0.5f)); // Scale back to original
        }
        else
        {
            scoreSpeed = orignalSpeed;

            scoreText.color = orignalTextColor;
        }
        
    }
    
}
