using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // Make sure you have DoTween imported

public class CanvasObjectManager : MonoBehaviour
{
    [SerializeField]
    Button vehicleSelect;
    [SerializeField]
    Button planetSelect;
    [SerializeField]
    Button settings;
    [SerializeField]
    Button backButton;
    [SerializeField]
    Button planetBackButton;
    [SerializeField]
    Button settingsBackButton;

    [SerializeField]
    GameObject vehicleSelectPanel;

    [SerializeField]
    GameObject planetSelectPanel;

    [SerializeField]
    GameObject planetUICamera;

    [SerializeField]
    GameObject vehicleUICamera;

    [SerializeField]
    GameObject settingsPanel;
    public float animationDuration = 0.5f; // Duration for the animation

    private void Awake()
    {
        vehicleSelect.onClick.AddListener(() => TogglePanels(vehicleSelectPanel, vehicleSelect, true));
        backButton.onClick.AddListener(() => TogglePanels(vehicleSelectPanel, vehicleSelect, false));

        planetSelect.onClick.AddListener(() => TogglePanels(planetSelectPanel, planetSelect, true));
        planetBackButton.onClick.AddListener(() => TogglePanels(planetSelectPanel, planetSelect, false));

        settings.onClick.AddListener(() => TogglePanels(settingsPanel, settings, true));
        settingsBackButton.onClick.AddListener(() => TogglePanels(settingsPanel, settings, false));
    }

    // Method to toggle panel with animation
    void TogglePanels(GameObject panel, Button sourceButton, bool active)
    {
        if (active)
        {
            // Show panel with pop-up animation
            AnimatePanelOpen(panel, sourceButton);
        }
        else
        {
            // Hide panel with reverse animation
            AnimatePanelClose(panel, sourceButton);

            
        }
    }

    // Method to animate the panel opening (popping out from the button)
    void AnimatePanelOpen(GameObject panel, Button sourceButton)
    {
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        RectTransform buttonRect = sourceButton.GetComponent<RectTransform>();

        // Set initial position and scale of the panel to match the button
        panelRect.localScale = Vector3.zero;
        panelRect.position = buttonRect.position;

        panel.SetActive(true); // Activate the panel

        // Animate position and scale
        panelRect.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
        panelRect.DOMove(transform.position, animationDuration).SetEase(Ease.OutBack);

        if(panel==planetSelectPanel)
        {
            planetUICamera.SetActive(true);
        }

        if(panel==vehicleSelectPanel) 
        {
            vehicleUICamera.SetActive(true);
        }
    }

    // Method to animate the panel closing (popping back to the button)
    void AnimatePanelClose(GameObject panel, Button sourceButton)
    {
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        RectTransform buttonRect = sourceButton.GetComponent<RectTransform>();

        // Animate position and scale to "shrink" the panel back into the button
        panelRect.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack);
        panelRect.DOMove(buttonRect.position, animationDuration).SetEase(Ease.InBack)
            .OnComplete(() => panel.SetActive(false)); // Deactivate panel after animation


        if(panel == planetSelectPanel)
        {
            UIPlanet.Instance.ResetDisplayToCurrentPlanet();
            planetUICamera.SetActive(false);
        }

        if(panel == vehicleSelectPanel)
        {
            vehicleUICamera.SetActive(false);
        }
    }
}
