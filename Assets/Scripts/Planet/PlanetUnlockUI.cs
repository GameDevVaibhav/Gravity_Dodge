using DG.Tweening;
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
    public TextMeshProUGUI overallProgressText;  // Text to display the overall progress percentage

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
        UpdateUnlockConditionsUI(conditionsContainer2, planetIndex);
    }

    public void UpdateUnlockConditionsUI(Transform conditionsContainer, int planetIndex = -1)
    {
        // Clear existing condition entries
        foreach (Transform child in conditionsContainer)
        {
            Destroy(child.gameObject);
        }

        

        // Use the provided planet index or the current one from the PlanetSwitcher
        int currentPlanetIndex = planetIndex >= 0 ? planetIndex : planetSwitcher.currentPlanetIndex;

        // Check if the planet is unlocked; if it is, don't instantiate the conditions
        if (planetSwitcher.planetUnlockedStatus[currentPlanetIndex])
        {
            Debug.Log("Planet is already unlocked, skipping condition instantiation.");
            return;  // Exit the method if the planet is unlocked
        }

        PlanetUnlockConditions unlockConditions = planetUnlockCondition.planetUnlockConditions[currentPlanetIndex];

        Dictionary<Collectible.CollectibleType, int> collectibleCounts = DataLoader.LoadCollectibleCounts();

        // Variables to keep track of overall progress
        int totalRequiredAmount = 0;
        int totalCurrentAmount = 0;

        foreach (UnlockCondition condition in unlockConditions.unlockConditions)
        {


            // Instantiate a new condition entry
            GameObject conditionEntry = Instantiate(conditionEntryPrefab, conditionsContainer);

            // Set the initial scale to zero for "pop-in" effect
            conditionEntry.transform.localScale = Vector3.zero;

            // Animate the scale from 0 to 1 using DoTween to create a pop effect
            conditionEntry.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);  // Customize ease and duration as needed

            // Get the components from the instantiated prefab
            Image collectibleImage = conditionEntry.transform.GetChild(3).GetComponent<Image>();
            TextMeshProUGUI progressText = conditionEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            GameObject doneTick=conditionEntry.transform.GetChild(2).gameObject;

            // Set the collectible sprite
            Sprite collectibleSprite = CollectibleSpriteManager.Instance.GetSpriteForType(condition.collectibleType);
            collectibleImage.sprite = collectibleSprite;

            // Calculate the current amount of collected items and set the progress text
            int currentAmount = collectibleCounts.ContainsKey(condition.collectibleType) ? collectibleCounts[condition.collectibleType] : 0;
            // Cap the current amount to the required amount (if currentAmount > requiredAmount, use requiredAmount)
            int cappedCurrentAmount = Mathf.Min(currentAmount, condition.requiredAmount);
            progressText.text = $"{cappedCurrentAmount}/{condition.requiredAmount}";

            if (currentAmount>condition.requiredAmount)
            {
                doneTick.SetActive(true);
                
            }

            // Add the required and current amounts to the totals for overall progress tracking
            totalRequiredAmount += condition.requiredAmount;
            totalCurrentAmount += cappedCurrentAmount;
        }

        // Calculate the overall progress percentage
        float overallProgress = (float)totalCurrentAmount / totalRequiredAmount * 100;

        LockCover lockCover=FindObjectOfType<LockCover>();
        // Call the LockCover script to update the shader property based on the progress percentage
        if (lockCover != null)
        {
            Debug.Log("progress: " + overallProgress);
            lockCover.UpdateCoverProgress(overallProgress);
        }
    }
}
