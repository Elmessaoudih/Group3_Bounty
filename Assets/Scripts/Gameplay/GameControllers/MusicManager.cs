using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    // Reference to this MusicManager, for use in other scripts
    public static MusicManager instance;

    // Array to hold your music tracks
    [SerializeField] public AudioClip[] musicTracks; 

    // Reference to the Slider the Player can use to control musicVolume upon pressing the ESC key
    [SerializeField] private Slider volumeSlider;
    // The AudioSource that plays the music
    [HideInInspector] public AudioSource audioSource;

    // Volume control variable, restricted to values from 0f to 1f
    [SerializeField, Range(0f, 1f)]
    private float musicVolume = 0.5f; 

    // PlayerPrefs key for storing the volume
    private const string MusicVolumePrefKey = "MusicVolume"; 

    private void Awake()
    {
        // Initialize instance, create an AudioSource, and disable it's loop property
        instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Ensure the audio doesn't loop automatically

        // Load the saved volume from PlayerPrefs (default to 0.5 if not set)
        musicVolume = PlayerPrefs.GetFloat(MusicVolumePrefKey, 0.5f);
        audioSource.volume = musicVolume;

        // Check if there are tracks available
        if (musicTracks.Length == 0)
        {
            Debug.LogError("No music tracks found in the Resources/Audio folder.");
            return;
        }

        // Start playing music
        PlayRandomTrack();
    }

    private void Start()
    {
        // Initialize the slider value to the saved audio source volume
        volumeSlider.value = musicVolume;

        // Add listener to handle slider value changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Method to set audio source volume and save it to PlayerPrefs
    private void SetVolume(float volume)
    {
        musicVolume = volume;

        // Apply the slider's volume to the audio source
        audioSource.volume = volume;
        // Save the volume to PlayerPrefs
        PlayerPrefs.SetFloat(MusicVolumePrefKey, volume);
        // Ensure the data is saved to disk
        PlayerPrefs.Save(); 
    }

    private void Update()
    {
        // Check if the audio has finished playing
        if (!audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    private void PlayRandomTrack()
    {
        // Randomly select a track from the list
        int randomIndex = Random.Range(0, musicTracks.Length);
        AudioClip selectedTrack = musicTracks[randomIndex];

        // Set the selected track to the audio source and play
        audioSource.clip = selectedTrack;
        audioSource.Play();
    }

    // Method to fade out current audio and play new track
    public void PlayClipWithFade(AudioClip newClip)
    {
        StartCoroutine(FadeOutAndPlayNewClip(newClip));
    }

    // Coroutine used to fade out current audio into newClip
    private IEnumerator FadeOutAndPlayNewClip(AudioClip newClip)
    {
        // Initial volume which will be faded out
        float startVolume = audioSource.volume;

        // Fade out over 0.25 seconds
        float fadeDuration = 0.25f;
        float fadeSpeed = startVolume / fadeDuration;

        // Gradually reduce volume to 0
        while (audioSource.volume > 0)
        {
            audioSource.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        // Stop the current track, reset volume, and play the new track
        audioSource.Stop();
        // Reset to current volume level
        audioSource.volume = musicVolume; 

        audioSource.clip = newClip;
        audioSource.Play();
    }
}
