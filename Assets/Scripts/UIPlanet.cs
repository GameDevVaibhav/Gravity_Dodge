using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanet : MonoBehaviour
{
    public static UIPlanet Instance;

    public GameObject[] planets;

    public PlanetSwitcher planetSwitcher;
    public PlanetUnlockUI planetUnlockUI;
    private int equippedPlanetIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }
    public void ActivateDisplayPlanet(int index)
    {
        if (gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        Instantiate(planets[index], transform);
    }

    public void ResetDisplayToCurrentPlanet()
    {
        equippedPlanetIndex = planetSwitcher.currentPlanetIndex;

        ActivateDisplayPlanet(equippedPlanetIndex);
        
        planetSwitcher.LoadPlanet(equippedPlanetIndex); 

    }
    
}
