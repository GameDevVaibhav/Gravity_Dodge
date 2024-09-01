using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUnlockUI : MonoBehaviour
{
    public GameObject conditionEntryPrefab; // The prefab for displaying a condition
    public Transform conditionsContainer1; // The container for all condition entries
    public Transform conditionsContainer2;

    public PlanetUnlockCondition planetUnlockCondition;
    public PlanetSwitcher planetSwitcher;

    private void Start()
    {
        // Get the PlanetUnlockCondition component
        planetUnlockCondition = FindObjectOfType<PlanetUnlockCondition>();
        UpdateConditionsContainer1();
    }

    public void UpdateConditionsContainer1()
    {
        UpdateUnlockConditionsUI(conditionsContainer1);
    }
    public void UpdateConditionsContainer2(int planetIndex)
    {
        UpdateUnlockConditionsUI(conditionsContainer2,planetIndex);
    }

    public void UpdateUnlockConditionsUI(Transform conditionsContainer,int planetIndex=-1)
    {
        // Clear existing condition entries
        foreach (Transform child in conditionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Use the provided planet index or the current one from the PlanetSwitcher
        int currentPlanetIndex = planetIndex >= 0 ? planetIndex : planetSwitcher.currentPlanetIndex;
        PlanetUnlockConditions unlockConditions = planetUnlockCondition.planetUnlockConditions[currentPlanetIndex];

        Dictionary<Collectible.CollectibleType, int> collectibleCounts = DataLoader.LoadCollectibleCounts();

        foreach (UnlockCondition condition in unlockConditions.unlockConditions)
        {
            // Instantiate a new condition entry
            GameObject conditionEntry = Instantiate(conditionEntryPrefab, conditionsContainer);

            // Get the components from the instantiated prefab
            Image collectibleImage = conditionEntry.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI progressText = conditionEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            // Set the collectible sprite
            Sprite collectibleSprite = CollectibleSpriteManager.Instance.GetSpriteForType(condition.collectibleType);
            collectibleImage.sprite = collectibleSprite;

            // Calculate the current amount of collected items and set the progress text
            int currentAmount = collectibleCounts.ContainsKey(condition.collectibleType) ? collectibleCounts[condition.collectibleType] : 0;
            Debug.Log(currentAmount);
            progressText.text = $"{currentAmount}/{condition.requiredAmount}";

           
        }
    }


}
