using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUnlockUI : MonoBehaviour
{
    public GameObject conditionEntryPrefab; // The prefab for displaying a condition
    public Transform conditionsContainer; // The container for all condition entries

    private PlanetUnlockCondition planetUnlockCondition;
    public PlanetSwitcher planetSwitcher;

    private void Start()
    {
        // Get the PlanetUnlockCondition component
        planetUnlockCondition = FindObjectOfType<PlanetUnlockCondition>();
        UpdateUnlockConditionsUI();
    }

    public void UpdateUnlockConditionsUI()
    {
        // Clear existing condition entries
        foreach (Transform child in conditionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Get the unlock conditions for the current planet
        int currentPlanetIndex = planetSwitcher.currentPlanetIndex; // Ensure you have a method to get the current planet index
        PlanetUnlockConditions unlockConditions = planetUnlockCondition.planetUnlockConditions[currentPlanetIndex];

        Dictionary<Collectible.CollectibleType, int> collectibleCounts = DataLoader.LoadCollectibleCounts();

        foreach (UnlockCondition condition in unlockConditions.unlockConditions)
        {
            // Instantiate a new condition entry
            GameObject conditionEntry = Instantiate(conditionEntryPrefab, conditionsContainer);

            // Get the components from the instantiated prefab
            Image collectibleImage = conditionEntry.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI progressText = conditionEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            // Set the collectible sprite
            Sprite collectibleSprite = CollectibleSpriteManager.Instance.GetSpriteForType(condition.collectibleType);
            collectibleImage.sprite = collectibleSprite;

            // Calculate the current amount of collected items and set the progress text
            int currentAmount = collectibleCounts.ContainsKey(condition.collectibleType) ? collectibleCounts[condition.collectibleType] : 0;
            Debug.Log(currentAmount);
            progressText.text = $"{currentAmount}/{condition.requiredAmount}";

            // Optionally, you could disable the prefab if the condition is met
            if (currentAmount >= condition.requiredAmount)
            {
                progressText.color = Color.green; // Indicate completion
            }
            else
            {
                progressText.color = Color.red; // Indicate remaining
            }
        }
    }
}
