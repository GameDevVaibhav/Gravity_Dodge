using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetSelectionManager : MonoBehaviour
{
    public GameObject planetButtonPrefab; // Prefab for the planet selection button
    public Transform planetButtonHolder; // UI container for the planet buttons

    [Header("Planet Info Display")]
    public Image selectedPlanetThumbnail; // UI element to display the planet thumbnail
    public TextMeshProUGUI selectedPlanetName; // UI element to display the planet name
    public TextMeshProUGUI ObstacleName; // UI element to display the planet description
    public TextMeshProUGUI selectedPlanetIcon;
    public Image ObstacleIcon;
    public Image CollectibleIcon;
    public Button equipButton;
    public TextMeshProUGUI equipButtonText; // Reference to the equip button's text

    public PlanetUnlockUI planetUnlockUI;

    private PlanetSwitcher planetSwitcher; // Reference to the PlanetSwitcher script
    private PlanetInfo selectedPlanetInfo; // Holds the currently selected planet's info

    void Start()
    {
        planetSwitcher = FindObjectOfType<PlanetSwitcher>(); // Get the PlanetSwitcher instance
        PopulatePlanetSelectionUI(); // Populate the planet selection UI
        equipButton.onClick.AddListener(EquipSelectedPlanet);
        SelectPlanet(0);
    }

    private void PopulatePlanetSelectionUI()
    {
        for (int i = 0; i < planetSwitcher.planetPrefabs.Length; i++)
        {
            // Instantiate the planet button prefab and set it as a child of the button holder
            GameObject button = Instantiate(planetButtonPrefab, planetButtonHolder);

            // Get the PlanetInfo component from the planet prefab
            PlanetInfo planetInfo = planetSwitcher.GetPlanetInfo(i);

            // Populate the button with the planet data
            button.transform.Find("Icon").GetComponent<Image>().sprite = planetInfo.planetSprite;
            button.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = planetInfo.planetName;

            int index = i;
            // Add an onClick listener to select the planet
            button.GetComponent<Button>().onClick.AddListener(() => SelectPlanet(index));
        }
    }

    public void SelectPlanet(int index)
    {
        selectedPlanetInfo = planetSwitcher.GetPlanetInfo(index); // Get the selected planet's info
        UpdatePlanetInfoUI(index); // Update the UI with the selected planet's info

        // Update equip button text based on the selected planet's lock status
        if (planetSwitcher.planetUnlockedStatus[index])
        {
            if (planetSwitcher.currentPlanetIndex == index)
            {
                equipButtonText.text = "Equipped";
                SetButtonColor("#7B7974");
            }
            else
            {
                equipButtonText.text = "Equip";
                SetButtonColor("#FFD407");
            }
        }
        else
        {
            equipButtonText.text = "Locked";
            SetButtonColor("red");
        }

        UIPlanet.Instance.ActivateDisplayPlanet(selectedPlanetInfo.gameObject);
    }

    private void SetButtonColor(string colorCode)
    {
        // Parse the hex color code and set the button color
        Color color;
        if (ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            Image buttonImage = equipButton.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = color;  // Set the button's background color
            }
        }
    }

    private void UpdatePlanetInfoUI(int index)
    {
        if (selectedPlanetInfo != null)
        {
            selectedPlanetThumbnail.sprite = selectedPlanetInfo.planetSprite;
            selectedPlanetName.text = selectedPlanetInfo.planetName;
            ObstacleName.text = selectedPlanetInfo.obstacleName;
            ObstacleIcon.sprite = selectedPlanetInfo.obstacleSprite;
            CollectibleIcon.sprite = selectedPlanetInfo.collectibleSprite;

            planetUnlockUI.UpdateConditionsContainer2(index);
        }
    }

    // Call this method when the play button is clicked
    public void EquipSelectedPlanet()
    {
        if (selectedPlanetInfo != null)
        {
            int index = System.Array.IndexOf(planetSwitcher.planetPrefabs, selectedPlanetInfo.gameObject);
            planetSwitcher.currentPlanetIndex = index;

            if (planetSwitcher.planetUnlockedStatus[index])
            {
                planetSwitcher.LoadPlanet(index); // Load the selected planet
                equipButtonText.text = "Equipped"; // Update button text to "Equipped"
            }
            else
            {
                NotificationManager.Instance.ShowNotification("Locked");
            }
        }
    }
}
