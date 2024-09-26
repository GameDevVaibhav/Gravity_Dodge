using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource backgroundMusic;    // Reference to the AudioSource playing the background music
    public Toggle musicToggle;             // Reference to the toggle for background music
    public Toggle ambientToggle;           // Reference to the toggle for ambient sound
    public Button soundToggleButton;       // Reference to the sound toggle button
    public Sprite soundOnSprite;           // Sprite for sound on
    public Sprite soundOffSprite;          // Sprite for sound off

    public AudioSource ambientAudioSource; // Reference to the AudioSource for ambient sounds
    public AudioClip[] ambientSounds;      // Array to store ambient audio clips

    private bool isSoundOn = true;         // Track the global sound state

    private void Awake()
    {
        // Ensure there's only one instance of the SoundManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set initial button sprite
        UpdateButtonSprite();

        // Add listener to the sound toggle button
        soundToggleButton.onClick.AddListener(ToggleSound);

        // Add listeners to the music and ambient toggles
        musicToggle.onValueChanged.AddListener(delegate { ToggleMusic(); });
        ambientToggle.onValueChanged.AddListener(delegate { ToggleAmbient(); });

        // Ensure there's an AudioSource for ambient sounds
        if (ambientAudioSource == null)
        {
            ambientAudioSource = gameObject.AddComponent<AudioSource>();
            ambientAudioSource.loop = true;   // Set ambient sound to loop by default
            ambientAudioSource.playOnAwake = false; // Don't play ambient sound automatically
        }
    }

    // Method to toggle global sound on/off
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        HandleBackgroundMusic(GameManager.Instance.currentState);
        HandleAmbientMusic();
        UpdateButtonSprite(); // Update the button sprite based on the sound state
    }

    // Method to toggle background music
    public void ToggleMusic()
    {
        HandleBackgroundMusic(GameManager.Instance.currentState);
    }

    // Method to toggle ambient sound
    public void ToggleAmbient()
    {
        HandleAmbientMusic();
    }

    // Method to play an ambient sound based on the index
    public void PlayAmbientSound(int index)
    {
        if (isSoundOn && ambientToggle.isOn && index >= 0 && index < ambientSounds.Length && ambientSounds[index] != null)
        {
            ambientAudioSource.clip = ambientSounds[index];
            ambientAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Invalid audio index or clip not assigned.");
        }
    }

    // Method to stop the ambient sound
    public void StopAmbientSound()
    {
        if (ambientAudioSource.isPlaying)
        {
            ambientAudioSource.Stop();
        }
    }

    // Method to handle background music based on game state
    public void HandleBackgroundMusic(GameState currentState)
    {
        if (isSoundOn && musicToggle.isOn && currentState == GameState.Play) // Check if we're in play mode, sound is on, and music is enabled
        {
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
        }
        else
        {
            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
        }
    }

    // Method to handle ambient music based on sound state
    public void HandleAmbientMusic()
    {
        if (isSoundOn && ambientToggle.isOn) // Check if ambient sound is enabled and sound is on
        {
            if (!ambientAudioSource.isPlaying)
            {
                ambientAudioSource.Play();
            }
        }
        else
        {
            ambientAudioSource.Pause();
        }
    }

    // Method to update the button sprite
    private void UpdateButtonSprite()
    {
        if (isSoundOn)
        {
            soundToggleButton.GetComponent<Image>().sprite = soundOnSprite;
        }
        else
        {
            soundToggleButton.GetComponent<Image>().sprite = soundOffSprite;
        }
    }
}
