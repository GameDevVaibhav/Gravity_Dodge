using UnityEngine;
using UnityEngine.UI;

public class PlanetSwitcher : MonoBehaviour
{
    public GameObject[] planetPrefabs; // Array to hold different planet prefabs
    public Transform planetContainer;  // Reference to the container for the planet
    public Button switchButton;        // Reference to the UI button

    private int currentPlanetIndex = 0; // To keep track of the current planet

    void Start()
    {
        // Ensure the first planet is instantiated at the start
        LoadPlanet(currentPlanetIndex);

        // Add a listener to the button
        switchButton.onClick.AddListener(SwitchPlanet);
    }

    void SwitchPlanet()
    {
        // Destroy the current planet in the container
        if (planetContainer.childCount > 0)
        {
            Destroy(planetContainer.GetChild(0).gameObject);
        }

        // Load the next planet in the array
        currentPlanetIndex = (currentPlanetIndex + 1) % planetPrefabs.Length;
        LoadPlanet(currentPlanetIndex);
    }

    void LoadPlanet(int index)
    {
        // Instantiate the planet prefab and set it as a child of the planetContainer
        GameObject planet = Instantiate(planetPrefabs[index], planetContainer);
        planet.transform.localPosition = Vector3.zero; // Reset the position
    }
}
