using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource backgroundMusic;    // Reference to the AudioSource playing the background music
    public Button soundToggleButton;       // Reference to the sound toggle button
    public Sprite soundOnSprite;           // Sprite for sound on
    public Sprite soundOffSprite;          // Sprite for sound off

    public AudioSource ambientAudioSource; // Reference to the AudioSource for ambient sounds
    public AudioClip[] ambientSounds;      // Array to store ambient audio clips

    private bool isSoundOn = true;         // Track the sound state

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

        // Add listener to the button
        soundToggleButton.onClick.AddListener(ToggleSound);

        // Ensure there's an AudioSource for ambient sounds
        if (ambientAudioSource == null)
        {
            ambientAudioSource = gameObject.AddComponent<AudioSource>();
            ambientAudioSource.loop = true;   // Set ambient sound to loop by default
            ambientAudioSource.playOnAwake = false; // Don't play ambient sound automatically
        }
    }

    // Method to toggle sound on/off
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        //HandleBackgroundMusic();
        HandleAmbientMusic();
        UpdateButtonSprite(); // Update the button sprite based on the sound state
    }

    // Method to play an ambient sound based on the index
    public void PlayAmbientSound(int index)
    {
        if (isSoundOn && index >= 0 && index < ambientSounds.Length && ambientSounds[index] != null)
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
        if (currentState==GameState.Play && isSoundOn) // Check if we're in play mode and sound is on
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
        if (isSoundOn)
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
