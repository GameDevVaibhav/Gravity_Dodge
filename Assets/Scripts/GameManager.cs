using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum GameState
{
    Menu,
    Play,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action OnGameRestart;
    public static event Action OnHome;

    public GameState currentState = GameState.Menu;
    public GameObject playButton;
    public GameObject planetSwitchButton;
    public Button homeButton;
    ScoreSystem scoreSystem;
    public GameObject gameOverUI;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI countdownText;
    public GameObject conditionsUI;
    public GameObject optionsButtonGroup;

    public Player player;
    public PlanetRotation planet;

    public PlanetSwitcher planetSwitcher;
    public PlanetUnlockUI planetUnlockUI;
    public VehicleUnlockUI vehicleUnlockUI;
    public int PlanetCount;

    private int currentHighScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        scoreSystem = gameObject.GetComponent<ScoreSystem>();

        player=FindObjectOfType<Player>();
        

        planetSwitcher = FindObjectOfType<PlanetSwitcher>();
        PlanetCount = planetSwitcher.planetPrefabs.Length;

        homeButton.onClick.AddListener(Home);
    }

    private void Start()
    {
        
        UpdateGameState(GameState.Menu);
        currentHighScore = DataLoader.LoadHighScore();
    }

    public void UpdateGameState(GameState newState)
    {
        currentState = newState;

        SoundManager.Instance.HandleBackgroundMusic(currentState);

        switch (currentState)
        {
            case GameState.Menu:
                EnterMenuState();
                break;
            case GameState.Play:
                EnterPlayState();
                break;
            case GameState.GameOver:
                EnterGameOverState();
                break;
        }
    }

    void EnterMenuState()
    {
        // Enable planet switching but disable planet rotation and other interactions
        playButton.SetActive(true);

        

        conditionsUI.SetActive(true);

        optionsButtonGroup.SetActive(true);
        
        gameOverUI.SetActive(false);

        scoreSystem.ResetScore();

        FindObjectOfType<PlanetUnlockCondition>().CheckAllUnlockStatuses();
        FindAnyObjectByType<VehicleUnlockCondition>().CheckAllUnlockStatuses();
        FindAnyObjectByType<VehicleSelectionManager>().LoadVehicleUnlockStatus();

        planetSwitcher.PlanetUnlockStatusUpdate();
        planetUnlockUI.UpdateConditionsContainer1();
        vehicleUnlockUI.UpdateUnlockConditionsUI();


        // Stop other game mechanics
    }

    void EnterPlayState()
    {
        
        
        scoreSystem.StartScore();
        
    }

    void EnterGameOverState()
    {
        // Handle game over logic
        scoreSystem.StopScore();
        ShowGameOverUI();
        CollectibleManager.Instance.SaveCollectibleCounts();

        int finalScore = scoreSystem.GetCurrentScore();

        this.finalScore.text=finalScore.ToString();

        if (finalScore > currentHighScore)
        {
            DataLoader.SaveHighScore(finalScore); // Update high score if necessary
            currentHighScore = finalScore;
        }
    }

    public void OnPlayButtonClicked()
    {
        // Check if the selected planet is locked
        if (planetSwitcher.IsCurrentPlanetLocked())
        {
            NotificationManager.Instance.ShowNotification("Planet is Locked!");
            return; // Exit the method if the planet is locked
        }
        conditionsUI.SetActive(false);
        playButton.GetComponent<PopUpEffect>().PopIn();
        //playButton.SetActive(false);
        optionsButtonGroup.SetActive(false);

        StartCoroutine(PlayDelay());
        
    }

    IEnumerator PlayDelay()
    {
        
       // planetSwitchButton.SetActive(false);
        countdownText.gameObject.SetActive(true);
        // Countdown logic
        if (countdownText != null)
        {
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            countdownText.text = ""; // Clear countdown text after finishing
            countdownText.gameObject.SetActive(false);
        }

        
       
        UpdateGameState(GameState.Play);
        StartCoroutine(SetPlayerInvincible());
    }
    
    IEnumerator SetPlayerInvincible()
    {
        player.SetInvincible(true);
        yield return new WaitForSeconds (3f);
        player.SetInvincible(false);
    }
   

    private void ShowGameOverUI()
    {
        // Enable the GameOver UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

    }
    public void RestartGame()
    {
        scoreSystem.ResetScore();

        OnGameRestart?.Invoke();

        gameOverUI.SetActive(false);

        player.ActivatePlayer();

        // Reset planet rotation direction when restarting
        FindObjectOfType<PlanetRotation>().ResetRotationDirection();

        StartCoroutine(PlayDelay());
    }

    void Home()
    {
        UpdateGameState(GameState.Menu);
        OnHome?.Invoke();

        player.ActivatePlayer();
    } 
}
