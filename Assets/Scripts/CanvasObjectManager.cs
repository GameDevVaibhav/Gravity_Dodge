using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasObjectManager : MonoBehaviour
{
    [SerializeField]
    Button vehicleSelect;
    [SerializeField]
    Button planetSelect;
    [SerializeField]
    Button backButton;
    [SerializeField]
    Button planetBackButton;

    [SerializeField]
    GameObject vehicleSelectPanel;

    [SerializeField]
    GameObject planetSelectPanel;


    private void Awake()
    {
        vehicleSelect.onClick.AddListener(()=>TogglePanels(vehicleSelectPanel,true));
        backButton.onClick.AddListener(()=>TogglePanels(vehicleSelectPanel,false));

        planetSelect.onClick.AddListener(() => TogglePanels(planetSelectPanel, true));
        planetBackButton.onClick.AddListener(() => TogglePanels(planetSelectPanel, false));

    }

    void TogglePanels(GameObject panel,bool active)
    {
        panel.SetActive(active);
    }
}
