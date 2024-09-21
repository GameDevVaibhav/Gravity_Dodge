using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoader
{
    private static string collectibleFilePath = Application.persistentDataPath + "/collectibleCounts.json";
    private static string highScoreFilePath = Application.persistentDataPath + "/highScore.json";
    private static string planetUnlockFilePath = Application.persistentDataPath + "/planetUnlockedStatus.json";
    private static string vehicleUnlockFilePath = Application.persistentDataPath + "/vehicleUnlockedStatus.json";

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

    // Load planet unlocked status from file
    public static bool[] LoadPlanetUnlockedStatus(int planetCount)
    {
        // Check if the JSON file exists
        if (File.Exists(planetUnlockFilePath))
        {
            string json = File.ReadAllText(planetUnlockFilePath);
            PlanetUnlockData data = JsonUtility.FromJson<PlanetUnlockData>(json);

            // If the planet count in the JSON file is less than the current planet count, update the file
            if (data.planetUnlockedStatus.Length < planetCount)
            {
                bool[] updatedStatus = UpdatePlanetUnlockStatus(data.planetUnlockedStatus, planetCount);
                SavePlanetUnlockedStatus(updatedStatus);
                return updatedStatus;
            }

            Debug.Log("Planet unlock status loaded from " + planetUnlockFilePath);
            return data.planetUnlockedStatus;
        }
        else
        {
            // Create a new file with the default planet unlock status if it doesn't exist
            CreatePlanetUnlockedStatusFile(planetCount);
            return InitializeDefaultPlanetUnlockStatus(planetCount);
        }
    }

    // Method to update the planet unlock status array with new planets set to default (locked)
    private static bool[] UpdatePlanetUnlockStatus(bool[] existingStatus, int planetCount)
    {
        // Create a new array with the updated planet count
        bool[] updatedStatus = new bool[planetCount];

        // Copy existing unlock status values
        for (int i = 0; i < existingStatus.Length; i++)
        {
            updatedStatus[i] = existingStatus[i];
        }

        // Set the new planets as locked (false)
        for (int i = existingStatus.Length; i < planetCount; i++)
        {
            updatedStatus[i] = false;
        }

        Debug.Log("Updated planet unlock status with new planets.");
        return updatedStatus;
    }


    private static bool[] InitializeDefaultPlanetUnlockStatus(int planetCount)
    {
        bool[] defaultStatus = new bool[planetCount];
        defaultStatus[0] = true; // Unlock the first planet by default
        for (int i = 1; i < planetCount; i++)
        {
            defaultStatus[i] = false; // Lock all other planets by default
        }
        return defaultStatus;
    }

    private static void CreatePlanetUnlockedStatusFile(int planetCount)
    {
        bool[] defaultStatus = InitializeDefaultPlanetUnlockStatus(planetCount);
        SavePlanetUnlockedStatus(defaultStatus);

        Debug.Log("Created default planet unlock status file");
    }

    // Save planet unlocked status to file
    public static void SavePlanetUnlockedStatus(bool[] status)
    {
        PlanetUnlockData data = new PlanetUnlockData { planetUnlockedStatus = status };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(planetUnlockFilePath, json);

        Debug.Log("Planet unlock status updated");
    }


    // Load vehicle unlocked status from file
    public static bool[] LoadVehicleUnlockedStatus(int vehicleCount)
    {
        if (File.Exists(vehicleUnlockFilePath))
        {
            string json = File.ReadAllText(vehicleUnlockFilePath);
            VehicleUnlockData data = JsonUtility.FromJson<VehicleUnlockData>(json);

            Debug.Log("Vehicle unlock status loaded from " + vehicleUnlockFilePath);
            return data.vehicleUnlockedStatus;
        }
        else
        {
            CreateVehicleUnlockedStatusFile(vehicleCount);
            return InitializeDefaultVehicleUnlockStatus(vehicleCount);
        }
    }

    private static bool[] InitializeDefaultVehicleUnlockStatus(int vehicleCount)
    {
        bool[] defaultStatus = new bool[vehicleCount];
        defaultStatus[0] = true; // Unlock the first vehicle by default
        for (int i = 1; i < vehicleCount; i++)
        {
            defaultStatus[i] = false; // Lock all other vehicles by default
        }
        return defaultStatus;
    }

    private static void CreateVehicleUnlockedStatusFile(int vehicleCount)
    {
        bool[] defaultStatus = InitializeDefaultVehicleUnlockStatus(vehicleCount);
        SaveVehicleUnlockedStatus(defaultStatus);

        Debug.Log("Created default vehicle unlock status file");
    }

    // Save vehicle unlocked status to file
    public static void SaveVehicleUnlockedStatus(bool[] status)
    {
        VehicleUnlockData data = new VehicleUnlockData { vehicleUnlockedStatus = status };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(vehicleUnlockFilePath, json);

        Debug.Log("Vehicle unlock status updated");
    }
}




