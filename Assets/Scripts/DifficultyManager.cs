using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int scorePerLevel = 50; // Increase level every 50 points
    private float baseRotationSpeed =100f;

    private ObjectEmergenceManager emergenceManager;
    private ScoreSystem scoreSystem;
    private PlanetRotation planetRotation;

    void OnEnable()
    {
        GameManager.OnGameRestart += ResetCurrentLevel;
    }

    void OnDisable()
    {
        GameManager.OnGameRestart -= ResetCurrentLevel;
    }

    void Start()
    {
        emergenceManager = GetComponent<ObjectEmergenceManager>();
        scoreSystem = FindObjectOfType<ScoreSystem>();
        planetRotation = transform.parent.GetComponent<PlanetRotation>();
        UpdateDifficulty();
    }

    void Update()
    {
        int newLevel = (scoreSystem.GetCurrentScore() / scorePerLevel) + 1;
        if (newLevel > currentLevel)
        {
            currentLevel = newLevel;
            UpdateDifficulty();
        }
    }

    void UpdateDifficulty()
    {
        int levelCycle = (currentLevel - 1) % 30;
        int cycleCount = (currentLevel - 1) / 30;

        emergenceManager.numberOfObjectsToEmerge = GetNumberOfObjects(levelCycle);
        emergenceManager.pauseDuration = GetPauseDuration(levelCycle);
        emergenceManager.moveSpeed = GetMoveSpeed(levelCycle);
        planetRotation.rotationSpeed = GetRotationSpeed(cycleCount);
    }

    int GetNumberOfObjects(int levelCycle)
    {
        // Increment the number of objects up to 10, then reset to 3
        return 3 + levelCycle / 3;
    }

    float GetPauseDuration(int levelCycle)
    {
        // Cycle through pause durations: 2f, 1f, 0f
        return (levelCycle % 3) switch
        {
            0 => 2f,
            1 => 1f,
            2 => 0f,
            _ => 2f,
        };
    }

    float GetMoveSpeed(int levelCycle)
    {
        // Cycle through move speeds: 2f, 4f, 8f
        return (levelCycle % 3) switch
        {
            0 => 2f,
            1 => 4f,
            2 => 8f,
            _ => 2f,
        };
    }

    float GetRotationSpeed(int cycleCount)
    {
        // Increase rotation speed by 1.5x each time the cycle of objects 3-10 resets
        return baseRotationSpeed * Mathf.Pow(1.5f, cycleCount);
    }

    public float GetObstacleMoveSpeed()
    {
        return emergenceManager.moveSpeed;
    }

    public void ResetCurrentLevel()
    {
        currentLevel = 1;
    }
}
