using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetSwitcher : MonoBehaviour
{
    public GameObject[] planetPrefabs; // Array to hold different planet prefabs
    private bool[] planetUnlockedStatus;
    public Transform planetContainer;  // Reference to the container for the planet

    private int currentPlanetIndex = 0; // To keep track of the current planet
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool touchStartedOnPlanet = false;
    private bool touchStartedOnUI = false;

    private EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    void OnEnable()
    {
        GameManager.OnHome += OnHomeLoadPlanet; // Subscribe to OnHome event
    }

    void OnDisable()
    {
        GameManager.OnHome -= OnHomeLoadPlanet; // Unsubscribe from OnHome event
    }
    void Start()
    {
        // Load planet unlocked status from JSON
        planetUnlockedStatus = DataLoader.LoadPlanetUnlockedStatus(planetPrefabs.Length);

        // Ensure the first planet is instantiated at the start
        LoadPlanet(currentPlanetIndex);

        // Get the EventSystem and GraphicRaycaster components
        eventSystem = EventSystem.current;
       
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Menu) { return; }
       
        DetectSwipe();
    }

    void DetectSwipe()
    {
        // For touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                touchStartedOnPlanet = IsTouchOnPlanet(touch.position);
                touchStartedOnUI=IsPointerOverUIObject();

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!touchStartedOnPlanet && !touchStartedOnUI)
                {
                    endTouchPosition = touch.position;
                    HandleSwipe();
                }
            }
        }
        // For mouse input (useful for testing in the editor)
        else if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            touchStartedOnPlanet = IsTouchOnPlanet(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!touchStartedOnPlanet)
            {
                endTouchPosition = Input.mousePosition;
                HandleSwipe();
            }
        }
    }

    void HandleSwipe()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            if (swipeDirection.x > 0)
            {
                // Right swipe detected
                SwitchToPreviousPlanet();
            }
            else
            {
                // Left swipe detected
                SwitchToNextPlanet();
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        // Create a PointerEventData
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        // Set the position of the pointer
        pointerEventData.position = Input.mousePosition;

       
        // Perform a raycast to check if the pointer is over a UI element
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        // If the raycast hits any UI element, return true
        return raycastResults.Count > 0;
    }

    bool IsTouchOnPlanet(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        Debug.Log(IsPointerOverUIObject());
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the raycast hit the planet
            if (hit.transform.IsChildOf(planetContainer))
            {
                
                return true;
            }
        }

        return false;
    }

    void SwitchToNextPlanet()
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

    void SwitchToPreviousPlanet()
    {
        // Destroy the current planet in the container
        if (planetContainer.childCount > 0)
        {
            Destroy(planetContainer.GetChild(0).gameObject);
        }

        // Load the previous planet in the array
        currentPlanetIndex--;
        if (currentPlanetIndex < 0)
        {
            currentPlanetIndex = planetPrefabs.Length - 1;
        }
        LoadPlanet(currentPlanetIndex);
    }

    void LoadPlanet(int index)
    {
        // Instantiate the planet prefab and set it as a child of the planetContainer
        GameObject planet = Instantiate(planetPrefabs[index], planetContainer);
        planet.transform.localPosition = Vector3.zero; // Reset the position
    }

    void DestroyCurrentPlanet()
    {
        // Destroy the current planet in the container
        if (planetContainer.childCount > 0)
        {
            Destroy(planetContainer.GetChild(0).gameObject);
        }
    }

    void OnHomeLoadPlanet()
    {
        DestroyCurrentPlanet();
        LoadPlanet(currentPlanetIndex);
    }

    public void PlanetUnlockStatusUpdate()
    {
        planetUnlockedStatus= DataLoader.LoadPlanetUnlockedStatus(planetPrefabs.Length);
    }

    public bool IsCurrentPlanetLocked()
    {
        return !planetUnlockedStatus[currentPlanetIndex];
    }
}
