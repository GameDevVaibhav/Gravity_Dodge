using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanet : MonoBehaviour
{
    public static UIPlanet Instance;


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
    public void ActivateDisplayPlanet(GameObject planet)
    {
        if (gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        Instantiate(planet, transform);
    }
}
