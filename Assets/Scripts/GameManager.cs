using UnityEngine;

public enum GameState
{
    Menu,
    Play,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentState = GameState.Menu;
    public GameObject playButton;
    public GameObject planetSwitchButton;
    ScoreSystem scoreSystem;

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
        planetSwitchButton.SetActive(true);
        // Stop other game mechanics
    }

    void EnterPlayState()
    {
        // Start the game: planet rotation, object emergence, collectibles, etc.
        playButton.SetActive(false);
        planetSwitchButton.SetActive(false);
        // Enable other game mechanics
        scoreSystem.StartScore();
    }

    void EnterGameOverState()
    {
        // Handle game over logic
        scoreSystem.StartScore();
    }

    public void OnPlayButtonClicked()
    {
        UpdateGameState(GameState.Play);
    }

    public void OnPlanetSwitchButtonClicked()
    {
        // Logic to switch the planet
    }
}
