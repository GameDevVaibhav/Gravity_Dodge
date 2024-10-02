using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoader
{
    private static string collectibleFilePath = Application.persistentDataPath + "/collectibleCounts.json";
    private static string highScoreFilePath = Application.persistentDataPath + "/highScore.json";
    private static string planetUnlockFilePath = Application.persistentDataPath + "/planetUnlockedStatus.json";
    private static string vehicleUnlockFilePath = Application.persistentDataPath + "/vehicleUnlockedStatus.json";
    private static string encryptionKey = "51434879"; 

    public static Dictionary<Collectible.CollectibleType, int> LoadCollectibleCounts()
    {
        if (File.Exists(collectibleFilePath))
        {
            string encryptedJson = File.ReadAllText(collectibleFilePath);
            string json = Decrypt(encryptedJson);
            CollectibleData data = JsonUtility.FromJson<CollectibleData>(json);

            Dictionary<Collectible.CollectibleType, int> collectibleCounts = new Dictionary<Collectible.CollectibleType, int>();

            // Load existing collectible counts from file
            foreach (var collectible in data.counts)
            {
                collectibleCounts[collectible.type] = collectible.count;
            }

            // Check if new collectible types have been added
            foreach (Collectible.CollectibleType type in System.Enum.GetValues(typeof(Collectible.CollectibleType)))
            {
                if (!collectibleCounts.ContainsKey(type))
                {
                    collectibleCounts[type] = 0; // Initialize new collectible types with a count of 0
                }
            }



            // Save the updated collectible counts with new types added
            SaveCollectibleCounts(collectibleCounts);

            Debug.Log("Collectible counts loaded and updated from " + collectibleFilePath);
            return collectibleCounts;
        }
        else
        {
            CreateCollectibleCountsFile();
            return InitializeDefaultCollectibleCounts();
        }
    }

    // Save collectible counts to file
    public static void SaveCollectibleCounts(Dictionary<Collectible.CollectibleType, int> collectibleCounts)
    {
        CollectibleData data = new CollectibleData(collectibleCounts);
        string json = JsonUtility.ToJson(data, true);

        // Debug before encryption
        Debug.Log("JSON before encryption: " + json);

        string encryptedJson = Encrypt(json);

        // Debug after encryption
        Debug.Log("Encrypted JSON: " + encryptedJson);

        File.WriteAllText(collectibleFilePath, encryptedJson);

        Debug.Log("Collectible counts updated");
    }

    private static Dictionary<Collectible.CollectibleType, int> InitializeDefaultCollectibleCounts()
    {
        Dictionary<Collectible.CollectibleType, int> defaultCounts = new Dictionary<Collectible.CollectibleType, int>();

        foreach (Collectible.CollectibleType type in System.Enum.GetValues(typeof(Collectible.CollectibleType)))
        {
            defaultCounts[type] = 0; // Initialize all collectible types with a count of 0
        }

        return defaultCounts;
    }

    private static void CreateCollectibleCountsFile()
    {
        Dictionary<Collectible.CollectibleType, int> defaultCounts = InitializeDefaultCollectibleCounts();
        SaveCollectibleCounts(defaultCounts);

        Debug.Log("Created default collectible counts file");
    }

    // Load high score from file
    public static int LoadHighScore()
    {
        if (File.Exists(highScoreFilePath))
        {
            string encryptedJson = File.ReadAllText(highScoreFilePath);
            string json = Decrypt(encryptedJson);
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
        string encryptedJson = Encrypt(json);
        File.WriteAllText(highScoreFilePath, encryptedJson);

        Debug.Log("Created Default file for high score");
    }

    // Save high score to file
    public static void SaveHighScore(int newHighScore)
    {
        HighScoreData data = new HighScoreData { highScore = newHighScore };

        string json = JsonUtility.ToJson(data, true);
        string encryptedJson = Encrypt(json);
        File.WriteAllText(highScoreFilePath, encryptedJson);

        Debug.Log("High score updated: " + newHighScore);
    }

    // Load planet unlocked status from file
    public static bool[] LoadPlanetUnlockedStatus(int planetCount)
    {
        // Check if the JSON file exists
        if (File.Exists(planetUnlockFilePath))
        {
            string encryptedJson = File.ReadAllText(planetUnlockFilePath);
            string json = Decrypt(encryptedJson);
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
        string encryptedJson = Encrypt(json);
        File.WriteAllText(planetUnlockFilePath, encryptedJson);

        Debug.Log("Planet unlock status updated");
    }

    // Load vehicle unlocked status from file
    public static bool[] LoadVehicleUnlockedStatus(int vehicleCount)
    {
        if (File.Exists(vehicleUnlockFilePath))
        {
            string encryptedJson = File.ReadAllText(vehicleUnlockFilePath);
            string json = Decrypt(encryptedJson);
            VehicleUnlockData data = JsonUtility.FromJson<VehicleUnlockData>(json);

            // Check if the vehicle count has increased since the last save
            if (data.vehicleUnlockedStatus.Length < vehicleCount)
            {
                // Create a new array with the updated vehicle count
                bool[] updatedStatus = new bool[vehicleCount];
                // Copy the existing unlock status values to the new array
                for (int i = 0; i < data.vehicleUnlockedStatus.Length; i++)
                {
                    updatedStatus[i] = data.vehicleUnlockedStatus[i];
                }
                // Set the default status (locked) for the new vehicles
                for (int i = data.vehicleUnlockedStatus.Length; i < vehicleCount; i++)
                {
                    updatedStatus[i] = false; // Lock the newly added vehicles by default
                }

                // Save the updated status to the file
                SaveVehicleUnlockedStatus(updatedStatus);
                Debug.Log("Vehicle unlock status updated with new vehicles.");

                return updatedStatus;
            }
            else
            {
                Debug.Log("Vehicle unlock status loaded from " + vehicleUnlockFilePath);
                return data.vehicleUnlockedStatus;
            }
        }
        else
        {
            // If the file doesn't exist, create a new one with the default status
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

    public static void SaveVehicleUnlockedStatus(bool[] status)
    {
        VehicleUnlockData data = new VehicleUnlockData { vehicleUnlockedStatus = status };

        string json = JsonUtility.ToJson(data, true);
        string encryptedJson = Encrypt(json);
        File.WriteAllText(vehicleUnlockFilePath, encryptedJson);

        Debug.Log("Vehicle unlock status updated");
    }

    // Simple encryption using XOR
    private static string Encrypt(string input)
    {
        char[] key = encryptionKey.ToCharArray();
        char[] inputArray = input.ToCharArray();
        char[] outputArray = new char[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            outputArray[i] = (char)(inputArray[i] ^ key[i % key.Length]);
        }

        return new string(outputArray);
    }

    // Simple decryption using XOR
    private static string Decrypt(string input)
    {
        return Encrypt(input); // XOR encryption is symmetrical
    }
}

