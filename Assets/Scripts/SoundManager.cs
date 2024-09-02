using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource backgroundMusic; // Reference to the AudioSource playing the music
    public Button soundToggleButton; // Reference to the sound toggle button
    public Sprite soundOnSprite; // Sprite for sound on
    public Sprite soundOffSprite; // Sprite for sound off

    private bool isSoundOn = true; // Track the sound state

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
    }

    // Method to toggle sound on/off
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
        {
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.Pause();
        }

        UpdateButtonSprite(); // Update the button sprite based on the sound state
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
