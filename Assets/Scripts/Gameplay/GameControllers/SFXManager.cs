using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    // Reference to this SFXManager, for use in other scripts
    public static SFXManager instance;

    // Reference to the Slider the Player can use to control sfxVolume upon pressing the ESC key
    [SerializeField] private Slider volumeSlider;
    // The AudioSource that plays the SFX
    [HideInInspector] public AudioSource audioSource;

    // Volume control variable, restricted to values from 0f to 1f
    [SerializeField, Range(0f, 1f)]
    private float sfxVolume = 0.5f;

    // PlayerPrefs key for storing the volume
    private const string VolumePrefKey = "SFXVolume"; 

    private void Awake()
    {
        // Initialize instance, create an AudioSource, and disable it's loop property
        instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Ensure the audio doesn't loop automatically

        // Load the saved volume from PlayerPrefs (default to 0.5 if not set)
        sfxVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f);
        audioSource.volume = sfxVolume;
    }

    private void Start()
    {
        // Initialize the slider value to the saved audio source volume
        volumeSlider.value = sfxVolume;

        // Add listener to handle slider value changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Method to set audio source volume and save it to PlayerPrefs
    private void SetVolume(float volume)
    {
        sfxVolume = volume;

        // Apply the slider's volume to the audio source
        audioSource.volume = volume;
        // Save the volume to PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        // Ensure the data is saved to disk
        PlayerPrefs.Save(); 
    }

    // Method to stop current SFX audio and play new track
    public void PlayClip(AudioClip newClip)
    {
        // Stop current clip
        audioSource.Stop();

        // Reset to current volume level
        audioSource.volume = sfxVolume; 

        // Play newClip
        audioSource.clip = newClip;
        audioSource.Play();
    }
}
