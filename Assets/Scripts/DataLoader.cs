using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoader
{
    private static string collectibleFilePath = Application.persistentDataPath + "/collectibleCounts.json";
    private static string highScoreFilePath = Application.persistentDataPath + "/highScore.json";

    // Load collectible counts from file
    public static Dictionary<Collectible.CollectibleType, int> LoadCollectibleCounts()
    {
        if (File.Exists(collectibleFilePath))
        {
            string json = File.ReadAllText(collectibleFilePath);
            CollectibleData data = JsonUtility.FromJson<CollectibleData>(json);

            Dictionary<Collectible.CollectibleType, int> collectibleCounts = new Dictionary<Collectible.CollectibleType, int>();
            foreach (var collectible in data.counts)
            {
                collectibleCounts[collectible.type] = collectible.count;
            }

            Debug.Log("Collectible counts loaded from " + collectibleFilePath);
            return collectibleCounts;
        }
        else
        {
            CreateCollectibleCountsFile();
            return InitializeDefaultCollectibleCounts();
        }
    }

    private static Dictionary<Collectible.CollectibleType, int> InitializeDefaultCollectibleCounts()
    {
        Dictionary<Collectible.CollectibleType, int> defaultCounts = new Dictionary<Collectible.CollectibleType, int>();
        foreach (Collectible.CollectibleType type in System.Enum.GetValues(typeof(Collectible.CollectibleType)))
        {
            defaultCounts[type] = 0;
        }
        return defaultCounts;
    }

    private static void CreateCollectibleCountsFile()
    {
        CollectibleData data = new CollectibleData(InitializeDefaultCollectibleCounts());

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(collectibleFilePath, json);

        Debug.Log("Created Default file for collectibles");
    }

    // Load high score from file
    public static int LoadHighScore()
    {
        if (File.Exists(highScoreFilePath))
        {
            string json = File.ReadAllText(highScoreFilePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            Debug.Log("High score loaded: " + data.highScore);
            return data.highScore;
        }
        else
        {
            CreateHighScoreFile();
            return 0; // Default high score
        }
    }

    private static void CreateHighScoreFile()
    {
        HighScoreData data = new HighScoreData { highScore = 0 };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(highScoreFilePath, json);

        Debug.Log("Created Default file for high score");
    }

    // Save high score to file
    public static void SaveHighScore(int newHighScore)
    {
        HighScoreData data = new HighScoreData { highScore = newHighScore };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(highScoreFilePath, json);

        Debug.Log("High score updated: " + newHighScore);
    }
}


