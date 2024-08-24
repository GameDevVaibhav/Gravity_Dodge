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
    public TextMeshProUGUI countdownText;

    public Player player;

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

        homeButton.onClick.AddListener(Home);
    }

    private void Start()
    {
        UpdateGameState(GameState.Menu);
    }

    public void UpdateGameState(GameState newState)
    {
        currentState = newState;

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
        
        gameOverUI.SetActive(false);

        scoreSystem.ResetScore();
        // Stop other game mechanics
    }

    void EnterPlayState()
    {
        // Start the game: planet rotation, object emergence, collectibles, etc.
        
        // Enable other game mechanics
        
        scoreSystem.StartScore();
    }

    void EnterGameOverState()
    {
        // Handle game over logic
        scoreSystem.StopScore();
        ShowGameOverUI();
        CollectibleManager.Instance.SaveCollectibleCounts();
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine(PlayDelay());
        
    }

    IEnumerator PlayDelay()
    {
        playButton.SetActive(false);
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
    public void OnPlanetSwitchButtonClicked()
    {
        // Logic to switch the planet
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

        StartCoroutine(PlayDelay());
    }

    void Home()
    {
        UpdateGameState(GameState.Menu);
        OnHome?.Invoke();
    } 
}
